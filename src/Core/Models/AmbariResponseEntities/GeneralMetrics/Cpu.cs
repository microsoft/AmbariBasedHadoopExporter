// <copyright file="Cpu.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.GeneralMetrics
{
    using Newtonsoft.Json;

    public class Cpu
    {
        [JsonProperty(PropertyName = "cpu_idle")]
        public double Idle { get; set; }

        [JsonProperty(PropertyName = "cpu_nice")]
        public double Nice { get; set; }

        [JsonProperty(PropertyName = "cpu_system")]
        public double System { get; set; }

        [JsonProperty(PropertyName = "cpu_user")]
        public double User { get; set; }

        [JsonProperty(PropertyName = "cpu_wio")]
        public double Wio { get; set; }
    }
}
