// <copyright file="LivenessHostedService.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Services.Hosted
{
    using System;
    using System.IO;
    using System.Threading;
    using System.Threading.Tasks;
    using App.Configuration;
    using Core.Configurations.Exporters;
    using Core.Models.AmbariResponseEntities.Cluster;
    using Core.Providers;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Newtonsoft.Json;

    /// <summary>
    /// Liveness hosted servie that will be used by Kubernetes to report the status of our pod.
    /// </summary>
    public class LivenessHostedService : IHostedService, IDisposable
    {
        private readonly IContentProvider _contentProvider;
        private readonly ClusterExporterConfiguration _clusterExporterConfiguration;
        private readonly LivenessConfiguration _configuration;
        private readonly ILogger<LivenessHostedService> _logger;
        private Timer _timer;

        public LivenessHostedService(
            IContentProvider contentProvider,
            IOptions<ClusterExporterConfiguration> clusterExporterConfiguration,
            IOptions<LivenessConfiguration> configuration,
            ILogger<LivenessHostedService> logger)
        {
            _contentProvider = contentProvider;
            _clusterExporterConfiguration = clusterExporterConfiguration.Value;
            _configuration = configuration.Value;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Starting the service - initializing and starting the timer.");
            _timer = new Timer(HealthCheck, null, TimeSpan.Zero, TimeSpan.FromSeconds(_configuration.SamplingPeriodInSeconds));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Stopping the service - stopping the timer.");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        /// <summary>
        /// Validating access to ambari.
        /// </summary>
        /// <param name="state">State</param>
        internal async void HealthCheck(object state)
        {
            try
            {
                _logger.LogInformation("Running HealthCheck.");

                var content = await _contentProvider.GetResponseContentAsync(_clusterExporterConfiguration.AmbariServerUri);
                var clusters = JsonConvert.DeserializeObject<Clusters>(content);

                // Creating a temporary file which notify Kubernetes that we're healthy.
                File.Create(_configuration.LivenessFilePath == null ? "/tmp/healthy" : _configuration.LivenessFilePath).Close();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "HealthCheck failed.");

                System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(e.InnerException).Throw();
                throw;
            }
        }
    }
}
