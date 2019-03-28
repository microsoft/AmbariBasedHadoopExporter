// <copyright file="Memory.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.GeneralMetrics
{
    using Newtonsoft.Json;

    public class Memory
    {
        [JsonProperty(PropertyName = "mem_cached")]
        public double CachedKb { get; set; }

        [JsonProperty(PropertyName = "mem_free")]
        public double FreeKb { get; set; }

        [JsonProperty(PropertyName = "mem_shared")]
        public double SharedKb { get; set; }

        [JsonProperty(PropertyName = "mem_total")]
        public double TotalKb { get; set; }

        [JsonProperty(PropertyName = "swap_free")]
        public double SwapFreeKb { get; set; }
    }
}
