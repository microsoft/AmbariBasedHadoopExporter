// <copyright file="YarnNodeManagerExporterConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Configurations.Exporters
{
    /// <summary>
    /// Configuring the Yarn NodeManager exporter required endpoints, based on Ambari API's structure.
    /// </summary>
    public class YarnNodeManagerExporterConfiguration : BaseExporterConfiguration
    {
        public override string UriEndpoint => $"{BaseUri}/services/YARN/components/NODEMANAGER";
    }
}
