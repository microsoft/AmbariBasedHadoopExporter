// <copyright file="ClusterComponent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.Components
{
    using System.Collections.Generic;
    using Core.Models.AmbariResponseEntities.Cluster;
    using Newtonsoft.Json;

    public class ClusterComponent
    {
        [JsonProperty(PropertyName = "Clusters")]
        public ClusterReport ClusterReport { get; set; }

        [JsonProperty(PropertyName = "alerts_summary")]
        public AlertsSummary AlertsSummary { get; set; }

        [JsonProperty(PropertyName = "alerts_summary_hosts")]
        public AlertsSummaryHosts AlertsSummaryHosts { get; set; }

        [JsonProperty(PropertyName = "hosts")]
        public List<HostEntry> HostEntryList { get; set; }
    }
}
