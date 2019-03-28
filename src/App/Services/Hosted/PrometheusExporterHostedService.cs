// <copyright file="PrometheusExporterHostedService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Services.Hosted
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Configuration;
    using Core.Exporters.Abstract;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Prometheus;

    /// <summary>
    /// Main logic of our process, this is where we invoke Prometheus MetricServer and register our exporters.
    /// </summary>
    internal class PrometheusExporterHostedService : IHostedService
    {
        private readonly IEnumerable<IExporter> _exporters;
        private readonly PrometheusExporterConfiguration _configuration;
        private readonly MetricServer _metricServer;
        private readonly ILogger<PrometheusExporterHostedService> _logger;

        public PrometheusExporterHostedService(
            IEnumerable<IExporter> exporters,
            IOptions<PrometheusExporterConfiguration> configuration,
            ILogger<PrometheusExporterHostedService> logger)
        {
            _exporters = exporters;
            _configuration = configuration.Value;
            _logger = logger;
            _metricServer = new MetricServer(_configuration.Port);
        }

        /// <inheritdoc/>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting Prometheus MetricServer at {DateTime.UtcNow}.");

            // Not using 'await' because the register is running synchronously
            Metrics.DefaultRegistry.AddBeforeCollectCallback(() => RunExportersAsync().ConfigureAwait(false).GetAwaiter().GetResult());
            _metricServer.Start();

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping Prometheus MetricServer at {DateTime.UtcNow}.");

            _metricServer.Stop();

            return Task.CompletedTask;
        }

        /// <summary>
        /// Registering all exporters that implement IExporter interface.
        /// </summary>
        /// <returns>Completed task.</returns>
        private async Task RunExportersAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            var tasks = new List<Task>();
            foreach (var exporter in _exporters)
            {
                var task = exporter.ExportMetricsAsync();
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            _logger.LogInformation($"{nameof(RunExportersAsync)} took {stopwatch.Elapsed}.");
        }
    }
}
