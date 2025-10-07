import asyncio
import os
from dotenv import load_dotenv

from agent_framework.azure import AzureOpenAIResponsesClient
from azure.identity import AzureCliCredential
from elasticsearch_tools import ElasticsearchTools

"""
Azure AI Chat Client example with Elasticsearch integration

Demonstrates direct AzureAIChatClient usage for chat interactions with Azure AI models.
Shows function calling capabilities with Elasticsearch as a tool.
"""

async def main() -> None:
    tools = ElasticsearchTools()
    agent = AzureOpenAIResponsesClient(credential=AzureCliCredential()).create_agent(
        instructions="You are a helpful assistant for an ecommerce backend application.",
        tools=[tools.find_customer, tools.revenue_by_cities],
    )

    # Example 1: Simple query to find a customer
    query = "Is Eddie Underwood our customer? If so, what is his email?"
    print(f"User: {query}")
    result = await agent.run(query)
    print(f"Result: {result}\n")

    # Example 2: More complex query with limit
    query = "List all customers with the last name 'Smith'. Limit to 5 results."
    print(f"User: {query}")
    result = await agent.run(query)
    print(f"Result: {result}\n")

    # Example 3: What are the first three city with more revenue?
    query = "What are the first three city with more revenue?"
    print(f"User: {query}")
    result = await agent.run(query)
    print(f"Result: {result}\n")

if __name__ == "__main__":
    current_folder = os.path.dirname(os.path.abspath(__file__))
    dotenv_path= current_folder + '/../.env'
    if not os.path.exists(dotenv_path):
        raise FileNotFoundError("Missing .env file. Please create one based on the .env-dev file.")
    load_dotenv(dotenv_path)
    asyncio.run(main())