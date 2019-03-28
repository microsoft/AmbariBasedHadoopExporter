// <copyright file="YarnNodeManagerExporterTests.cs" company="Microsoft">
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

    public class YarnNodeManagerExporterTests
    {
        private readonly Mock<IContentProvider> _contentProvider;
        private readonly Mock<IPrometheusUtils> _prometheusUtils;
        private readonly Mock<IOptions<YarnNodeManagerExporterConfiguration>> _configurationOptions;
        private readonly Mock<YarnNodeManagerExporterConfiguration> _configuration;
        private readonly Mock<ILogger<YarnNodeManagerExporter>> _logger;
        private readonly YarnNodeManagerExporter _exporter;

        public YarnNodeManagerExporterTests()
        {
            _contentProvider = new Mock<IContentProvider>();
            _prometheusUtils = new Mock<IPrometheusUtils>();

            _configurationOptions = new Mock<IOptions<YarnNodeManagerExporterConfiguration>>();
            _configuration = new Mock<YarnNodeManagerExporterConfiguration>();
            _configuration.Setup(f => f.UriEndpoint).Returns("cluster");
            _configurationOptions.Setup(f => f.Value).Returns(_configuration.Object);

            _logger = new Mock<ILogger<YarnNodeManagerExporter>>();

            _exporter = new YarnNodeManagerExporter(
                _contentProvider.Object,
                _prometheusUtils.Object,
                _configurationOptions.Object,
                _logger.Object);
        }

        [Fact]
        public void Should_Run_Successfully()
        {
            var content = File.ReadAllText("Jsons/YarnNodeManagerResponse.json");
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
