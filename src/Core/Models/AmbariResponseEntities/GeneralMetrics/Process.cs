// <copyright file="Process.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.GeneralMetrics
{
    using Newtonsoft.Json;

    public class Process
    {
        [JsonProperty(PropertyName = "proc_run")]
        public double Run { get; set; }

        [JsonProperty(PropertyName = "proc_total")]
        public double Total { get; set; }
    }
}
