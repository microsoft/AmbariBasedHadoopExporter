// <copyright file="HdfsNameNodeDfs.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsNameNode
{
    using Newtonsoft.Json;

    public class HdfsNameNodeDfs
    {
        [JsonProperty("FSNamesystem")]
        public HdfsNameNodeSystem System { get; set; }

        [JsonProperty("namenode")]
        public HdfsNameNode NameNode { get; set; }
    }
}
