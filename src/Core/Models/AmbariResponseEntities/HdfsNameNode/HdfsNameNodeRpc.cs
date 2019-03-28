// <copyright file="HdfsNameNodeRpc.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsNameNode
{
    using Newtonsoft.Json;

    public class HdfsNameNodeRpc
    {
        [JsonProperty("client")]
        public HdfsNameNodeRpcClient Client { get; set; }
    }
}
