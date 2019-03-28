// <copyright file="HdfsNameNodeSystem.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.HdfsNameNode
{
    using Newtonsoft.Json;

    public class HdfsNameNodeSystem
    {
        [JsonProperty("BlockCapacity")]
        public int BlockCapacity { get; set; }

        [JsonProperty("BlocksTotal")]
        public int BlocksTotal { get; set; }

        [JsonProperty("CapacityRemaining")]
        public long CapacityRemaining { get; set; }

        [JsonProperty("CapacityRemainingGB")]
        public double CapacityRemainingGB { get; set; }

        [JsonProperty("CapacityTotal")]
        public long CapacityTotal { get; set; }

        [JsonProperty("CapacityTotalGB")]
        public double CapacityTotalGB { get; set; }

        [JsonProperty("CapacityUsed")]
        public int CapacityUsed { get; set; }

        [JsonProperty("CapacityUsedGB")]
        public double CapacityUsedGB { get; set; }

        [JsonProperty("CorruptBlocks")]
        public int CorruptBlocks { get; set; }

        [JsonProperty("ExcessBlocks")]
        public int ExcessBlocks { get; set; }

        [JsonProperty("ExpiredHeartbeats")]
        public int ExpiredHeartbeats { get; set; }

        [JsonProperty("FilesTotal")]
        public int FilesTotal { get; set; }

        [JsonProperty("LastCheckpointTime")]
        public long LastCheckpointTime { get; set; }

        [JsonProperty("LastWrittenTransactionId")]
        public int LastWrittenTransactionId { get; set; }

        [JsonProperty("MillisSinceLastLoadedEdits")]
        public int MillisSinceLastLoadedEdits { get; set; }

        [JsonProperty("MissingBlocks")]
        public int MissingBlocks { get; set; }

        [JsonProperty("MissingReplOneBlocks")]
        public int MissingReplOneBlocks { get; set; }

        [JsonProperty("PendingDataNodeMessageCount")]
        public int PendingDataNodeMessageCount { get; set; }

        [JsonProperty("PendingDeletionBlocks")]
        public int PendingDeletionBlocks { get; set; }

        [JsonProperty("PendingReplicationBlocks")]
        public int PendingReplicationBlocks { get; set; }

        [JsonProperty("PostponedMisreplicatedBlocks")]
        public int PostponedMisreplicatedBlocks { get; set; }

        [JsonProperty("ScheduledReplicationBlocks")]
        public int ScheduledReplicationBlocks { get; set; }

        [JsonProperty("Snapshots")]
        public int Snapshots { get; set; }

        [JsonProperty("SnapshottableDirectories")]
        public int SnapshottableDirectories { get; set; }

        [JsonProperty("StaleDataNodes")]
        public int StaleDataNodes { get; set; }

        [JsonProperty("TotalFiles")]
        public int TotalFiles { get; set; }

        [JsonProperty("TotalLoad")]
        public int TotalLoad { get; set; }

        [JsonProperty("TransactionsSinceLastCheckpoint")]
        public int TransactionsSinceLastCheckpoint { get; set; }

        [JsonProperty("TransactionsSinceLastLogRoll")]
        public int TransactionsSinceLastLogRoll { get; set; }

        [JsonProperty("UnderReplicatedBlocks")]
        public int UnderReplicatedBlocks { get; set; }
    }
}
