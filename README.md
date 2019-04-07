# Introduction
Apache Ambari is an open-source management tool that allows you to administer your Hadoop cluster easly. Ambari uses AMS (Ambari Metrics System) to monitor an collect logs from related Hadoop installtions in your cluster, exposing them to the user in each component dashboard. In addition, you may want to export those metrics to a cenralized tool you are using. Grafana has a built in support for that, which is great.</br>
However, you may want to export AMS's collected metrics to a different source like Prometheus. This repo attepts to try and solve this issue, implementing a **Prometheus exporter for Hadoop based on Ambari API**. </br></br>

> Note - there are open source Prometheus exporters for some Hadoop components like YARN and HDFS, but, Ambari gives you a single point of access to multiple collected metrics for services, components and configuration. Thus our effort building an exporter based on its API.



# Tested environment
We're running on Azure environment, using Azure HDInsight cluster 3.6 running HDP 2.6. <br>
The exporter was deployed on our Kubernetes cluster running [init-container](https://github.com/Hexadite/acs-keyvault-agent) that handles all secrets acquisition from an Azure Key Vault and injection to our containers.



# Build with Docker
1. Clone the project and go to its root folder.
2. Run `docker build . -t ambari-exporter`, this will create a docker image on your local machine, run `docker images ambari-exporter` to validate that the image exists.

# Deploying to Kubernetes

Every service requires settings file to run, an example of full settings file can be found [here](test\ComponentTests\appsettings.json). But, since we're using Kubernetes as our orchestrator, we can leverage  [helm](https://github.com/helm/charts) to inject those values during deployment as environment variables.<br>

## Prerequisites
Install [init-container](https://github.com/Hexadite/acs-keyvault-agent) in your cluster for Azure Key Vault integration. This is used to extract secrets in a secure way.


## Deployment example
View an example deployment helm chart [here](deployment\ambari-based-hadoop-exporter).



# Dependencies
All of the exporters are based on [Ambari's API](https://github.com/apache/ambari/blob/trunk/ambari-server/docs/api/v1/index.md). <br><br>
Implementation dependencies:
1. [Prometheus-net]([https://link](https://github.com/prometheus-net/prometheus-net)) - used to expose the API endpoint for Prometheus.
2. [Serilog](https://github.com/serilog/serilog-aspnetcore) - for formatting log messages.
3. [Newtonsoft](https://github.com/JamesNK/Newtonsoft.Json) - for Json parsing.
4. [FluentAssertions](https://github.com/fluentassertions/fluentassertions), [Moq 4](https://github.com/moq/moq4), [xUnit](https://xunit.github.io/) - for testing.
5. [StyleCop](https://github.com/StyleCop/StyleCop) - for code formatting.




# Support
For any issue, request, question please feel to create an issue, we'll be trying giving you the best support as possible.



# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.microsoft.com.

When you submit a pull request, a CLA-bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., label, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.
