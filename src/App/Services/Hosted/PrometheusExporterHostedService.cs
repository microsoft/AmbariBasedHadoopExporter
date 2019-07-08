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
        // Self monitoring metrics
        internal readonly Counter TotalScrapeActivations;
        internal readonly Counter TotalSuccessfulScrapeActivations;
        internal readonly Gauge IsSuccessful;
        internal readonly Histogram ScrapeTime;

        // Private members
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

            // Init metrics
            TotalScrapeActivations = Metrics.CreateCounter(
                "total_activations",
                "Total activations of the exporter",
                new CounterConfiguration() { SuppressInitialValue = true });
            TotalSuccessfulScrapeActivations = Metrics.CreateCounter(
                "total_success_activations",
                "Total successful activation of the exporter",
                new CounterConfiguration() { SuppressInitialValue = true });
            IsSuccessful = Metrics.CreateGauge(
                "is_successful_scrape",
                "Indication to if the last scrape was successful",
                new GaugeConfiguration() { SuppressInitialValue = true });
            ScrapeTime = Metrics.CreateHistogram(
                "scrape_time",
                "Total scraping time of the exporter",
                new HistogramConfiguration() { SuppressInitialValue = true });
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
        internal async Task RunExportersAsync()
        {
            var stopwatch = Stopwatch.StartNew();

            try
            {
                TotalScrapeActivations.Inc();

                var tasks = new List<Task>();
                foreach (var exporter in _exporters)
                {
                    var task = exporter.ExportMetricsAsync();
                    tasks.Add(task);
                }

                await Task.WhenAll(tasks);
                IsSuccessful.Set(1);
                TotalSuccessfulScrapeActivations.Inc();
            }
            catch (AggregateException ae)
            {
                foreach (var innerException in ae.Flatten().InnerExceptions)
                {
                    _logger.LogError($"{nameof(RunExportersAsync)} failed. Message: {innerException.Message}");
                }

                IsSuccessful.Set(0);
            }
            finally
            {
                stopwatch.Stop();
                _logger.LogInformation($"{nameof(RunExportersAsync)} took {stopwatch.Elapsed}.");

                // Sending metrics
                ScrapeTime.Observe(stopwatch.Elapsed.TotalSeconds);
            }
        }
    }
}
