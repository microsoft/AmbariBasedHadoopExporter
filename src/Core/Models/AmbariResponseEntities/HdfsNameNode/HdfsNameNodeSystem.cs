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
        public long BlockCapacity { get; set; }

        [JsonProperty("BlocksTotal")]
        public long BlocksTotal { get; set; }

        [JsonProperty("CapacityRemaining")]
        public long CapacityRemaining { get; set; }

        [JsonProperty("CapacityRemainingGB")]
        public double CapacityRemainingGB { get; set; }

        [JsonProperty("CapacityTotal")]
        public long CapacityTotal { get; set; }

        [JsonProperty("CapacityTotalGB")]
        public double CapacityTotalGB { get; set; }

        [JsonProperty("CapacityUsed")]
        public long CapacityUsed { get; set; }

        [JsonProperty("CapacityUsedGB")]
        public double CapacityUsedGB { get; set; }

        [JsonProperty("CorruptBlocks")]
        public long CorruptBlocks { get; set; }

        [JsonProperty("ExcessBlocks")]
        public long ExcessBlocks { get; set; }

        [JsonProperty("ExpiredHeartbeats")]
        public long ExpiredHeartbeats { get; set; }

        [JsonProperty("FilesTotal")]
        public long FilesTotal { get; set; }

        [JsonProperty("LastCheckpointTime")]
        public long LastCheckpointTime { get; set; }

        [JsonProperty("LastWrittenTransactionId")]
        public long LastWrittenTransactionId { get; set; }

        [JsonProperty("MillisSinceLastLoadedEdits")]
        public long MillisSinceLastLoadedEdits { get; set; }

        [JsonProperty("MissingBlocks")]
        public long MissingBlocks { get; set; }

        [JsonProperty("MissingReplOneBlocks")]
        public long MissingReplOneBlocks { get; set; }

        [JsonProperty("PendingDataNodeMessageCount")]
        public long PendingDataNodeMessageCount { get; set; }

        [JsonProperty("PendingDeletionBlocks")]
        public long PendingDeletionBlocks { get; set; }

        [JsonProperty("PendingReplicationBlocks")]
        public long PendingReplicationBlocks { get; set; }

        [JsonProperty("PostponedMisreplicatedBlocks")]
        public long PostponedMisreplicatedBlocks { get; set; }

        [JsonProperty("ScheduledReplicationBlocks")]
        public long ScheduledReplicationBlocks { get; set; }

        [JsonProperty("Snapshots")]
        public long Snapshots { get; set; }

        [JsonProperty("SnapshottableDirectories")]
        public long SnapshottableDirectories { get; set; }

        [JsonProperty("StaleDataNodes")]
        public long StaleDataNodes { get; set; }

        [JsonProperty("TotalFiles")]
        public long TotalFiles { get; set; }

        [JsonProperty("TotalLoad")]
        public long TotalLoad { get; set; }

        [JsonProperty("TransactionsSinceLastCheckpoint")]
        public long TransactionsSinceLastCheckpoint { get; set; }

        [JsonProperty("TransactionsSinceLastLogRoll")]
        public long TransactionsSinceLastLogRoll { get; set; }

        [JsonProperty("UnderReplicatedBlocks")]
        public long UnderReplicatedBlocks { get; set; }
    }
}
