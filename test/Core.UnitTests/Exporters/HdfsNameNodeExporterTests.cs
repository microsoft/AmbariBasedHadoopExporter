// <copyright file="HdfsNameNodeExporterTests.cs" company="Microsoft">
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

    public class HdfsNameNodeExporterTests
    {
        private readonly Mock<IContentProvider> _contentProvider;
        private readonly Mock<IPrometheusUtils> _prometheusUtils;
        private readonly Mock<IOptions<HdfsNameNodeExporterConfiguration>> _configurationOptions;
        private readonly Mock<HdfsNameNodeExporterConfiguration> _configuration;
        private readonly Mock<ILogger<HdfsNameNodeExporter>> _logger;
        private readonly HdfsNameNodeExporter _exporter;

        public HdfsNameNodeExporterTests()
        {
            _contentProvider = new Mock<IContentProvider>();
            _prometheusUtils = new Mock<IPrometheusUtils>();

            _configurationOptions = new Mock<IOptions<HdfsNameNodeExporterConfiguration>>();
            _configuration = new Mock<HdfsNameNodeExporterConfiguration>();
            _configuration.Setup(f => f.UriEndpoint).Returns("cluster");
            _configurationOptions.Setup(f => f.Value).Returns(_configuration.Object);

            _logger = new Mock<ILogger<HdfsNameNodeExporter>>();

            _exporter = new HdfsNameNodeExporter(
                _contentProvider.Object,
                _prometheusUtils.Object,
                _configurationOptions.Object,
                _logger.Object);
        }

        [Fact]
        public void Should_Run_Successfully()
        {
            var content = File.ReadAllText("Jsons/HdfsNameNodeResponse.json");
            _contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.FromResult(content));

            Func<Task> func = async () => await _exporter.ExportMetricsAsync();

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
