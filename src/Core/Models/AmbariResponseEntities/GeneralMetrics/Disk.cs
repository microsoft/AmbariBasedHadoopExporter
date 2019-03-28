// <copyright file="Disk.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.GeneralMetrics
{
    using Newtonsoft.Json;

    public class Disk
    {
        [JsonProperty(PropertyName = "disk_free")]
        public double Free { get; set; }

        [JsonProperty(PropertyName = "disk_total")]
        public double Total { get; set; }

        [JsonProperty(PropertyName = "read_bytes")]
        public double ReadBytes { get; set; }

        [JsonProperty(PropertyName = "read_count")]
        public double ReadCount { get; set; }

        [JsonProperty(PropertyName = "read_time")]
        public double ReadTime { get; set; }

        [JsonProperty(PropertyName = "write_bytes")]
        public double WriteBytes { get; set; }

        [JsonProperty(PropertyName = "write_count")]
        public double WriteCount { get; set; }

        [JsonProperty(PropertyName = "write_time")]
        public double WriteTime { get; set; }
    }
}
