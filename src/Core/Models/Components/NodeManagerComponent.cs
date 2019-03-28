// <copyright file="NodeManagerComponent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.Components
{
    using Core.Models.AmbariResponseEntities.YarnNodeManager;
    using Newtonsoft.Json;

    public class NodeManagerComponent
    {
        [JsonProperty(PropertyName = "ServiceComponentInfo")]
        public ComponentInfo Info { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public YarnNodeManagerMetrics Metrics { get; set; }
    }
}
