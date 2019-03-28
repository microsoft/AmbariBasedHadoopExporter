// <copyright file="HdfsDataNodeComponent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.Components
{
    using Core.Models.AmbariResponseEntities.HdfsDataNode;
    using Newtonsoft.Json;

    public class HdfsDataNodeComponent
    {
        [JsonProperty(PropertyName = "ServiceComponentInfo")]
        public ComponentInfo Info { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public HdfsDataNodeMetrics Metrics { get; set; }
    }
}
