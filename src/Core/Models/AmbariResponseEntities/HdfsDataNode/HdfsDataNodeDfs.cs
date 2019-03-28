// <copyright file="HdfsDataNodeDfs.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsDataNode
{
    using Newtonsoft.Json;

    public class HdfsDataNodeDfs
    {
        [JsonProperty(PropertyName = "datanode")]
        public HdfsDataNode HdfsDataNode { get; set; }
    }
}
