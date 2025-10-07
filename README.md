## Elasticsearch code examples for Microsoft Agent Framework

This repository contains some examples in Python and .NET for using the
[Microsoft Agent Framework](https://github.com/microsoft/agent-framework) with Elasticsearch.

This repository is part of the article [Insert the title]() published in the
Elasticsearch Labs website by [Elastic](https://www.elastic.co/).

# Microsoft Agent Framework

Microsof Agetn Framework is a comprehensive multi-language framework for building, orchestrating, and deploying AI agents with support for both .NET and Python implementations. This framework provides everything from simple chat agents to complex multi-agent workflows with graph-based orchestration.

For more information you can read this [Microsoft announcement](https://azure.microsoft.com/en-us/blog/introducing-microsoft-agent-framework/).

# Run Elasticsearch

To execute the examples reported in this repository you need to have an
instance of [Elasticsearch](https://www.elastic.co/elasticsearch) running. You can register for a free trial on
[Elastic Cloud](https://www.elastic.co/cloud/cloud-trial-overview) or install a local instance of Elasticsearch on your computer.

To install locally, you need to execute this command in the terminal:

```bash
curl -fsSL https://elastic.co/start-local | sh
```

This will install Elasticsearch and [Kibana](https://www.elastic.co/kibana) on macOS, Linux and Windows using WSDL.

# Use the sample data in Kibana

The examples reported in this repository use a sample data provided by Kibana.
You need to import this sample data using the following procedure:

- After the login in Kibana, open the navigation menu on the left and select
  **Integrations** page in the navigation bar on the left (Figure 1).

![Figure 1](/img/figure1.png)

- In the Integrations page, search for "sample" and click on **Sample Data** (Figure 2)

![Figure 2](/img/figure2.png)

- Finally, in the Sample data page, click on **Other sample data sets** and click
  on **Add data** for the Sample eCommerce orders use case (Figure 3).

![Figure 3](/img/figure3.png)

The ecommerce data will be stored in an index called `kibana_sample_data_ecommerce` containing about 4,675 orders.

# Configure Azure AI

In the examples, We used Azure OpenAI. You need to configure a `.env`
file containing the following environmental variables:

```
ELASTICSEARCH_ENDPOINT="The endpoint of your Elasticsearch deployment here"
ELASTICSEARCH_API_KEY="The API Key of your Elasticsearch deployment here"

AZURE_OPENAI_ENDPOINT="The Azure OpenAI endpoint here"
AZURE_OPENAI_RESPONSES_DEPLOYMENT_NAME="The Azure OpenAI deployment name here"
AZURE_OPENAI_API_KEY="The API Key of your Azure OpenAI deployment here"
```

You can generate the `.env` file copying if from `.env-dev` file.

If you installed Elasticsearch using [start-local](https://github.com/elastic/start-local), 
you can read the endpoint and api key from the `.env` file of the folder installation.

For Azure OpenAI you can read the value settings in the Azure portal.

# Python examples

To install the Python examples, you can create and activate a virtual
environment ([venv](https://docs.python.org/3/library/venv.html)) in the `python` folder.

Use the following commands from the root folder of the repository:

```bash
cd python
python -m venv .venv
source .venv/bin/activate
```

After, you can install all the required packages as follows:

```bash
pip install -r requirements.txt
```

Now, you can execute the examples. For instance, you can run the `simple_agent_tools.py`
with the following command:

```bash
python simple_agent_tools.py
```

This example build an agent that can interact with Elasticsearch using some tools.
The tools are implemented in the [python/elasticsearch_tools.py](/python/elasticsearch_tools.py) file.

# .NET examples

# License

This software is licensed under the [Apache License 2.0](./LICENSE)
