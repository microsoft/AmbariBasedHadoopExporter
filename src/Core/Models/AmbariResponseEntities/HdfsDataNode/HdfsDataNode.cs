// <copyright file="HdfsDataNode.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsDataNode
{
    using Newtonsoft.Json;

    public class HdfsDataNode
    {
        [JsonProperty(PropertyName = "Capacity")]
        public double Capacity { get; set; }

        [JsonProperty(PropertyName = "DfsUsed")]
        public double DfsUsed { get; set; }

        [JsonProperty(PropertyName = "NumFailedVolumes")]
        public double NumFailedVolumes { get; set; }
    }
}
