// <copyright file="HostDetails.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.ClusterHost
{
    using Newtonsoft.Json;

    public class HostDetails
    {
        [JsonProperty(PropertyName = "cluster_name")]
        public string ClusterName { get; set; }

        [JsonProperty(PropertyName = "host_name")]
        public string HostName { get; set; }
    }
}
