// <copyright file="HostExporterTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.UnitTests.Exporters
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
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
    using Prometheus;
    using Xunit;

    public class HostExporterTests
    {
        private readonly Mock<IContentProvider> _contentProvider;
        private readonly Mock<IPrometheusUtils> _prometheusUtils;
        private readonly Mock<IOptions<HostExporterConfiguration>> _hostConfigurationOptions;
        private readonly Mock<HostExporterConfiguration> _hostConfiguration;
        private readonly Mock<ILogger<HostExporter>> _logger;
        private readonly HostExporter _exporter;

        public HostExporterTests()
        {
            _contentProvider = new Mock<IContentProvider>();
            _prometheusUtils = new Mock<IPrometheusUtils>();
            _hostConfigurationOptions = new Mock<IOptions<HostExporterConfiguration>>();
            _hostConfiguration = new Mock<HostExporterConfiguration>();
            _hostConfiguration.Setup(f => f.UriEndpoint).Returns("host");
            _hostConfigurationOptions.Setup(f => f.Value).Returns(_hostConfiguration.Object);

            _logger = new Mock<ILogger<HostExporter>>();

            _exporter = new HostExporter(
                _contentProvider.Object,
                _prometheusUtils.Object,
                _hostConfigurationOptions.Object,
                _logger.Object);
        }

        [Fact]
        public void Exporting_Host_Should_Run_Successfully()
        {
            int reportedCounter = 0;
            _prometheusUtils.Setup(f => f.ReportGauge(
                It.IsAny<ConcurrentDictionary<string, Collector>>(),
                It.IsAny<string>(),
                It.IsAny<double>(),
                It.IsAny<Dictionary<string, string>>(),
                It.IsAny<string>())).Callback(() => { reportedCounter++; });
            var content = File.ReadAllText("Jsons/HostsResponse.json");
            _contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.FromResult(content));

            Func<Task> func = async () => await _exporter.ExportMetricsAsync("hn1-sparkp.pk3tke12z5uejpo3re1mqa2wyf.fx.internal.cloudapp.net");

            func.Should().NotThrow();
            reportedCounter.Should().Be(25);
        }
    }
}
