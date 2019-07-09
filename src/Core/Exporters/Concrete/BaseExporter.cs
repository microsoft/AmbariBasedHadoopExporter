// <copyright file="BaseExporter.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Exporters.Concrete
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using Core.Configurations.Exporters;
    using Core.Exporters.Abstract;
    using Core.Extensions;
    using Core.Providers;
    using Core.Utils;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Prometheus;

    public abstract class BaseExporter : IExporter
    {
        protected readonly IContentProvider ContentProvider;
        protected readonly IPrometheusUtils PrometheusUtils;
        protected readonly BaseExporterConfiguration BaseConfiguration;
        protected readonly ILogger Logger;
        protected readonly Type ComponentType;

        private readonly Dictionary<string, string> _exporterMetricsLabels;

        protected BaseExporter(
            IContentProvider contentProvider,
            IPrometheusUtils prometheusUtils,
            BaseExporterConfiguration baseConfiguration,
            Type componentType,
            ILogger logger)
        {
            ContentProvider = contentProvider;
            PrometheusUtils = prometheusUtils;
            BaseConfiguration = baseConfiguration;
            ComponentType = componentType;
            Logger = logger;
            Collectors = new ConcurrentDictionary<string, Collector>();

            _exporterMetricsLabels = new Dictionary<string, string>()
            {
                { "Exporter", $"{GetType().Name}" }
            };
            _exporterMetricsLabels.TryAdd(BaseConfiguration.DefaultLabels);
        }

        /// <inheritdoc />
        public ConcurrentDictionary<string, Collector> Collectors { get; protected set; }

        /// <inheritdoc />
        public async Task ExportMetricsAsync(string endpointUrlSuffix = null)
        {
            // Getting the full endpoint URL
            var fullEndpointUrl = GetFullEndpointUrl(endpointUrlSuffix);

            // Invoking the request and sending metrics
            var content = string.Empty;
            var stopWatch = Stopwatch.StartNew();
            var successfullRun = 1;
            try
            {
                using (Logger.BeginScope(new Dictionary<string, object>() { { "Exporter", GetType().Name } }))
                {
                    Logger.LogInformation($"{nameof(ExportMetricsAsync)} Started.");

                  content = await ContentProvider.GetResponseContentAsync(fullEndpointUrl);
                    var component = JsonConvert.DeserializeObject(content, ComponentType);
                    await ReportMetrics(component);
                }
            }
            catch (Exception e)
            {
                successfullRun = 0;
                Logger.LogError(e, $"{GetType().Name}.{nameof(ExportMetricsAsync)}: Failed to export metrics. Labels: {BaseConfiguration.DefaultLabels}, Content length: {content.Length}");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                Logger.LogInformation($"{GetType().Name}.{nameof(ExportMetricsAsync)} ran for: {stopWatch.Elapsed}.");
                PrometheusUtils.ReportGauge(
                    Collectors,
                    "exporter_is_successful_scrape",
                    successfullRun,
                    _exporterMetricsLabels,
                    "Indication to if the last scrape was successful");
            }

            // Sending scrape time only on successful operations
            PrometheusUtils.ReportGauge(
                Collectors,
                "exporter_scrape_time_seconds",
                stopWatch.ElapsedMilliseconds / 1000.0,
                _exporterMetricsLabels,
                "Total scraping time of a specific exporter component");
        }

        /// <summary>
        /// Helper method used to build the full url that will be used in the http request.
        /// </summary>
        /// <param name="endpointUrlSuffix">Suffix of the url, must not start with '/'</param>
        /// <returns>Full URL</returns>
        internal string GetFullEndpointUrl(string endpointUrlSuffix)
        {
            // Validating suffix
            var fullEndpointUrl = BaseConfiguration.UriEndpoint;
            if (endpointUrlSuffix != null)
            {
                // Valid suffix
                if (endpointUrlSuffix == string.Empty || endpointUrlSuffix.StartsWith("/"))
                {
                    throw new ArgumentException(
                        $"{nameof(ExportMetricsAsync)} recieved an invalid endpoint url suffix - {endpointUrlSuffix}.");
                }

                fullEndpointUrl += $"/{endpointUrlSuffix}";
            }

            return fullEndpointUrl;
        }

        /// <summary>
        /// Reporting metrics using the exporter component received from Ambari api.
        /// </summary>
        /// <param name="component">Component object</param>
        /// <returns>Task</returns>
        protected abstract Task ReportMetrics(object component);
    }
}
