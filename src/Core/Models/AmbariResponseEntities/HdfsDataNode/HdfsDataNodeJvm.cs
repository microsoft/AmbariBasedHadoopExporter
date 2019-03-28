// <copyright file="HdfsDataNodeJvm.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsDataNode
{
    using Newtonsoft.Json;

    public class HdfsDataNodeJvm
    {
        [JsonProperty(PropertyName = "gcCount")]
        public double GcCount { get; set; }

        [JsonProperty(PropertyName = "memHeapCommittedM")]
        public double MemHeapCommittedM { get; set; }

        [JsonProperty(PropertyName = "memHeapUsedM")]
        public double MemHeapUsedM { get; set; }
    }
}
