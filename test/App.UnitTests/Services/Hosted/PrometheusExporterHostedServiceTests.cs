// <copyright file="PrometheusExporterHostedServiceTests.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.UnitTests.Services.Hosted
{
    using System;
    using System.Collections.Generic;
    using App.Configuration;
    using App.Services.Hosted;
    using Core.Exporters.Abstract;
    using FluentAssertions;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class PrometheusExporterHostedServiceTests
    {
        private readonly Mock<IEnumerable<IExporter>> _exporters;
        private readonly Mock<IOptions<PrometheusExporterConfiguration>> _configurationOptions;
        private readonly Mock<ILogger<PrometheusExporterHostedService>> _logger;
        private PrometheusExporterHostedService _prometheusExporterService;

        public PrometheusExporterHostedServiceTests()
        {
            _exporters = new Mock<IEnumerable<IExporter>>();

            var configuration = new PrometheusExporterConfiguration()
            {
                Port = 9561
            };
            _configurationOptions = new Mock<IOptions<PrometheusExporterConfiguration>>();
            _configurationOptions.Setup(f => f.Value).Returns(configuration);

            _logger = new Mock<ILogger<PrometheusExporterHostedService>>();

            _prometheusExporterService = new PrometheusExporterHostedService(_exporters.Object, _configurationOptions.Object, _logger.Object);
        }

        [Fact]
        public async void Invoking_Successful_And_Invalid_Scrapes_Should_Increase_Accordingly()
        {
            var validExporter = new Mock<IExporter>();
            var exporterEnumerator = new List<IExporter> { validExporter.Object };
            _exporters.Setup(f => f.GetEnumerator()).Returns(() => exporterEnumerator.GetEnumerator());

            for (int i = 0; i < 5; i++)
            {
                await _prometheusExporterService.RunExportersAsync();
                _prometheusExporterService.IsSuccessful.Value.Should().Be(1);
            }

            // Validating metrics
            _prometheusExporterService.ScrapeTime.Should().NotBeNull();
            _prometheusExporterService.TotalScrapeActivations.Value.Should().Be(5);
            _prometheusExporterService.TotalSuccessfulScrapeActivations.Value.Should().Be(5);

            // Adding invalid exporter
            var invalidExporter = new Mock<IExporter>();
            invalidExporter.Setup(f => f.ExportMetricsAsync()).Throws(new Exception("Test exception"));
            exporterEnumerator.Add(invalidExporter.Object);
            for (int i = 0; i < 5; i++)
            {
                await _prometheusExporterService.RunExportersAsync();
                _prometheusExporterService.IsSuccessful.Value.Should().Be(0);
            }

            // Validating metrics
            _prometheusExporterService.ScrapeTime.Should().NotBeNull();
            _prometheusExporterService.TotalScrapeActivations.Value.Should().Be(10);
            _prometheusExporterService.TotalSuccessfulScrapeActivations.Value.Should().Be(5);

            // Adding invalid aggregated exception
            var invalidAggregatedExporter = new Mock<IExporter>();
            invalidAggregatedExporter.Setup(f => f.ExportMetricsAsync()).Throws(new AggregateException("Test exception"));
            exporterEnumerator.Add(invalidAggregatedExporter.Object);
            for (int i = 0; i < 5; i++)
            {
                await _prometheusExporterService.RunExportersAsync();
                _prometheusExporterService.IsSuccessful.Value.Should().Be(0);
            }

            // Validating metrics
            _prometheusExporterService.ScrapeTime.Should().NotBeNull();
            _prometheusExporterService.TotalScrapeActivations.Value.Should().Be(15);
            _prometheusExporterService.TotalSuccessfulScrapeActivations.Value.Should().Be(5);

            // Removing valid exporter
            exporterEnumerator.Remove(validExporter.Object);
            for (int i = 0; i < 5; i++)
            {
                await _prometheusExporterService.RunExportersAsync();
                _prometheusExporterService.IsSuccessful.Value.Should().Be(0);
            }

            // Validating metrics
            _prometheusExporterService.ScrapeTime.Should().NotBeNull();
            _prometheusExporterService.TotalScrapeActivations.Value.Should().Be(20);
            _prometheusExporterService.TotalSuccessfulScrapeActivations.Value.Should().Be(5);
        }
    }
}
