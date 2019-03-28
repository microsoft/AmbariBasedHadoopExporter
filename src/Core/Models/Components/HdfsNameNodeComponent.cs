// <copyright file="HdfsNameNodeComponent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.Components
{
    using Core.Models.AmbariResponseEntities.HdfsNameNode;
    using Newtonsoft.Json;

    public class HdfsNameNodeComponent
    {
        [JsonProperty("ServiceComponentInfo")]
        public ComponentInfo Info { get; set; }

        [JsonProperty("metrics")]
        public HdfsNameNodeMetrics Metrics { get; set; }
    }
}
