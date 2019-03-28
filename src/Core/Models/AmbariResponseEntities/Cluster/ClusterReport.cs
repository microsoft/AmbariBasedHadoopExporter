// <copyright file="ClusterReport.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.Cluster
{
    using Newtonsoft.Json;

    public class ClusterReport
    {
        [JsonProperty(PropertyName = "cluster_name")]
        public string ClusterName { get; set; }

        [JsonProperty(PropertyName = "health_report")]
        public HealthReport HealthReport { get; set; }
    }
}
