// <copyright file="HdfsDataNodeMetrics.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsDataNode
{
    using Core.Models.AmbariResponseEntities.GeneralMetrics;
    using Newtonsoft.Json;

    public class HdfsDataNodeMetrics
    {
        [JsonProperty(PropertyName = "cpu")]
        public Cpu Cpu { get; set; }

        [JsonProperty(PropertyName = "dfs")]
        public HdfsDataNodeDfs HdfsDataNodeDfs { get; set; }

        [JsonProperty(PropertyName = "disk")]
        public Disk Disk { get; set; }

        [JsonProperty(PropertyName = "jvm")]
        public HdfsDataNodeJvm DataNodeJvm { get; set; }

        [JsonProperty(PropertyName = "load")]
        public Load Load { get; set; }

        [JsonProperty(PropertyName = "memory")]
        public Memory Memory { get; set; }

        [JsonProperty(PropertyName = "network")]
        public Network Network { get; set; }

        [JsonProperty(PropertyName = "process")]
        public Process Process { get; set; }

        [JsonProperty(PropertyName = "rpc")]
        public HdfsDataNodeRpc DataNodeRpc { get; set; }
    }
}
