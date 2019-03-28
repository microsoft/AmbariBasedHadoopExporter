// <copyright file="HdfsNameNodeMetrics.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsNameNode
{
    using Core.Models.AmbariResponseEntities.GeneralMetrics;
    using Newtonsoft.Json;

    public class HdfsNameNodeMetrics
    {
        [JsonProperty("cpu")]
        public Cpu Cpu { get; set; }

        [JsonProperty("dfs")]
        public HdfsNameNodeDfs Dfs { get; set; }

        [JsonProperty("disk")]
        public Disk Disk { get; set; }

        [JsonProperty("jvm")]
        public HdfsNameNodeJvm HdfsNameNodeJvm { get; set; }

        [JsonProperty("load")]
        public Load Load { get; set; }

        [JsonProperty("memory")]
        public Memory Memory { get; set; }

        [JsonProperty("network")]
        public Network Network { get; set; }

        [JsonProperty("process")]
        public Process Process { get; set; }

        [JsonProperty("rpc")]
        public HdfsNameNodeRpc Rpc { get; set; }
    }
}
