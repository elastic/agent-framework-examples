using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text.Json;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;

internal sealed class ECommerceQuery
{
    private readonly ElasticsearchClient _client;

    public ECommerceQuery(IElasticsearchClientSettings settings)
    {
        ArgumentNullException.ThrowIfNull(settings);

        this._client = new ElasticsearchClient(settings);
    }

    [Description("Get the customer information for a given name.")]
    public async Task<IEnumerable<JsonElement>> QueryCustomersAsync(
        [Description("The name of the customer to find.")]
        string name,
        [Description("The maximum number of results to return.")]
        int limit = 10)
    {
        IEnumerable<JsonElement> response = await this._client.Esql
            .QueryAsObjectsAsync<JsonElement>(x => x
                .Params(
                    name,
                    limit
                )
                .Query("""
                       FROM kibana_sample_data_ecommerce
                       | WHERE MATCH(customer_full_name, ?1, {"operator": "AND"})
                       | LIMIT ?2
                       """
                )
            )
            .ConfigureAwait(false);

        Console.WriteLine($"-- DEBUG - Tool: QueryCustomersAsync, ES|QL query parameters: name = {name}, limit = {limit}");

        return response;
    }

    [Description("Get the total revenue grouped by city.")]
    public async Task<IEnumerable<JsonElement>> QueryRevenueAsync()
    {
        IEnumerable<JsonElement> response = await this._client.Esql
            .QueryAsObjectsAsync<JsonElement>(x => x
                .Query("""
                       FROM kibana_sample_data_ecommerce
                       | STATS revenue = SUM(taxful_total_price) BY geoip.city_name
                       | SORT revenue DESC
                       | LIMIT 1000
                       """)
            )
            .ConfigureAwait(false);

        Console.WriteLine("-- DEBUG - Tool: QueryRevenueAsync");

        return response;
    }
}
