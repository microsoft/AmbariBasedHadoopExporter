// <copyright file="AlertsSummary.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.Cluster
{
    using Newtonsoft.Json;

    public class AlertsSummary
    {
        [JsonProperty(PropertyName = "CRITICAL")]
        public int Critical { get; set; }

        [JsonProperty(PropertyName = "MAINTENANCE")]
        public int Maintenance { get; set; }

        [JsonProperty(PropertyName = "OK")]
        public int Ok { get; set; }

        [JsonProperty(PropertyName = "UNKNOWN")]
        public int Unknown { get; set; }

        [JsonProperty(PropertyName = "WARNING")]
        public int Warning { get; set; }
    }
}
