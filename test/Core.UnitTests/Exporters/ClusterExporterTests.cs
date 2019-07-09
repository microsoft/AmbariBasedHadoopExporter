// <copyright file="ClusterExporterTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Exporters
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Exporters.Concrete;
    using Core.Providers;
    using Core.Utils;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class ClusterExporterTests
    {
        private readonly Mock<IContentProvider> _contentProvider;
        private readonly Mock<IPrometheusUtils> _prometheusUtils;
        private readonly Mock<IOptions<ClusterExporterConfiguration>> _configurationOptions;
        private readonly Mock<IOptions<HostExporterConfiguration>> _hostConfigurationOptions;
        private readonly Mock<ClusterExporterConfiguration> _configuration;
        private readonly Mock<HostExporterConfiguration> _hostConfiguration;
        private readonly Mock<ILogger<ClusterExporter>> _logger;
        private readonly ClusterExporter _exporter;

        public ClusterExporterTests()
        {
            _contentProvider = new Mock<IContentProvider>();
            _prometheusUtils = new Mock<IPrometheusUtils>();

            _configurationOptions = new Mock<IOptions<ClusterExporterConfiguration>>();
            _configuration = new Mock<ClusterExporterConfiguration>();
            _configuration.Setup(f => f.UriEndpoint).Returns("cluster");
            _configurationOptions.Setup(f => f.Value).Returns(_configuration.Object);

            _hostConfigurationOptions = new Mock<IOptions<HostExporterConfiguration>>();
            _hostConfiguration = new Mock<HostExporterConfiguration>();
            _hostConfiguration.Setup(f => f.UriEndpoint).Returns("host");
            _hostConfigurationOptions.Setup(f => f.Value).Returns(_hostConfiguration.Object);

            _logger = new Mock<ILogger<ClusterExporter>>();

            _exporter = new ClusterExporter(
                _contentProvider.Object,
                _prometheusUtils.Object,
                _configurationOptions.Object,
                _hostConfigurationOptions.Object,
                _logger.Object);
        }

        [Fact]
        public void Should_Run_Successfully()
        {
            var content = File.ReadAllText("Jsons/ClusterResponse.json");
            _contentProvider.Setup(f => f.GetResponseContentAsync(It.IsRegex("^(?!.*host).*$"))).Returns(Task.FromResult(content));
            var hostContent = File.ReadAllText("Jsons/HostsResponse.json");
            _contentProvider.Setup(f => f.GetResponseContentAsync(It.IsRegex(".*host.*"))).Returns(Task.FromResult(hostContent));

            Func<Task> func = async () => { await _exporter.ExportMetricsAsync(); };

            func.Should().NotThrow();
        }

        [Fact]
        public void Should_Raise_Exception()
        {
            _contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.FromResult("invalid"));

            Func<Task> func = async () => { await _exporter.ExportMetricsAsync(); };

            func.Should().Throw<Exception>();
        }
    }
}
