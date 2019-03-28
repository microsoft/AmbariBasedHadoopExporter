// <copyright file="YarnClusterRelatedMetrics.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class YarnClusterRelatedMetrics
    {
        [JsonProperty(PropertyName = "ClusterMetrics")]
        public YarnResourceManagerClusterMetrics YarnResourceManagerClusterMetrics { get; set; }

        [JsonProperty(PropertyName = "Queue")]
        public Queues Queues { get; set; }
    }
}
