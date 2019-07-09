// <copyright file="BaseExporterTests.cs" company="Microsoft">
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

    public class BaseExporterTests
    {
        [Fact]
        public void IllegalAruments_Should_Throw()
        {
            // Using YarnResourceManagerExporter as a placeholder for one of the implementing classes
            var contentProvider = new Mock<IContentProvider>();
            var prometheusUtils = new Mock<IPrometheusUtils>();
            var configurationOptions = new Mock<IOptions<YarnResourceManagerExporterConfiguration>>();
            var configuration = new Mock<YarnResourceManagerExporterConfiguration>();
            var logger = new Mock<ILogger<YarnResourceManagerExporter>>();
            contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.FromResult(string.Empty));
            configuration.Setup(f => f.UriEndpoint).Returns("cluster");
            configurationOptions.Setup(f => f.Value).Returns(configuration.Object);
            var exporter = new YarnResourceManagerExporter(
                contentProvider.Object,
                prometheusUtils.Object,
                configurationOptions.Object,
                logger.Object);

            // Empty string
            Func<Task> func = async () => { await exporter.ExportMetricsAsync(string.Empty); };
            func.Should().Throw<ArgumentException>();

            // Invalid string format
            func = async () => { await exporter.ExportMetricsAsync("/someString"); };
            func.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task Updating_Self_Metrics_Should_Update_Accordingly()
        {
            // Using YarnResourceManagerExporter as a placeholder for one of the implementing classes
            var contentProvider = new Mock<IContentProvider>();
            var prometheusUtils = new Mock<IPrometheusUtils>();
            var configurationOptions = new Mock<IOptions<YarnResourceManagerExporterConfiguration>>();
            var configuration = new Mock<YarnResourceManagerExporterConfiguration>();
            var logger = new Mock<ILogger<YarnResourceManagerExporter>>();
            var content = File.ReadAllText("Jsons/YarnResourceManagerResponse.json");
            contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(() =>
            {
                Task.Delay(1000).GetAwaiter().GetResult();
                return Task.FromResult(content);
            });
            configuration.Setup(f => f.UriEndpoint).Returns("cluster");
            configurationOptions.Setup(f => f.Value).Returns(configuration.Object);
            var exporter = new YarnResourceManagerExporter(
                contentProvider.Object,
                prometheusUtils.Object,
                configurationOptions.Object,
                logger.Object);

            var successfulScrapes = 0;
            var lastRunDuration = 0.0;
            prometheusUtils
                .Setup(f => f.ReportGauge(
                    It.IsAny<ConcurrentDictionary<string, Collector>>(),
                    It.IsIn("exporter_is_successful_scrape"),
                    It.IsIn(1.0),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<string>()))
                .Callback(() => { successfulScrapes++; });
            prometheusUtils
                .Setup(f => f.ReportGauge(
                    It.IsAny<ConcurrentDictionary<string, Collector>>(),
                    It.IsIn("exporter_scrape_time_seconds"),
                    It.IsAny<double>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<string>()))
                .Callback<ConcurrentDictionary<string, Collector>, string, double, Dictionary<string, string>,
                    string>(
                    (concurrentDict, metricName, metricValue, metricLabel, metricDesc) =>
                    {
                        lastRunDuration = metricValue;
                    });
            for (int i = 1; i <= 5; i++)
            {
                // Activating
                await exporter.ExportMetricsAsync();

                // Validating
                successfulScrapes.Should().Be(i);
                lastRunDuration.Should().BeGreaterThan(0);
            }

            // Validatin on bad behavior
            lastRunDuration = 0.0;
            contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.FromResult("invalid"));
            for (int i = 0; i < 5; i++)
            {
                Func<Task> func = async () => { await exporter.ExportMetricsAsync(); };
                func.Should().Throw<Exception>();
                successfulScrapes.Should().Be(5);
                lastRunDuration.Should().Be(0);
            }
        }
    }
}
