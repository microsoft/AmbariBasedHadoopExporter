// <copyright file="ComponentInfo.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.Components
{
    using Newtonsoft.Json;

    public class ComponentInfo
    {
        [JsonProperty(PropertyName = "cluster_name")]
        public string ClusterName { get; set; }

        [JsonProperty(PropertyName = "component_name")]
        public string ComponentName { get; set; }

        [JsonProperty(PropertyName = "StartTime")]
        public long StartTime { get; set; }

        [JsonProperty(PropertyName = "started_count")]
        public int StartedCount { get; set; }

        [JsonProperty(PropertyName = "total_count")]
        public int TotalCount { get; set; }

        [JsonProperty(PropertyName = "unknown_count")]
        public int UnknownCount { get; set; }
    }
}
