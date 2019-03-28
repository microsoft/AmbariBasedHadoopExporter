// <copyright file="YarnResourceManagerMetrics.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class YarnResourceManagerMetrics
    {
        [JsonProperty(PropertyName = "jvm")]
        public Jvm ResourceManagerJvm { get; set; }

        [JsonProperty(PropertyName = "rpc")]
        public YarnResourceManagerRpc ResourceManagerYarnResourceManagerRpc { get; set; }

        [JsonProperty(PropertyName = "runtime")]
        public Runtime Runtime { get; set; }

        [JsonProperty(PropertyName = "ugi")]
        public Ugi Ugi { get; set; }

        [JsonProperty(PropertyName = "yarn")]
        public YarnClusterRelatedMetrics YarnMetrics { get; set; }
    }
}
