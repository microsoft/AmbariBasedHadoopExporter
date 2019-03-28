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
        public int NumActiveNMs { get; set; }

        [JsonProperty(PropertyName = "NumDecommissionedNMs")]
        public int NumDecommissionedNMs { get; set; }

        [JsonProperty(PropertyName = "NumLostNMs")]
        public int NumLostNMs { get; set; }

        [JsonProperty(PropertyName = "NumRebootedNMs")]
        public int NumRebootedNMs { get; set; }

        [JsonProperty(PropertyName = "NumUnhealthyNMs")]
        public int NumUnhealthyNMs { get; set; }
    }
}
