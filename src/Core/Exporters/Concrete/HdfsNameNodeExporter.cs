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
    using Core.Exporters.Abstract;
    using Core.Extensions;
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Prometheus;

    /// <summary>
    /// Responsible for exporting HDFS NameNode metrics.
    /// </summary>
    internal class HdfsNameNodeExporter : IExporter
    {
        private readonly IContentProvider _contentProvider;
        private readonly IPrometheusUtils _prometheusUtils;
        private readonly HdfsNameNodeExporterConfiguration _exporterConfiguration;
        private readonly ILogger<HdfsNameNodeExporter> _logger;

        public HdfsNameNodeExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<HdfsNameNodeExporterConfiguration> exporterConfiguration,
            ILogger<HdfsNameNodeExporter> logger)
        {
            _contentProvider = contentProvider;
            _prometheusUtils = prometheusUtils;
            _exporterConfiguration = exporterConfiguration.Value;
            _logger = logger;
            Collectors = new ConcurrentDictionary<string, Collector>();
        }

        /// <inheritdoc/>
        public ConcurrentDictionary<string, Collector> Collectors { get; private set; }

        /// <inheritdoc/>
        public async Task ExportMetricsAsync()
        {
            try
            {
                var content = await _contentProvider.GetResponseContentAsync(_exporterConfiguration.UriEndpoint);
                var component = JsonConvert.DeserializeObject<HdfsNameNodeComponent>(content);

                // Constructing labels
                var labels = new Dictionary<string, string>()
                    {
                        { "ClusterName", component.Info.ClusterName },
                        { "Component", component.Info.ComponentName },
                    };
                labels.TryAdd(_exporterConfiguration.DefaultLabels);

                // General info
                _prometheusUtils.ReportGauge(Collectors, "Info_StartTime", component.Info.StartTime, labels);
                _prometheusUtils.ReportGauge(Collectors, "Info_StartedCount", component.Info.StartedCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Info_TotalCount", component.Info.TotalCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Info_UnknownCount", component.Info.UnknownCount, labels);

                // Cpu
                _prometheusUtils.ReportGauge(Collectors, "Cpu_Idle", component.Metrics.Cpu.Idle, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_Nice", component.Metrics.Cpu.Nice, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_System", component.Metrics.Cpu.System, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_User", component.Metrics.Cpu.User, labels);
                _prometheusUtils.ReportGauge(Collectors, "Cpu_Wio", component.Metrics.Cpu.Wio, labels);

                // HdfsDataNodeDfs - namenode
                _prometheusUtils.ReportGauge(
                    Collectors,
                    "CorruptFiles",
                    component.Metrics.Dfs.NameNode.CorruptFiles.TrimStart('[').TrimEnd(']').Split(",", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                _prometheusUtils.ReportGauge(
                    Collectors,
                    "DeadNodes",
                    component.Metrics.Dfs.NameNode.DeadNodes.TrimStart('{').TrimEnd('}').Split("},", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                _prometheusUtils.ReportGauge(
                    Collectors,
                    "DecomNodes",
                    component.Metrics.Dfs.NameNode.DecomNodes.TrimStart('{').TrimEnd('}').Split("},", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                _prometheusUtils.ReportGauge(
                    Collectors,
                    "LiveNodes",
                    component.Metrics.Dfs.NameNode.LiveNodes.TrimStart('[').TrimEnd(']').Split("},", StringSplitOptions.RemoveEmptyEntries).Length,
                    labels);
                _prometheusUtils.ReportGauge(Collectors, "Free", component.Metrics.Dfs.NameNode.Free, labels);
                _prometheusUtils.ReportGauge(Collectors, "NonDfsUsedSpace", component.Metrics.Dfs.NameNode.NonDfsUsedSpace, labels);
                _prometheusUtils.ReportGauge(Collectors, "PercentRemaining", component.Metrics.Dfs.NameNode.PercentRemaining, labels);
                _prometheusUtils.ReportGauge(Collectors, "PercentUsed", component.Metrics.Dfs.NameNode.PercentUsed, labels);
                _prometheusUtils.ReportGauge(Collectors, "Threads", component.Metrics.Dfs.NameNode.Threads, labels);
                _prometheusUtils.ReportGauge(Collectors, "Total", component.Metrics.Dfs.NameNode.Total, labels);
                _prometheusUtils.ReportGauge(Collectors, "TotalBlocks", component.Metrics.Dfs.NameNode.TotalBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "TotalFiles", component.Metrics.Dfs.NameNode.TotalFiles, labels);
                _prometheusUtils.ReportGauge(Collectors, "Used", component.Metrics.Dfs.NameNode.Used, labels);

                // HdfsDataNodeDfs - system
                _prometheusUtils.ReportGauge(Collectors, "BlockCapacity", component.Metrics.Dfs.System.BlockCapacity, labels);
                _prometheusUtils.ReportGauge(Collectors, "BlocksTotal", component.Metrics.Dfs.System.BlocksTotal, labels);
                _prometheusUtils.ReportGauge(Collectors, "CapacityRemaining", component.Metrics.Dfs.System.CapacityRemaining, labels);
                _prometheusUtils.ReportGauge(Collectors, "CapacityRemainingGB", component.Metrics.Dfs.System.CapacityRemainingGB, labels);
                _prometheusUtils.ReportGauge(Collectors, "CapacityTotal", component.Metrics.Dfs.System.CapacityTotal, labels);
                _prometheusUtils.ReportGauge(Collectors, "CapacityTotalGB", component.Metrics.Dfs.System.CapacityTotalGB, labels);
                _prometheusUtils.ReportGauge(Collectors, "CapacityUsed", component.Metrics.Dfs.System.CapacityUsed, labels);
                _prometheusUtils.ReportGauge(Collectors, "CapacityUsedGB", component.Metrics.Dfs.System.CapacityUsedGB, labels);
                _prometheusUtils.ReportGauge(Collectors, "CorruptBlocks", component.Metrics.Dfs.System.CorruptBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "ExcessBlocks", component.Metrics.Dfs.System.ExcessBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "ExpiredHeartbeats", component.Metrics.Dfs.System.ExpiredHeartbeats, labels);
                _prometheusUtils.ReportGauge(Collectors, "FilesTotal", component.Metrics.Dfs.System.FilesTotal, labels);
                _prometheusUtils.ReportGauge(Collectors, "LastCheckpointTime", component.Metrics.Dfs.System.LastCheckpointTime, labels);
                _prometheusUtils.ReportGauge(Collectors, "LastWrittenTransactionId", component.Metrics.Dfs.System.LastWrittenTransactionId, labels);
                _prometheusUtils.ReportGauge(Collectors, "MillisSinceLastLoadedEdits", component.Metrics.Dfs.System.MillisSinceLastLoadedEdits, labels);
                _prometheusUtils.ReportGauge(Collectors, "MissingBlocks", component.Metrics.Dfs.System.MissingBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "MissingReplOneBlocks", component.Metrics.Dfs.System.MissingReplOneBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "PendingDataNodeMessageCount", component.Metrics.Dfs.System.PendingDataNodeMessageCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "PendingDeletionBlocks", component.Metrics.Dfs.System.PendingDeletionBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "PendingReplicationBlocks", component.Metrics.Dfs.System.PendingReplicationBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "PostponedMisreplicatedBlocks", component.Metrics.Dfs.System.PostponedMisreplicatedBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "ScheduledReplicationBlocks", component.Metrics.Dfs.System.ScheduledReplicationBlocks, labels);
                _prometheusUtils.ReportGauge(Collectors, "Snapshots", component.Metrics.Dfs.System.Snapshots, labels);
                _prometheusUtils.ReportGauge(Collectors, "SnapshottableDirectories", component.Metrics.Dfs.System.SnapshottableDirectories, labels);
                _prometheusUtils.ReportGauge(Collectors, "StaleDataNodes", component.Metrics.Dfs.System.StaleDataNodes, labels);
                _prometheusUtils.ReportGauge(Collectors, "TotalFiles", component.Metrics.Dfs.System.TotalFiles, labels);
                _prometheusUtils.ReportGauge(Collectors, "TotalLoad", component.Metrics.Dfs.System.TotalLoad, labels);
                _prometheusUtils.ReportGauge(Collectors, "TransactionsSinceLastCheckpoint", component.Metrics.Dfs.System.TransactionsSinceLastCheckpoint, labels);
                _prometheusUtils.ReportGauge(Collectors, "TransactionsSinceLastLogRoll", component.Metrics.Dfs.System.TransactionsSinceLastLogRoll, labels);
                _prometheusUtils.ReportGauge(Collectors, "UnderReplicatedBlocks", component.Metrics.Dfs.System.UnderReplicatedBlocks, labels);

                // Disk
                _prometheusUtils.ReportGauge(Collectors, "Disk_Free", component.Metrics.Disk.Free, labels);
                _prometheusUtils.ReportGauge(Collectors, "Disk_ReadBytes", component.Metrics.Disk.ReadBytes, labels);
                _prometheusUtils.ReportGauge(Collectors, "Disk_ReadCount", component.Metrics.Disk.ReadCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Disk_ReadTime", component.Metrics.Disk.ReadTime, labels);
                _prometheusUtils.ReportGauge(Collectors, "Disk_Total", component.Metrics.Disk.Total, labels);
                _prometheusUtils.ReportGauge(Collectors, "Disk_WriteBytes", component.Metrics.Disk.WriteBytes, labels);
                _prometheusUtils.ReportGauge(Collectors, "Disk_WriteCount", component.Metrics.Disk.WriteCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Disk_WriteTime", component.Metrics.Disk.WriteTime, labels);

                // HdfsNameNodeJvm
                _prometheusUtils.ReportGauge(Collectors, "Jvm_GcCountConcurrentMarkSweep", component.Metrics.HdfsNameNodeJvm.GcCountConcurrentMarkSweep, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_GcTimeMillisConcurrentMarkSweep", component.Metrics.HdfsNameNodeJvm.GcTimeMillisConcurrentMarkSweep, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_gcCount", component.Metrics.HdfsNameNodeJvm.GcCount, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_gcTimeMillis", component.Metrics.HdfsNameNodeJvm.GcTimeMillis, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_logError", component.Metrics.HdfsNameNodeJvm.LogError, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_logFatal", component.Metrics.HdfsNameNodeJvm.LogFatal, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_logInfo", component.Metrics.HdfsNameNodeJvm.LogInfo, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_logWarn", component.Metrics.HdfsNameNodeJvm.LogWarn, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_memHeapCommittedM", component.Metrics.HdfsNameNodeJvm.MemHeapCommittedM, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_memHeapUsedM", component.Metrics.HdfsNameNodeJvm.MemHeapUsedM, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_memMaxM", component.Metrics.HdfsNameNodeJvm.MemMaxM, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_memNonHeapCommittedM", component.Metrics.HdfsNameNodeJvm.MemNonHeapCommittedM, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_memNonHeapUsedM", component.Metrics.HdfsNameNodeJvm.MemNonHeapUsedM, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_threadsBlocked", component.Metrics.HdfsNameNodeJvm.ThreadsBlocked, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_threadsNew", component.Metrics.HdfsNameNodeJvm.ThreadsNew, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_threadsRunnable", component.Metrics.HdfsNameNodeJvm.ThreadsRunnable, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_threadsTerminated", component.Metrics.HdfsNameNodeJvm.ThreadsTerminated, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_threadsTimedWaiting", component.Metrics.HdfsNameNodeJvm.ThreadsTimedWaiting, labels);
                _prometheusUtils.ReportGauge(Collectors, "Jvm_threadsWaiting", component.Metrics.HdfsNameNodeJvm.ThreadsWaiting, labels);

                // Memory
                _prometheusUtils.ReportGauge(Collectors, "Memory_CachedKb", component.Metrics.Memory.CachedKb, labels);
                _prometheusUtils.ReportGauge(Collectors, "Memory_FreeKb", component.Metrics.Memory.FreeKb, labels);
                _prometheusUtils.ReportGauge(Collectors, "Memory_SharedKb", component.Metrics.Memory.SharedKb, labels);
                _prometheusUtils.ReportGauge(Collectors, "Memory_SwapFreeKb", component.Metrics.Memory.SwapFreeKb, labels);
                _prometheusUtils.ReportGauge(Collectors, "Memory_TotalKb", component.Metrics.Memory.TotalKb, labels);

                // Network
                _prometheusUtils.ReportGauge(Collectors, "Network_BytesIn", component.Metrics.Network.BytesIn, labels);
                _prometheusUtils.ReportGauge(Collectors, "Network_BytesOut", component.Metrics.Network.BytesOut, labels);
                _prometheusUtils.ReportGauge(Collectors, "Network_PktsIn", component.Metrics.Network.PktsIn, labels);
                _prometheusUtils.ReportGauge(Collectors, "Network_PktsOut", component.Metrics.Network.PktsOut, labels);

                // Process
                _prometheusUtils.ReportGauge(Collectors, "Process_Run", component.Metrics.Process.Run, labels);
                _prometheusUtils.ReportGauge(Collectors, "Process_Total", component.Metrics.Process.Total, labels);

                // YarnResourceManagerRpc
                _prometheusUtils.ReportGauge(Collectors, "Rpc_NumOpenConnections", component.Metrics.Rpc.Client.NumOpenConnections, labels);
                _prometheusUtils.ReportGauge(Collectors, "Rpc_ReceivedBytes", component.Metrics.Rpc.Client.ReceivedBytes, labels);
                _prometheusUtils.ReportGauge(Collectors, "Rpc_ProcessingTimeAvgTime", component.Metrics.Rpc.Client.ProcessingTimeAvgTime, labels);
                _prometheusUtils.ReportGauge(Collectors, "Rpc_QueueTimeAvgTime", component.Metrics.Rpc.Client.QueueTimeAvgTime, labels);
                _prometheusUtils.ReportGauge(Collectors, "Rpc_SentBytes", component.Metrics.Rpc.Client.SentBytes, labels);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to export metrics.");
                throw;
            }
        }
    }
}
