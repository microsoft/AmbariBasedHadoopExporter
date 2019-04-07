// <copyright file="HdfsNameNodeComponentTest.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Models
{
    using System;
    using System.IO;
    using Core.Models.Components;
    using FluentAssertions;
    using Newtonsoft.Json;
    using Xunit;

    public class HdfsNameNodeComponentTest
    {
        [Fact]
        public void Should_Load_Class_From_Json_Successfully()
        {
            var content = File.ReadAllText("Jsons/HdfsNameNodeResponse.json");
            var component = JsonConvert.DeserializeObject<HdfsNameNodeComponent>(content);

            component.Should().NotBeNull();

            // Info
            component.Info.ClusterName.Should().Be("sparkpoc");
            component.Info.ComponentName.Should().Be("NAMENODE");
            component.Info.StartTime.Should().Be(1547394299895);
            component.Info.StartedCount.Should().Be(2);
            component.Info.TotalCount.Should().Be(2);
            component.Info.UnknownCount.Should().Be(0);

            // Cpu
            component.Metrics.Cpu.Idle.Should().Be(84.30476190476192);
            component.Metrics.Cpu.Nice.Should().Be(0.0);
            component.Metrics.Cpu.System.Should().Be(4.671428571428572);
            component.Metrics.Cpu.User.Should().Be(7.747619047619048);
            component.Metrics.Cpu.Wio.Should().Be(3.1999999999999997);

            // HdfsDataNodeDfs - namenode
            component.Metrics.Dfs.NameNode.CorruptFiles.TrimStart('[').TrimEnd(']')
                .Split(",", StringSplitOptions.RemoveEmptyEntries).Length.Should().Be(0);
            component.Metrics.Dfs.NameNode.DeadNodes.TrimStart('{').TrimEnd('}')
                .Split("},", StringSplitOptions.RemoveEmptyEntries).Length.Should().Be(0);
            component.Metrics.Dfs.NameNode.DecomNodes.TrimStart('{').TrimEnd('}')
                .Split("},", StringSplitOptions.RemoveEmptyEntries).Length.Should().Be(0);
            component.Metrics.Dfs.NameNode.Free.Should().Be(364431560704);
            component.Metrics.Dfs.NameNode.LiveNodes.TrimStart('{').TrimEnd('}')
                .Split("},", StringSplitOptions.RemoveEmptyEntries).Length.Should().Be(2);
            component.Metrics.Dfs.NameNode.NonDfsUsedSpace.Should().Be(34391437312);
            component.Metrics.Dfs.NameNode.PercentRemaining.Should().Be(86.69969);
            component.Metrics.Dfs.NameNode.PercentUsed.Should().Be(0.0015747182);
            component.Metrics.Dfs.NameNode.Threads.Should().Be(139);
            component.Metrics.Dfs.NameNode.Total.Should().Be(420337795072);
            component.Metrics.Dfs.NameNode.TotalBlocks.Should().Be(2);
            component.Metrics.Dfs.NameNode.TotalFiles.Should().Be(25);
            component.Metrics.Dfs.NameNode.Used.Should().Be(6619136);

            // HdfsDataNodeDfs - system
            component.Metrics.Dfs.System.BlockCapacity.Should().Be(2097152);
            component.Metrics.Dfs.System.BlocksTotal.Should().Be(2);
            component.Metrics.Dfs.System.CapacityRemaining.Should().Be(364431560704);
            component.Metrics.Dfs.System.CapacityRemainingGB.Should().Be(339.0);
            component.Metrics.Dfs.System.CapacityTotal.Should().Be(420337795072);
            component.Metrics.Dfs.System.CapacityTotalGB.Should().Be(391.0);
            component.Metrics.Dfs.System.CapacityUsed.Should().Be(6619136);
            component.Metrics.Dfs.System.CapacityUsedGB.Should().Be(0.0);
            component.Metrics.Dfs.System.CorruptBlocks.Should().Be(0);
            component.Metrics.Dfs.System.ExcessBlocks.Should().Be(0);
            component.Metrics.Dfs.System.ExpiredHeartbeats.Should().Be(0);
            component.Metrics.Dfs.System.FilesTotal.Should().Be(25);
            component.Metrics.Dfs.System.LastCheckpointTime.Should().Be(1552551695724);
            component.Metrics.Dfs.System.LastWrittenTransactionId.Should().Be(2455329);
            component.Metrics.Dfs.System.MillisSinceLastLoadedEdits.Should().Be(0);
            component.Metrics.Dfs.System.MissingBlocks.Should().Be(0);
            component.Metrics.Dfs.System.MissingReplOneBlocks.Should().Be(0);
            component.Metrics.Dfs.System.PendingDataNodeMessageCount.Should().Be(0);
            component.Metrics.Dfs.System.PendingDeletionBlocks.Should().Be(0);
            component.Metrics.Dfs.System.PendingReplicationBlocks.Should().Be(0);
            component.Metrics.Dfs.System.PostponedMisreplicatedBlocks.Should().Be(0);
            component.Metrics.Dfs.System.ScheduledReplicationBlocks.Should().Be(0);
            component.Metrics.Dfs.System.Snapshots.Should().Be(0);
            component.Metrics.Dfs.System.SnapshottableDirectories.Should().Be(0);
            component.Metrics.Dfs.System.StaleDataNodes.Should().Be(0);
            component.Metrics.Dfs.System.TotalFiles.Should().Be(25);
            component.Metrics.Dfs.System.TotalLoad.Should().Be(4);
            component.Metrics.Dfs.System.TransactionsSinceLastCheckpoint.Should().Be(967);
            component.Metrics.Dfs.System.TransactionsSinceLastLogRoll.Should().Be(1);
            component.Metrics.Dfs.System.UnderReplicatedBlocks.Should().Be(2);

            // Disk
            component.Metrics.Disk.Free.Should().Be(1088.9771428571428);
            component.Metrics.Disk.Total.Should().Be(1138.6699999999998);
            component.Metrics.Disk.ReadBytes.Should().Be(2087548671390.4763);
            component.Metrics.Disk.ReadCount.Should().Be(78317992.71428572);
            component.Metrics.Disk.ReadTime.Should().Be(94448621.14285715);
            component.Metrics.Disk.WriteBytes.Should().Be(2581627998012.952);
            component.Metrics.Disk.WriteCount.Should().Be(190327754.09523812);
            component.Metrics.Disk.WriteTime.Should().Be(545197061.7142856);

            // Jvm
            component.Metrics.HdfsNameNodeJvm.GcCountConcurrentMarkSweep.Should().Be(2);
            component.Metrics.HdfsNameNodeJvm.GcTimeMillisConcurrentMarkSweep.Should().Be(112);
            component.Metrics.HdfsNameNodeJvm.GcCount.Should().Be(5847);
            component.Metrics.HdfsNameNodeJvm.GcTimeMillis.Should().Be(70862);
            component.Metrics.HdfsNameNodeJvm.LogError.Should().Be(4);
            component.Metrics.HdfsNameNodeJvm.LogFatal.Should().Be(0);
            component.Metrics.HdfsNameNodeJvm.LogInfo.Should().Be(3236642);
            component.Metrics.HdfsNameNodeJvm.LogWarn.Should().Be(6146);
            component.Metrics.HdfsNameNodeJvm.MemHeapCommittedM.Should().Be(1004.0);
            component.Metrics.HdfsNameNodeJvm.MemHeapUsedM.Should().Be(547.4121);
            component.Metrics.HdfsNameNodeJvm.MemMaxM.Should().Be(1004.0);
            component.Metrics.HdfsNameNodeJvm.MemNonHeapCommittedM.Should().Be(93.015625);
            component.Metrics.HdfsNameNodeJvm.MemNonHeapUsedM.Should().Be(90.40014);
            component.Metrics.HdfsNameNodeJvm.ThreadsBlocked.Should().Be(0);
            component.Metrics.HdfsNameNodeJvm.ThreadsNew.Should().Be(0);
            component.Metrics.HdfsNameNodeJvm.ThreadsRunnable.Should().Be(9);
            component.Metrics.HdfsNameNodeJvm.ThreadsTerminated.Should().Be(0);
            component.Metrics.HdfsNameNodeJvm.ThreadsTimedWaiting.Should().Be(121);
            component.Metrics.HdfsNameNodeJvm.ThreadsWaiting.Should().Be(9);

            // Memory
            component.Metrics.Memory.CachedKb.Should().Be(4927475.809523809);
            component.Metrics.Memory.FreeKb.Should().Be(5864473.523809524);
            component.Metrics.Memory.SharedKb.Should().Be(0.0);
            component.Metrics.Memory.SwapFreeKb.Should().Be(6408751.428571428);
            component.Metrics.Memory.TotalKb.Should().Be(9717450.285714285);

            // Network
            component.Metrics.Network.BytesIn.Should().Be(86756.0661848409);
            component.Metrics.Network.BytesOut.Should().Be(72762.21994039192);
            component.Metrics.Network.PktsIn.Should().Be(92.67241878226822);
            component.Metrics.Network.PktsOut.Should().Be(59.68176967665266);

            // Process
            component.Metrics.Process.Run.Should().Be(0.14285714285714285);
            component.Metrics.Process.Total.Should().Be(164.38095238095235);

            // YarnResourceManagerRpc
            component.Metrics.Rpc.Client.NumOpenConnections.Should().Be(5);
            component.Metrics.Rpc.Client.ReceivedBytes.Should().Be(3282883140);
            component.Metrics.Rpc.Client.ProcessingTimeAvgTime.Should().Be(0.0);
            component.Metrics.Rpc.Client.QueueTimeAvgTime.Should().Be(0.0);
            component.Metrics.Rpc.Client.SentBytes.Should().Be(622947220);
        }
    }
}
