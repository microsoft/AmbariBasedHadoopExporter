// <copyright file="BaseExporterConfiguration.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Configurations.Exporters
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Core.Utils;

    /// <summary>
    /// Base class for any ambari based exporter configuration.
    /// </summary>
    public abstract class BaseExporterConfiguration : BaseValidatableConfiguration
    {
        /// <summary>
        /// Gets or sets the Cluster name.
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [RegularExpression(@"^[a-zA-Z]+$")]
        public string ClusterName { get; set; }

        /// <summary>
        /// Gets or sets the base Ambari server uri, e.g. http[s]://{your.ambari.server}/api/v1/clusters
        /// </summary>
        [Required(AllowEmptyStrings = false)]
        [AmbariUri]
        public string AmbariServerUri { get; set; }

        /// <summary>
        /// Gets or sets the default labels that will be appended to all metrics.
        /// </summary>
        public Dictionary<string, string> DefaultLabels { get; set; }

        /// <summary>
        /// Gets the uri endpoint of Ambari's API for the current resource.
        /// </summary>
        public abstract string UriEndpoint { get; }

        /// <summary>
        /// Gets the base uri based on the cluster name and a const infix.
        /// </summary>
        protected string BaseUri => $"{AmbariServerUri}/{ClusterName}";
    }
}
