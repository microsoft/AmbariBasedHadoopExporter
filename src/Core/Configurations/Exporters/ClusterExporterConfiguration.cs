// <copyright file="ClusterExporterConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Configurations.Exporters
{
    /// <summary>
    /// Configuring the cluster exporter required endpoints, based on Ambari API's structure.
    /// </summary>
    public class ClusterExporterConfiguration : BaseExporterConfiguration
    {
        /// <inheritdoc />
        public override string UriEndpoint => BaseUri;
    }
}
