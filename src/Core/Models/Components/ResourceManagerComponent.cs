// <copyright file="ResourceManagerComponent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.Components
{
    using Core.Models.AmbariResponseEntities.YarnResourceManager;
    using Newtonsoft.Json;

    public class ResourceManagerComponent
    {
        [JsonProperty(PropertyName = "ServiceComponentInfo")]
        public ComponentInfo Info { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public YarnResourceManagerMetrics Metrics { get; set; }
    }
}
