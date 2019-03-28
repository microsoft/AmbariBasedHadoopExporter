// <copyright file="Host.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.Cluster
{
    using Newtonsoft.Json;

    public class Host
    {
        [JsonProperty(PropertyName = "Cluster_Name")]
        public string ClusterName { get; set; }

        [JsonProperty(PropertyName = "Host_Name")]
        public string HostName { get; set; }
    }
}
