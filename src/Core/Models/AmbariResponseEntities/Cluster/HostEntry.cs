// <copyright file="HostEntry.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.Cluster
{
    using Newtonsoft.Json;

    public class HostEntry
    {
        [JsonProperty(PropertyName = "href")]
        public string Href { get; set; }

        [JsonProperty(PropertyName = "Hosts")]
        public Host Host { get; set; }
    }
}
