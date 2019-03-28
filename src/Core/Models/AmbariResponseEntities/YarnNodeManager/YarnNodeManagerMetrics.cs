// <copyright file="YarnNodeManagerMetrics.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnNodeManager
{
    using Core.Models.AmbariResponseEntities.GeneralMetrics;
    using Newtonsoft.Json;

    public class YarnNodeManagerMetrics
    {
        [JsonProperty("cpu")]
        public Cpu Cpu { get; set; }

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

        [JsonProperty(PropertyName = "yarn")]
        public YarnNodeManagerBase YarnBase { get; set; }
    }
}
