// <copyright file="YarnResourceManagerRpc.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class YarnResourceManagerRpc
    {
        [JsonProperty(PropertyName = "NumOpenConnections")]
        public int NumOpenConnections { get; set; }

        [JsonProperty(PropertyName = "ReceivedBytes")]
        public int ReceivedBytes { get; set; }

        [JsonProperty(PropertyName = "RpcProcessingTime_avg_time")]
        public double RpcProcessingTime_avg_time { get; set; }

        [JsonProperty(PropertyName = "RpcProcessingTime_num_ops")]
        public int RpcProcessingTime_num_ops { get; set; }

        [JsonProperty(PropertyName = "RpcQueueTime_avg_time")]
        public double RpcQueueTime_avg_time { get; set; }

        [JsonProperty(PropertyName = "RpcQueueTime_num_ops")]
        public int RpcQueueTime_num_ops { get; set; }

        [JsonProperty(PropertyName = "SentBytes")]
        public int SentBytes { get; set; }

        [JsonProperty(PropertyName = "callQueueLen")]
        public int CallQueueLen { get; set; }

        [JsonProperty(PropertyName = "rpcAuthenticationFailures")]
        public int RpcAuthenticationFailures { get; set; }

        [JsonProperty(PropertyName = "rpcAuthenticationSuccesses")]
        public int RpcAuthenticationSuccesses { get; set; }

        [JsonProperty(PropertyName = "rpcAuthorizationFailures")]
        public int RpcAuthorizationFailures { get; set; }

        [JsonProperty(PropertyName = "rpcAuthorizationSuccesses")]
        public int RpcAuthorizationSuccesses { get; set; }
    }
}
