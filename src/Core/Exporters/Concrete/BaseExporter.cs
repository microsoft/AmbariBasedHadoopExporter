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
        }

        /// <inheritdoc />
        public ConcurrentDictionary<string, Collector> Collectors { get; protected set; }

        /// <inheritdoc />
        public async Task ExportMetricsAsync(string endpointUrlSuffix = null)
        {
            var fullEndpointUri = BaseConfiguration.UriEndpoint;
            if (endpointUrlSuffix != null)
            {
                // Valid suffix
                if (endpointUrlSuffix == string.Empty || endpointUrlSuffix.StartsWith("/"))
                {
                    throw new ArgumentException(
                        $"{nameof(ExportMetricsAsync)} recieved an invalid endpoint url suffix - {endpointUrlSuffix}.");
                }

                fullEndpointUri += $"/{endpointUrlSuffix}";
            }

            var content = string.Empty;
            var stopWatch = Stopwatch.StartNew();
            try
            {
                using (Logger.BeginScope(new Dictionary<string, object>() { { "Exporter", GetType().Name } }))
                {
                    Logger.LogInformation($"{nameof(ExportMetricsAsync)} Started.");

                    content = await ContentProvider.GetResponseContentAsync(fullEndpointUri);
                    var component = JsonConvert.DeserializeObject(content, ComponentType);
                    await ReportMetrics(component);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, $"{GetType().Name}.{nameof(ExportMetricsAsync)}: Failed to export metrics. Labels: {BaseConfiguration.DefaultLabels}, Content length: {content.Length}");
                throw;
            }
            finally
            {
                stopWatch.Stop();
                Logger.LogInformation($"Runtime: {stopWatch.Elapsed}.");
            }
        }

        /// <summary>
        /// Reporting metrics using the exporter component received from Ambari api.
        /// </summary>
        /// <param name="component">Component object</param>
        /// <returns>Task</returns>
        protected abstract Task ReportMetrics(object component);
    }
}
