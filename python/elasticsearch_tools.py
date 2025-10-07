import os
from elasticsearch import Elasticsearch
from typing import Annotated
from pydantic import Field

class ElasticsearchTools:
    def __init__(self, url: str = None, api_key: str = None):
        """
        Initialize Elasticsearch client.

        Parameters
        ----------
        url : str, optional
            Elasticsearch cluster URL. If not provided, falls back to
            environment variable ELASTICSEARCH_ENDPOINT.
        api_key : str, optional
            Elasticsearch API key. If not provided, falls back to
            environment variable ELASTICSEARCH_API_KEY.
        """
        self.url = url or os.getenv("ELASTICSEARCH_ENDPOINT")
        self.api_key = api_key or os.getenv("ELASTICSEARCH_API_KEY")

        if not self.url or not self.api_key:
            raise ValueError("Missing Elasticsearch URL or API key.")

        self.client = Elasticsearch(
            [self.url],
            api_key=self.api_key
        )

        # Test connection
        try:
            if not self.client.ping():
                raise ConnectionError("Elasticsearch cluster is not reachable.")
        except Exception as e:
            raise ConnectionError(f"Failed to connect to Elasticsearch: {e}")

    def find_customer(
        self, 
        name: Annotated[str, Field(description="The name of the customer to find.")],
        limit: Annotated[int, Field(description="The maximum number of results to return.", default=10)]
    ) -> str:
        """Get the customer information for a given name."""
        query = f"""
            FROM kibana_sample_data_ecommerce
            | WHERE MATCH(customer_full_name,"{name}", {{"operator": "AND"}})
            | LIMIT {limit}
            """
        response = self.client.esql.query(
            query=query
        )
        print(f"-- DEBUG - Tool: find_customer, ES|QL query: ", query)
    
        if response['documents_found'] == 0:
            return "No customer found."
        return f"Found {response['documents_found']} customer(s) with name {name}:\n {response['values']}"
    
    def revenue_by_cities(
        self
    ) -> str:
        """Get the total revenue grouped by city."""
        query = f"""
            FROM kibana_sample_data_ecommerce
            | STATS revenue = SUM(taxful_total_price) BY geoip.city_name
            | SORT revenue DESC
            | LIMIT 1000
            """
        response = self.client.esql.query(
            query=query
        )
        print(f"-- DEBUG - Tool: revenue_by_cities, ES|QL query: ", query)

        if response['documents_found'] == 0:
            return "No revenue found grouped by city."
        return f"Total revenue grouped by cities:\n {response['values']}"