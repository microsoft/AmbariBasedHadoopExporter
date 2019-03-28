// <copyright file="HdfsNameNode.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsNameNode
{
    using Newtonsoft.Json;

    public class HdfsNameNode
    {
        [JsonProperty("CorruptFiles")]
        public string CorruptFiles { get; set; }

        [JsonProperty("DeadNodes")]
        public string DeadNodes { get; set; }

        [JsonProperty("DecomNodes")]
        public string DecomNodes { get; set; }

        [JsonProperty("Free")]
        public long Free { get; set; }

        [JsonProperty("LiveNodes")]
        public string LiveNodes { get; set; }

        [JsonProperty("NameDirStatuses")]
        public string NameDirStatuses { get; set; }

        [JsonProperty("NonDfsUsedSpace")]
        public long NonDfsUsedSpace { get; set; }

        [JsonProperty("PercentRemaining")]
        public double PercentRemaining { get; set; }

        [JsonProperty("PercentUsed")]
        public double PercentUsed { get; set; }

        [JsonProperty("Safemode")]
        public string Safemode { get; set; }

        [JsonProperty("Threads")]
        public long Threads { get; set; }

        [JsonProperty("Total")]
        public long Total { get; set; }

        [JsonProperty("TotalBlocks")]
        public long TotalBlocks { get; set; }

        [JsonProperty("TotalFiles")]
        public long TotalFiles { get; set; }

        [JsonProperty("UpgradeFinalized")]
        public bool UpgradeFinalized { get; set; }

        [JsonProperty("Used")]
        public long Used { get; set; }

        [JsonProperty("Version")]
        public string Version { get; set; }
    }
}
