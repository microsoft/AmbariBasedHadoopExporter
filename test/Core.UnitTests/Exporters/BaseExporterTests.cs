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
        /// <summary>
        /// Helper method that inits some YarnResourceManagerExporter
        /// </summary>
        /// <returns>Tuple containing relevant mocked objects</returns>
        public (
            BaseExporter exporter,
            Mock<IContentProvider> contentProvider,
            Mock<IPrometheusUtils> prometheusUtils,
            Mock<YarnResourceManagerExporterConfiguration> configuration)
            InitBaseExporterMocks()
        {
            var contentProvider = new Mock<IContentProvider>();
            var prometheusUtils = new Mock<IPrometheusUtils>();
            var configurationOptions = new Mock<IOptions<YarnResourceManagerExporterConfiguration>>();
            var configuration = new Mock<YarnResourceManagerExporterConfiguration>();
            var logger = new Mock<ILogger<YarnResourceManagerExporter>>();
            configuration.Setup(f => f.UriEndpoint).Returns("cluster");
            configurationOptions.Setup(f => f.Value).Returns(configuration.Object);
            var exporter = new YarnResourceManagerExporter(
                contentProvider.Object,
                prometheusUtils.Object,
                configurationOptions.Object,
                logger.Object);

            return (exporter, contentProvider, prometheusUtils, configuration);
        }

        [Fact]
        public void FullEndpointUrl_IllegalAruments_Should_Throw()
        {
            var mocks = InitBaseExporterMocks();

            // Empty string
            Func<string> func = () => { return mocks.exporter.GetFullEndpointUrl(string.Empty); };
            func.Should().Throw<ArgumentException>();

            // Invalid string format
            func = () => { return mocks.exporter.GetFullEndpointUrl("/someString"); };
            func.Should().Throw<ArgumentException>();
        }

        [Fact]
        public async Task Updating_Self_Metrics_Should_Update_Accordingly()
        {
            var mocks = InitBaseExporterMocks();

            // Mocking specific behavior
            var content = File.ReadAllText("Jsons/YarnResourceManagerResponse.json");
            mocks.contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(() =>
            {
                Task.Delay(1000).GetAwaiter().GetResult();
                return Task.FromResult(content);
            });

            var successfulScrapes = 0;
            var lastRunDuration = 0.0;
            mocks.prometheusUtils
                .Setup(f => f.ReportGauge(
                    It.IsAny<ConcurrentDictionary<string, Collector>>(),
                    It.IsIn("exporter_is_successful_scrape"),
                    It.IsIn(1.0),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<string>()))
                .Callback(() => { successfulScrapes++; });
            mocks.prometheusUtils
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
                await mocks.exporter.ExportMetricsAsync();

                // Validating
                successfulScrapes.Should().Be(i);
                lastRunDuration.Should().BeGreaterThan(0);
            }

            // Validatin on bad behavior
            lastRunDuration = 0.0;
            mocks.contentProvider.Setup(f => f.GetResponseContentAsync(It.IsAny<string>())).Returns(Task.FromResult("invalid"));
            for (int i = 0; i < 5; i++)
            {
                Func<Task> func = async () => { await mocks.exporter.ExportMetricsAsync(); };
                func.Should().Throw<Exception>();
                successfulScrapes.Should().Be(5);
                lastRunDuration.Should().Be(0);
            }
        }
    }
}
