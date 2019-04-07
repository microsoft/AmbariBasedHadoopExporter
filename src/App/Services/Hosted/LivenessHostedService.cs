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
    public class LivenessHostedService : IHostedService
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
            _timer?.Dispose();
            _timer = new Timer(HealthCheck, null, TimeSpan.Zero, TimeSpan.FromMinutes(_configuration.SamplingPeriodInMinutes));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Dispose();
            return Task.CompletedTask;
        }

        /// <summary>
        /// Validating access to ambari.
        /// </summary>
        /// <param name="state">State</param>
        internal async void HealthCheck(object state)
        {
            try
            {
                var content = await _contentProvider.GetResponseContentAsync(_clusterExporterConfiguration.AmbariServerUri);
                var clusters = JsonConvert.DeserializeObject<Clusters>(content);

                // Creating a temporary file which notify Kubernetes that we're healthy.
                File.Create("/tmp/healthy").Close();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "HealthCheck failed.");
                throw e;
            }
        }
    }
}
