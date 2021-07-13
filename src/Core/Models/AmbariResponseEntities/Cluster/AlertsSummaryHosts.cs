// <copyright file="AlertsSummaryHosts.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.Cluster
{
    using Newtonsoft.Json;

    public class AlertsSummaryHosts
    {
        [JsonProperty(PropertyName = "CRITICAL")]
        public long Critical { get; set; }

        [JsonProperty(PropertyName = "OK")]
        public long Ok { get; set; }

        [JsonProperty(PropertyName = "UNKNOWN")]
        public long Unknown { get; set; }

        [JsonProperty(PropertyName = "WARNING")]
        public long Warning { get; set; }
    }
}
