// <copyright file="Network.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.GeneralMetrics
{
    using Newtonsoft.Json;

    public class Network
    {
        [JsonProperty(PropertyName = "bytes_in")]
        public double BytesIn { get; set; }

        [JsonProperty(PropertyName = "bytes_out")]
        public double BytesOut { get; set; }

        [JsonProperty(PropertyName = "pkts_in")]
        public double PktsIn { get; set; }

        [JsonProperty(PropertyName = "pkts_out")]
        public double PktsOut { get; set; }
    }
}
