// <copyright file="HostCpu.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.ClusterHost
{
    using Core.Models.AmbariResponseEntities.GeneralMetrics;
    using Newtonsoft.Json;

    public class HostCpu : Cpu
    {
        [JsonProperty(PropertyName = "cpu_num")]
        public double Num { get; set; }
    }
}
