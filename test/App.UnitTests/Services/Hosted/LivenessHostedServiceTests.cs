// <copyright file="LivenessHostedServiceTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.UnitTests.Services.Hosted
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using App.Configuration;
    using App.Services.Hosted;
    using Core.Configurations.Exporters;
    using Core.Providers;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class LivenessHostedServiceTests
    {
        private readonly Mock<IContentProvider> _contentProvider;
        private readonly Mock<IOptions<ClusterExporterConfiguration>> _clusterExporterConfiguration;
        private readonly Mock<ILogger<LivenessHostedService>> _logger;

        public LivenessHostedServiceTests()
        {
            _contentProvider = new Mock<IContentProvider>();

            var clusterExporterConfiguration = new ClusterExporterConfiguration
            {
                AmbariServerUri = string.Empty
            };
            _clusterExporterConfiguration = new Mock<IOptions<ClusterExporterConfiguration>>();
            _clusterExporterConfiguration.Setup(f => f.Value).Returns(clusterExporterConfiguration);

            _logger = new Mock<ILogger<LivenessHostedService>>();
        }

        [Fact]
        public void Create_Health_Status_File_Should_Run_Successfully()
        {
            // Setup
            var livenessConfiguration = new Mock<IOptions<LivenessConfiguration>>();
            livenessConfiguration.Setup(f => f.Value).Returns(new LivenessConfiguration
            {
                LivenessFilePath = $"{Directory.GetCurrentDirectory()}/healthy"
            });

            var livenessHostedService = new LivenessHostedService(
                _contentProvider.Object,
                _clusterExporterConfiguration.Object,
                livenessConfiguration.Object,
                _logger.Object);

            // Test content
            var content = File.ReadAllText("Jsons/ClustersResponse.json");
            _contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.Run(() => content));

            Action a = () => livenessHostedService.HealthCheck(null);

            a.Should().NotThrow();
        }
    }
}
