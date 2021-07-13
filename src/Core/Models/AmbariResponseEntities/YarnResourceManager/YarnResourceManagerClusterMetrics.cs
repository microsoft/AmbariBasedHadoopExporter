// <copyright file="YarnResourceManagerClusterMetrics.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class YarnResourceManagerClusterMetrics
    {
        [JsonProperty(PropertyName = "NumActiveNMs")]
        public long NumActiveNMs { get; set; }

        [JsonProperty(PropertyName = "NumDecommissionedNMs")]
        public long NumDecommissionedNMs { get; set; }

        [JsonProperty(PropertyName = "NumLostNMs")]
        public long NumLostNMs { get; set; }

        [JsonProperty(PropertyName = "NumRebootedNMs")]
        public long NumRebootedNMs { get; set; }

        [JsonProperty(PropertyName = "NumUnhealthyNMs")]
        public long NumUnhealthyNMs { get; set; }
    }
}
