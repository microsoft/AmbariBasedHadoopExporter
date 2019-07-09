// <copyright file="HdfsNameNodeExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Extensions;
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Prometheus;

    /// <summary>
    /// Responsible for exporting HDFS NameNode metrics.
    /// </summary>
    internal class HdfsNameNodeExporter : BaseExporter
    {
        private readonly HdfsNameNodeExporterConfiguration _exporterConfiguration;

        public HdfsNameNodeExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<HdfsNameNodeExporterConfiguration> exporterConfiguration,
            ILogger<HdfsNameNodeExporter> logger)
            : base(contentProvider, prometheusUtils, exporterConfiguration.Value, typeof(HdfsNameNodeComponent), logger)
        {
            _exporterConfiguration = exporterConfiguration.Value;
        }

        /// <inheritdoc/>
        protected override async Task ReportMetrics(object component)
        {
            await Task.Factory.StartNew(() =>
            {
                var hdfsNameNodeComponent = (HdfsNameNodeComponent)component;

                // Constructing labels
                var labels = new Dictionary<string, string>()
                {
                    { "ClusterName", hdfsNameNodeComponent.Info.ClusterName },
                    { "Component", hdfsNameNodeComponent.Info.ComponentName },
                };
                labels.TryAdd(_exporterConfiguration.DefaultLabels);

                // General info
                PrometheusUtils.ReportGauge(Collectors, "Info_StartTime", hdfsNameNodeComponent.Info.StartTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_StartedCount", hdfsNameNodeComponent.Info.StartedCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_TotalCount", hdfsNameNodeComponent.Info.TotalCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_UnknownCount", hdfsNameNodeComponent.Info.UnknownCount, labels);

                // Cpu
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Idle", hdfsNameNodeComponent.Metrics.Cpu.Idle, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Nice", hdfsNameNodeComponent.Metrics.Cpu.Nice, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_System", hdfsNameNodeComponent.Metrics.Cpu.System, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_User", hdfsNameNodeComponent.Metrics.Cpu.User, labels);
                PrometheusUtils.ReportGauge(Collectors, "Cpu_Wio", hdfsNameNodeComponent.Metrics.Cpu.Wio, labels);

                // HdfsDataNodeDfs - namenode
                PrometheusUtils.ReportGauge(
                    Collectors,
                    "CorruptFiles",
                    hdfsNameNodeComponent.Metrics.Dfs.NameNode.CorruptFiles.TrimStart('[').TrimEnd(']').Split(",", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                PrometheusUtils.ReportGauge(
                    Collectors,
                    "DeadNodes",
                    hdfsNameNodeComponent.Metrics.Dfs.NameNode.DeadNodes.TrimStart('{').TrimEnd('}').Split("},", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                PrometheusUtils.ReportGauge(
                    Collectors,
                    "DecomNodes",
                    hdfsNameNodeComponent.Metrics.Dfs.NameNode.DecomNodes.TrimStart('{').TrimEnd('}').Split("},", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                PrometheusUtils.ReportGauge(
                    Collectors,
                    "LiveNodes",
                    hdfsNameNodeComponent.Metrics.Dfs.NameNode.LiveNodes.TrimStart('[').TrimEnd(']').Split("},", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                PrometheusUtils.ReportGauge(Collectors, "Free", hdfsNameNodeComponent.Metrics.Dfs.NameNode.Free, labels);
                PrometheusUtils.ReportGauge(Collectors, "NonDfsUsedSpace", hdfsNameNodeComponent.Metrics.Dfs.NameNode.NonDfsUsedSpace, labels);
                PrometheusUtils.ReportGauge(Collectors, "PercentRemaining", hdfsNameNodeComponent.Metrics.Dfs.NameNode.PercentRemaining, labels);
                PrometheusUtils.ReportGauge(Collectors, "PercentUsed", hdfsNameNodeComponent.Metrics.Dfs.NameNode.PercentUsed, labels);
                PrometheusUtils.ReportGauge(Collectors, "Threads", hdfsNameNodeComponent.Metrics.Dfs.NameNode.Threads, labels);
                PrometheusUtils.ReportGauge(Collectors, "Total", hdfsNameNodeComponent.Metrics.Dfs.NameNode.Total, labels);
                PrometheusUtils.ReportGauge(Collectors, "TotalBlocks", hdfsNameNodeComponent.Metrics.Dfs.NameNode.TotalBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "TotalFiles", hdfsNameNodeComponent.Metrics.Dfs.NameNode.TotalFiles, labels);
                PrometheusUtils.ReportGauge(Collectors, "Used", hdfsNameNodeComponent.Metrics.Dfs.NameNode.Used, labels);

                // HdfsDataNodeDfs - system
                PrometheusUtils.ReportGauge(Collectors, "BlockCapacity", hdfsNameNodeComponent.Metrics.Dfs.System.BlockCapacity, labels);
                PrometheusUtils.ReportGauge(Collectors, "BlocksTotal", hdfsNameNodeComponent.Metrics.Dfs.System.BlocksTotal, labels);
                PrometheusUtils.ReportGauge(Collectors, "CapacityRemaining", hdfsNameNodeComponent.Metrics.Dfs.System.CapacityRemaining, labels);
                PrometheusUtils.ReportGauge(Collectors, "CapacityRemainingGB", hdfsNameNodeComponent.Metrics.Dfs.System.CapacityRemainingGB, labels);
                PrometheusUtils.ReportGauge(Collectors, "CapacityTotal", hdfsNameNodeComponent.Metrics.Dfs.System.CapacityTotal, labels);
                PrometheusUtils.ReportGauge(Collectors, "CapacityTotalGB", hdfsNameNodeComponent.Metrics.Dfs.System.CapacityTotalGB, labels);
                PrometheusUtils.ReportGauge(Collectors, "CapacityUsed", hdfsNameNodeComponent.Metrics.Dfs.System.CapacityUsed, labels);
                PrometheusUtils.ReportGauge(Collectors, "CapacityUsedGB", hdfsNameNodeComponent.Metrics.Dfs.System.CapacityUsedGB, labels);
                PrometheusUtils.ReportGauge(Collectors, "CorruptBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.CorruptBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "ExcessBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.ExcessBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "ExpiredHeartbeats", hdfsNameNodeComponent.Metrics.Dfs.System.ExpiredHeartbeats, labels);
                PrometheusUtils.ReportGauge(Collectors, "FilesTotal", hdfsNameNodeComponent.Metrics.Dfs.System.FilesTotal, labels);
                PrometheusUtils.ReportGauge(Collectors, "LastCheckpointTime", hdfsNameNodeComponent.Metrics.Dfs.System.LastCheckpointTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "LastWrittenTransactionId", hdfsNameNodeComponent.Metrics.Dfs.System.LastWrittenTransactionId, labels);
                PrometheusUtils.ReportGauge(Collectors, "MillisSinceLastLoadedEdits", hdfsNameNodeComponent.Metrics.Dfs.System.MillisSinceLastLoadedEdits, labels);
                PrometheusUtils.ReportGauge(Collectors, "MissingBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.MissingBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "MissingReplOneBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.MissingReplOneBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "PendingDataNodeMessageCount", hdfsNameNodeComponent.Metrics.Dfs.System.PendingDataNodeMessageCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "PendingDeletionBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.PendingDeletionBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "PendingReplicationBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.PendingReplicationBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "PostponedMisreplicatedBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.PostponedMisreplicatedBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "ScheduledReplicationBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.ScheduledReplicationBlocks, labels);
                PrometheusUtils.ReportGauge(Collectors, "Snapshots", hdfsNameNodeComponent.Metrics.Dfs.System.Snapshots, labels);
                PrometheusUtils.ReportGauge(Collectors, "SnapshottableDirectories", hdfsNameNodeComponent.Metrics.Dfs.System.SnapshottableDirectories, labels);
                PrometheusUtils.ReportGauge(Collectors, "StaleDataNodes", hdfsNameNodeComponent.Metrics.Dfs.System.StaleDataNodes, labels);
                PrometheusUtils.ReportGauge(Collectors, "TotalFiles", hdfsNameNodeComponent.Metrics.Dfs.System.TotalFiles, labels);
                PrometheusUtils.ReportGauge(Collectors, "TotalLoad", hdfsNameNodeComponent.Metrics.Dfs.System.TotalLoad, labels);
                PrometheusUtils.ReportGauge(Collectors, "TransactionsSinceLastCheckpoint", hdfsNameNodeComponent.Metrics.Dfs.System.TransactionsSinceLastCheckpoint, labels);
                PrometheusUtils.ReportGauge(Collectors, "TransactionsSinceLastLogRoll", hdfsNameNodeComponent.Metrics.Dfs.System.TransactionsSinceLastLogRoll, labels);
                PrometheusUtils.ReportGauge(Collectors, "UnderReplicatedBlocks", hdfsNameNodeComponent.Metrics.Dfs.System.UnderReplicatedBlocks, labels);

                // Disk
                PrometheusUtils.ReportGauge(Collectors, "Disk_Free", hdfsNameNodeComponent.Metrics.Disk.Free, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadBytes", hdfsNameNodeComponent.Metrics.Disk.ReadBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadCount", hdfsNameNodeComponent.Metrics.Disk.ReadCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_ReadTime", hdfsNameNodeComponent.Metrics.Disk.ReadTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_Total", hdfsNameNodeComponent.Metrics.Disk.Total, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteBytes", hdfsNameNodeComponent.Metrics.Disk.WriteBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteCount", hdfsNameNodeComponent.Metrics.Disk.WriteCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Disk_WriteTime", hdfsNameNodeComponent.Metrics.Disk.WriteTime, labels);

                // HdfsNameNodeJvm
                PrometheusUtils.ReportGauge(Collectors, "Jvm_GcCountConcurrentMarkSweep", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.GcCountConcurrentMarkSweep, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_GcTimeMillisConcurrentMarkSweep", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.GcTimeMillisConcurrentMarkSweep, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_gcCount", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.GcCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_gcTimeMillis", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.GcTimeMillis, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_logError", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.LogError, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_logFatal", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.LogFatal, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_logInfo", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.LogInfo, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_logWarn", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.LogWarn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_memHeapCommittedM", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.MemHeapCommittedM, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_memHeapUsedM", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.MemHeapUsedM, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_memMaxM", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.MemMaxM, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_memNonHeapCommittedM", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.MemNonHeapCommittedM, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_memNonHeapUsedM", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.MemNonHeapUsedM, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_threadsBlocked", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.ThreadsBlocked, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_threadsNew", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.ThreadsNew, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_threadsRunnable", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.ThreadsRunnable, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_threadsTerminated", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.ThreadsTerminated, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_threadsTimedWaiting", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.ThreadsTimedWaiting, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_threadsWaiting", hdfsNameNodeComponent.Metrics.HdfsNameNodeJvm.ThreadsWaiting, labels);

                // Memory
                PrometheusUtils.ReportGauge(Collectors, "Memory_CachedKb", hdfsNameNodeComponent.Metrics.Memory.CachedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_FreeKb", hdfsNameNodeComponent.Metrics.Memory.FreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_SharedKb", hdfsNameNodeComponent.Metrics.Memory.SharedKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_SwapFreeKb", hdfsNameNodeComponent.Metrics.Memory.SwapFreeKb, labels);
                PrometheusUtils.ReportGauge(Collectors, "Memory_TotalKb", hdfsNameNodeComponent.Metrics.Memory.TotalKb, labels);

                // Network
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesIn", hdfsNameNodeComponent.Metrics.Network.BytesIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_BytesOut", hdfsNameNodeComponent.Metrics.Network.BytesOut, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsIn", hdfsNameNodeComponent.Metrics.Network.PktsIn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Network_PktsOut", hdfsNameNodeComponent.Metrics.Network.PktsOut, labels);

                // Process
                PrometheusUtils.ReportGauge(Collectors, "Process_Run", hdfsNameNodeComponent.Metrics.Process.Run, labels);
                PrometheusUtils.ReportGauge(Collectors, "Process_Total", hdfsNameNodeComponent.Metrics.Process.Total, labels);

                // YarnResourceManagerRpc
                PrometheusUtils.ReportGauge(Collectors, "Rpc_NumOpenConnections", hdfsNameNodeComponent.Metrics.Rpc.Client.NumOpenConnections, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_ReceivedBytes", hdfsNameNodeComponent.Metrics.Rpc.Client.ReceivedBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_ProcessingTimeAvgTime", hdfsNameNodeComponent.Metrics.Rpc.Client.ProcessingTimeAvgTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_QueueTimeAvgTime", hdfsNameNodeComponent.Metrics.Rpc.Client.QueueTimeAvgTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_SentBytes", hdfsNameNodeComponent.Metrics.Rpc.Client.SentBytes, labels);
            });
        }
    }
}
