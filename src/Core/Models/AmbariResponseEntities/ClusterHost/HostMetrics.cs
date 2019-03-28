// <copyright file="HostMetrics.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.ClusterHost
{
    using Core.Models.AmbariResponseEntities.GeneralMetrics;
    using Newtonsoft.Json;

    public class HostMetrics
    {
        [JsonProperty("cpu")]
        public HostCpu HostCpu { get; set; }

        [JsonProperty("disk")]
        public Disk Disk { get; set; }

        [JsonProperty("load")]
        public Load Load { get; set; }

        [JsonProperty("memory")]
        public Memory Memory { get; set; }

        [JsonProperty("network")]
        public Network Network { get; set; }

        [JsonProperty("process")]
        public Process Process { get; set; }
    }
}
