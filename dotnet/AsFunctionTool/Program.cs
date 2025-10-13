using System;
using System.ClientModel;
using Azure.AI.OpenAI;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Microsoft.Agents.AI;
using Microsoft.Extensions.AI;
using OpenAI;

var oaiEndpoint = 
    Environment.GetEnvironmentVariable("AZURE_OPENAI_ENDPOINT") 
    ?? throw new InvalidOperationException("AZURE_OPENAI_ENDPOINT is not set.");
var oaiDeploymentName = 
    Environment.GetEnvironmentVariable("AZURE_OPENAI_RESPONSES_DEPLOYMENT_NAME") 
    ?? throw new InvalidOperationException("AZURE_OPENAI_RESPONSES_DEPLOYMENT_NAME is not set.");
var oaiKey = 
    Environment.GetEnvironmentVariable("AZURE_OPENAI_KEY") 
    ?? throw new InvalidOperationException("AZURE_OPENAI_KEY is not set.");

var esEndpoint =
    Environment.GetEnvironmentVariable("ELASTICSEARCH_ENDPOINT") 
    ?? throw new InvalidOperationException("ELASTICSEARCH_ENDPOINT is not set.");
var esKey =
    Environment.GetEnvironmentVariable("ELASTICSEARCH_API_KEY") 
    ?? throw new InvalidOperationException("ELASTICSEARCH_API_KEY is not set.");

var eCommercePlugin =
    new ECommerceQuery(
        new ElasticsearchClientSettings(new SingleNodePool(new Uri(esEndpoint)))
            .Authentication(new ApiKey(esKey))
            .EnableDebugMode()
    );

// Create the chat client and agent, and provide the function tool to the agent.
var ecommerceAgent = new AzureOpenAIClient(
        new Uri(oaiEndpoint), 
        new ApiKeyCredential(oaiKey))
    .GetChatClient(oaiDeploymentName)
    .CreateAIAgent(
        instructions: "You are a helpful assistant for an ecommerce backend application.",
        name: "ECommerceAgent",
        description: "An agent that answers questions about orders in an ecommerce system.",
        tools: 
        [
            AIFunctionFactory.Create(eCommercePlugin.QueryCustomersAsync),
            AIFunctionFactory.Create(eCommercePlugin.QueryRevenueAsync)
        ]
    );

// Create the main agent, and provide the ecommerce agent as a function tool.
var agent = new AzureOpenAIClient(
        new Uri(oaiEndpoint),
        new ApiKeyCredential(oaiKey))
    .GetChatClient(oaiDeploymentName)
    .CreateAIAgent("You are a helpful assistant who responds in German.", tools: [ecommerceAgent.AsAIFunction()]);

// Invoke the agent and output the text result.

Console.WriteLine(await agent.RunAsync("Is Eddie Underwood our customer? If so, what is his email?"));
Console.WriteLine(await agent.RunAsync("List all customers with the last name 'Smith'. Limit to 5 results."));
Console.WriteLine(await agent.RunAsync("What are the first three cities with the highest revenue?"));
