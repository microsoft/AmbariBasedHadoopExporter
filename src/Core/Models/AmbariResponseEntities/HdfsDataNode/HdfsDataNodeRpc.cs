// <copyright file="HdfsDataNodeRpc.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsDataNode
{
    using Newtonsoft.Json;

    public class HdfsDataNodeRpc
    {
        [JsonProperty(PropertyName = "NumOpenConnections")]
        public double NumOpenConnections { get; set; }

        [JsonProperty(PropertyName = "RpcProcessingTime_avg_time")]
        public double ProcessingTimeAvgTime { get; set; }

        [JsonProperty(PropertyName = "RpcQueueTime_avg_time")]
        public double QueueTimeAvgTime { get; set; }
    }
}
