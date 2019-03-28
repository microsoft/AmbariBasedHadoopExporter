// <copyright file="ClusterHostComponent.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.Components
{
    using Core.Models.AmbariResponseEntities.ClusterHost;
    using Newtonsoft.Json;

    public class ClusterHostComponent
    {
        [JsonProperty(PropertyName = "Hosts")]
        public HostDetails HostDetails { get; set; }

        [JsonProperty(PropertyName = "metrics")]
        public HostMetrics Metrics { get; set; }
    }
}
