// <copyright file="YarnResourceManagerExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Exporters.Abstract;
    using Core.Extensions;
    using Core.Models.AmbariResponseEntities.YarnResourceManager;
    using Core.Models.Components;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;
    using Prometheus;

    /// <summary>
    /// Responsible for exporting Yarn ResourceManager and configured YARN queues metrics.
    /// </summary>
    internal class YarnResourceManagerExporter : IExporter
    {
        private readonly IContentProvider _contentProvider;
        private readonly YarnResourceManagerExporterConfiguration _exporterConfiguration;
        private readonly ILogger<YarnResourceManagerExporter> _logger;
        private readonly IPrometheusUtils _prometheusUtils;

        public YarnResourceManagerExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<YarnResourceManagerExporterConfiguration> exporterConfiguration,
            ILogger<YarnResourceManagerExporter> logger)
        {
            _contentProvider = contentProvider;
            _exporterConfiguration = exporterConfiguration.Value;
            _logger = logger;
            _prometheusUtils = prometheusUtils;
            Collectors = new ConcurrentDictionary<string, Collector>();
        }

        /// <inheritdoc/>
        public ConcurrentDictionary<string, Collector> Collectors { get; private set; }

        /// <inheritdoc/>
        public async Task ExportMetricsAsync()
        {
            try
            {
                using (_logger.BeginScope(new Dictionary<string, object>() { { "Exporter", GetType().Name }, }))
                {
                    _logger.LogInformation($"{nameof(ExportMetricsAsync)} Started.");
                    var stopWatch = Stopwatch.StartNew();

                    var content = await _contentProvider.GetResponseContentAsync(_exporterConfiguration.UriEndpoint);
                    var component = JsonConvert.DeserializeObject<ResourceManagerComponent>(content);

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

                    // Yarn queues
                    ReportQueueMetrics(component.Metrics.YarnMetrics.Queues.Root, labels);
                    foreach (var queue in component.Metrics.YarnMetrics.Queues.Root.GetChildrenQueuesIEnumerable())
                    {
                        ReportQueueMetrics(queue, labels);
                    }

                    // Node Manager
                    _prometheusUtils.ReportGauge(Collectors, "NumActiveNMs", component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumActiveNMs, labels);
                    _prometheusUtils.ReportGauge(Collectors, "NumDecommissionedNMs", component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumDecommissionedNMs, labels);
                    _prometheusUtils.ReportGauge(Collectors, "NumLostNMs", component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumLostNMs, labels);
                    _prometheusUtils.ReportGauge(Collectors, "NumRebootedNMs", component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumRebootedNMs, labels);
                    _prometheusUtils.ReportGauge(Collectors, "NumUnhealthyNMs", component.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumUnhealthyNMs, labels);

                    // HdfsNameNodeJvm
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_HeapMemoryMax", component.Metrics.ResourceManagerJvm.HeapMemoryMax, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_HeapMemoryUsed", component.Metrics.ResourceManagerJvm.HeapMemoryUsed, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_NonHeapMemoryMax", component.Metrics.ResourceManagerJvm.NonHeapMemoryMax, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_NonHeapMemoryUsed", component.Metrics.ResourceManagerJvm.NonHeapMemoryUsed, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_GcCount", component.Metrics.ResourceManagerJvm.GcCount, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_GcTimeMillis", component.Metrics.ResourceManagerJvm.GcTimeMillis, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_LogError", component.Metrics.ResourceManagerJvm.LogError, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_LogFatal", component.Metrics.ResourceManagerJvm.LogFatal, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_LogInfo", component.Metrics.ResourceManagerJvm.LogInfo, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_LogWarn", component.Metrics.ResourceManagerJvm.LogWarn, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapCommittedM", component.Metrics.ResourceManagerJvm.MemHeapCommittedM, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapUsedMB", component.Metrics.ResourceManagerJvm.MemHeapUsedMB, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_MemMaxMB", component.Metrics.ResourceManagerJvm.MemMaxMB, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_MemNonHeapCommittedMB", component.Metrics.ResourceManagerJvm.MemNonHeapCommittedMB, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_MemNonHeapUsedMB", component.Metrics.ResourceManagerJvm.MemNonHeapUsedMB, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsBlocked", component.Metrics.ResourceManagerJvm.ThreadsBlocked, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsNew", component.Metrics.ResourceManagerJvm.ThreadsNew, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsRunnable", component.Metrics.ResourceManagerJvm.ThreadsRunnable, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsTerminated", component.Metrics.ResourceManagerJvm.ThreadsTerminated, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsTimedWaiting", component.Metrics.ResourceManagerJvm.ThreadsTimedWaiting, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsWaiting", component.Metrics.ResourceManagerJvm.ThreadsWaiting, labels);

                    // YarnResourceManagerRpc
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_NumOpenConnections", component.Metrics.ResourceManagerYarnResourceManagerRpc.NumOpenConnections, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_ReceivedBytes", component.Metrics.ResourceManagerYarnResourceManagerRpc.ReceivedBytes, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcProcessingTime_avg_time", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcProcessingTime_avg_time, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcProcessingTime_num_ops", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcProcessingTime_num_ops, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcQueueTime_avg_time", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcQueueTime_avg_time, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcQueueTime_num_ops", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcQueueTime_num_ops, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_SentBytes", component.Metrics.ResourceManagerYarnResourceManagerRpc.SentBytes, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_CallQueueLen", component.Metrics.ResourceManagerYarnResourceManagerRpc.CallQueueLen, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthenticationFailures", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthenticationFailures, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthenticationSuccesses", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthenticationSuccesses, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthorizationFailures", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthorizationFailures, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthorizationSuccesses", component.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthorizationSuccesses, labels);

                    // Runtime
                    _prometheusUtils.ReportGauge(Collectors, "Runtime_StartTime", component.Metrics.Runtime.StartTime, labels);

                    // Ugi
                    _prometheusUtils.ReportGauge(Collectors, "Ugi_LoginFailure_avg_time", component.Metrics.Ugi.LoginFailure_avg_time, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Ugi_LoginFailure_num_ops", component.Metrics.Ugi.LoginFailure_num_ops, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Ugi_LoginSuccess_avg_time", component.Metrics.Ugi.LoginSuccess_avg_time, labels);
                    _prometheusUtils.ReportGauge(Collectors, "Ugi_LoginSuccess_num_ops", component.Metrics.Ugi.LoginSuccess_num_ops, labels);

                    // Tracing
                    stopWatch.Stop();
                    _logger.LogInformation($"Runtime: {stopWatch.Elapsed}.");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to export metrics.");
                throw;
            }
        }

        internal void ReportQueueMetrics(Queue queue, Dictionary<string, string> labels)
        {
            // Constructing labels
            var localLabels = new Dictionary<string, string>()
            {
                { "QueueName", queue.Name },
            };
            localLabels.TryAdd(labels);

            _prometheusUtils.ReportGauge(Collectors, "Queue_AMResourceLimitMB", queue.AMResourceLimitMB, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AMResourceLimitVCores", queue.AMResourceLimitVCores, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_ActiveApplications", queue.ActiveApplications, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_ActiveUsers", queue.ActiveUsers, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AggregateContainersAllocated", queue.AggregateContainersAllocated, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AggregateContainersReleased", queue.AggregateContainersReleased, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AllocatedContainers", queue.AllocatedContainers, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AllocatedMB", queue.AllocatedMB, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AllocatedVCores", queue.AllocatedVCores, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppAttemptFirstContainerAllocationDelayAvgTime", queue.AppAttemptFirstContainerAllocationDelayAvgTime, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppAttemptFirstContainerAllocationDelayNumOps", queue.AppAttemptFirstContainerAllocationDelayNumOps, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppsCompleted", queue.AppsCompleted, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppsFailed", queue.AppsFailed, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppsKilled", queue.AppsKilled, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppsPending", queue.AppsPending, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppsRunning", queue.AppsRunning, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AppsSubmitted", queue.AppsSubmitted, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AvailableMB", queue.AvailableMB, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_AvailableVCores", queue.AvailableVCores, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_PendingContainers", queue.PendingContainers, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_PendingMB", queue.PendingMB, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_PendingVCores", queue.PendingVCores, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_ReservedContainers", queue.ReservedContainers, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_ReservedMB", queue.ReservedMB, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_ReservedVCores", queue.ReservedVCores, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_UsedAMResourceMB", queue.UsedAMResourceMB, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_UsedAMResourceVCores", queue.UsedAMResourceVCores, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_Running0", queue.Running0, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_Running1440", queue.Running1440, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_Running300", queue.Running300, localLabels);
            _prometheusUtils.ReportGauge(Collectors, "Queue_Running60", queue.Running60, localLabels);
        }
    }
}
