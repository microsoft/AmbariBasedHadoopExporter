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
    internal class YarnResourceManagerExporter : BaseExporter
    {
        private readonly YarnResourceManagerExporterConfiguration _exporterConfiguration;

        public YarnResourceManagerExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            IOptions<YarnResourceManagerExporterConfiguration> exporterConfiguration,
            ILogger<YarnResourceManagerExporter> logger)
            : base(contentProvider, prometheusUtils, exporterConfiguration.Value, typeof(ResourceManagerComponent), logger)
        {
            _exporterConfiguration = exporterConfiguration.Value;
        }

        /// <summary>
        /// Internal method used to extract queue metrics by queue name.
        /// </summary>
        /// <param name="queue">Queue object</param>
        /// <param name="labels">Additional labels</param>
        internal void ReportQueueMetrics(Queue queue, Dictionary<string, string> labels)
        {
            // Constructing labels
            var localLabels = new Dictionary<string, string>()
            {
                { "QueueName", queue.Name },
            };
            localLabels.TryAdd(labels);

            PrometheusUtils.ReportGauge(Collectors, "Queue_AMResourceLimitMB", queue.AMResourceLimitMB, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AMResourceLimitVCores", queue.AMResourceLimitVCores, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_ActiveApplications", queue.ActiveApplications, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_ActiveUsers", queue.ActiveUsers, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AggregateContainersAllocated", queue.AggregateContainersAllocated, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AggregateContainersReleased", queue.AggregateContainersReleased, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AllocatedContainers", queue.AllocatedContainers, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AllocatedMB", queue.AllocatedMB, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AllocatedVCores", queue.AllocatedVCores, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppAttemptFirstContainerAllocationDelayAvgTime", queue.AppAttemptFirstContainerAllocationDelayAvgTime, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppAttemptFirstContainerAllocationDelayNumOps", queue.AppAttemptFirstContainerAllocationDelayNumOps, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppsCompleted", queue.AppsCompleted, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppsFailed", queue.AppsFailed, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppsKilled", queue.AppsKilled, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppsPending", queue.AppsPending, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppsRunning", queue.AppsRunning, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AppsSubmitted", queue.AppsSubmitted, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AvailableMB", queue.AvailableMB, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_AvailableVCores", queue.AvailableVCores, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_PendingContainers", queue.PendingContainers, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_PendingMB", queue.PendingMB, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_PendingVCores", queue.PendingVCores, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_ReservedContainers", queue.ReservedContainers, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_ReservedMB", queue.ReservedMB, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_ReservedVCores", queue.ReservedVCores, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_UsedAMResourceMB", queue.UsedAMResourceMB, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_UsedAMResourceVCores", queue.UsedAMResourceVCores, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_Running0", queue.Running0, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_Running1440", queue.Running1440, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_Running300", queue.Running300, localLabels);
            PrometheusUtils.ReportGauge(Collectors, "Queue_Running60", queue.Running60, localLabels);
        }

        /// <inheritdoc/>
        protected override async Task ReportMetrics(object component)
        {
            await Task.Factory.StartNew(() =>
            {
                var resourceManagerComponent = (ResourceManagerComponent)component;

                // Constructing labels
                var labels = new Dictionary<string, string>()
                {
                    { "ClusterName", resourceManagerComponent.Info.ClusterName },
                    { "Component", resourceManagerComponent.Info.ComponentName },
                };
                labels.TryAdd(_exporterConfiguration.DefaultLabels);

                // General info
                PrometheusUtils.ReportGauge(Collectors, "Info_StartTime", resourceManagerComponent.Info.StartTime, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_StartedCount", resourceManagerComponent.Info.StartedCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_TotalCount", resourceManagerComponent.Info.TotalCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Info_UnknownCount", resourceManagerComponent.Info.UnknownCount, labels);

                // Yarn queues
                ReportQueueMetrics(resourceManagerComponent.Metrics.YarnMetrics.Queues.Root, labels);
                foreach (var queue in resourceManagerComponent.Metrics.YarnMetrics.Queues.Root.GetChildrenQueuesIEnumerable())
                {
                    ReportQueueMetrics(queue, labels);
                }

                // Node Manager
                PrometheusUtils.ReportGauge(Collectors, "NumActiveNMs", resourceManagerComponent.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumActiveNMs, labels);
                PrometheusUtils.ReportGauge(Collectors, "NumDecommissionedNMs", resourceManagerComponent.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumDecommissionedNMs, labels);
                PrometheusUtils.ReportGauge(Collectors, "NumLostNMs", resourceManagerComponent.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumLostNMs, labels);
                PrometheusUtils.ReportGauge(Collectors, "NumRebootedNMs", resourceManagerComponent.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumRebootedNMs, labels);
                PrometheusUtils.ReportGauge(Collectors, "NumUnhealthyNMs", resourceManagerComponent.Metrics.YarnMetrics.YarnResourceManagerClusterMetrics.NumUnhealthyNMs, labels);

                // HdfsNameNodeJvm
                PrometheusUtils.ReportGauge(Collectors, "Jvm_HeapMemoryMax", resourceManagerComponent.Metrics.ResourceManagerJvm.HeapMemoryMax, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_HeapMemoryUsed", resourceManagerComponent.Metrics.ResourceManagerJvm.HeapMemoryUsed, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_NonHeapMemoryMax", resourceManagerComponent.Metrics.ResourceManagerJvm.NonHeapMemoryMax, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_NonHeapMemoryUsed", resourceManagerComponent.Metrics.ResourceManagerJvm.NonHeapMemoryUsed, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_GcCount", resourceManagerComponent.Metrics.ResourceManagerJvm.GcCount, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_GcTimeMillis", resourceManagerComponent.Metrics.ResourceManagerJvm.GcTimeMillis, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_LogError", resourceManagerComponent.Metrics.ResourceManagerJvm.LogError, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_LogFatal", resourceManagerComponent.Metrics.ResourceManagerJvm.LogFatal, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_LogInfo", resourceManagerComponent.Metrics.ResourceManagerJvm.LogInfo, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_LogWarn", resourceManagerComponent.Metrics.ResourceManagerJvm.LogWarn, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapCommittedM", resourceManagerComponent.Metrics.ResourceManagerJvm.MemHeapCommittedM, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_MemHeapUsedMB", resourceManagerComponent.Metrics.ResourceManagerJvm.MemHeapUsedMB, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_MemMaxMB", resourceManagerComponent.Metrics.ResourceManagerJvm.MemMaxMB, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_MemNonHeapCommittedMB", resourceManagerComponent.Metrics.ResourceManagerJvm.MemNonHeapCommittedMB, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_MemNonHeapUsedMB", resourceManagerComponent.Metrics.ResourceManagerJvm.MemNonHeapUsedMB, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsBlocked", resourceManagerComponent.Metrics.ResourceManagerJvm.ThreadsBlocked, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsNew", resourceManagerComponent.Metrics.ResourceManagerJvm.ThreadsNew, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsRunnable", resourceManagerComponent.Metrics.ResourceManagerJvm.ThreadsRunnable, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsTerminated", resourceManagerComponent.Metrics.ResourceManagerJvm.ThreadsTerminated, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsTimedWaiting", resourceManagerComponent.Metrics.ResourceManagerJvm.ThreadsTimedWaiting, labels);
                PrometheusUtils.ReportGauge(Collectors, "Jvm_ThreadsWaiting", resourceManagerComponent.Metrics.ResourceManagerJvm.ThreadsWaiting, labels);

                // YarnResourceManagerRpc
                PrometheusUtils.ReportGauge(Collectors, "Rpc_NumOpenConnections", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.NumOpenConnections, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_ReceivedBytes", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.ReceivedBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcProcessingTime_avg_time", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcProcessingTime_avg_time, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcProcessingTime_num_ops", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcProcessingTime_num_ops, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcQueueTime_avg_time", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcQueueTime_avg_time, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcQueueTime_num_ops", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcQueueTime_num_ops, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_SentBytes", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.SentBytes, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_CallQueueLen", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.CallQueueLen, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthenticationFailures", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthenticationFailures, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthenticationSuccesses", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthenticationSuccesses, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthorizationFailures", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthorizationFailures, labels);
                PrometheusUtils.ReportGauge(Collectors, "Rpc_RpcAuthorizationSuccesses", resourceManagerComponent.Metrics.ResourceManagerYarnResourceManagerRpc.RpcAuthorizationSuccesses, labels);

                // Runtime
                PrometheusUtils.ReportGauge(Collectors, "Runtime_StartTime", resourceManagerComponent.Metrics.Runtime.StartTime, labels);

                // Ugi
                PrometheusUtils.ReportGauge(Collectors, "Ugi_LoginFailure_avg_time", resourceManagerComponent.Metrics.Ugi.LoginFailure_avg_time, labels);
                PrometheusUtils.ReportGauge(Collectors, "Ugi_LoginFailure_num_ops", resourceManagerComponent.Metrics.Ugi.LoginFailure_num_ops, labels);
                PrometheusUtils.ReportGauge(Collectors, "Ugi_LoginSuccess_avg_time", resourceManagerComponent.Metrics.Ugi.LoginSuccess_avg_time, labels);
                PrometheusUtils.ReportGauge(Collectors, "Ugi_LoginSuccess_num_ops", resourceManagerComponent.Metrics.Ugi.LoginSuccess_num_ops, labels);
            });
        }
    }
}
