// <copyright file="ServiceExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Extensions
{
    using Core.Configurations;
    using Core.Configurations.Exporters;
    using Core.Exporters.Abstract;
    using Core.Exporters.Concrete;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extenstion methods for <see cref="IServiceCollection"/>, adding required configuration and services for
    /// the current project.
    /// </summary>
    public static class ServiceExtensions
    {
        public static void AddCoreConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var baseConfiguration = configuration.GetSection("BaseExporterConfiguration");
            services.Configure<YarnResourceManagerExporterConfiguration>(baseConfiguration);
            services.Configure<YarnNodeManagerExporterConfiguration>(baseConfiguration);
            services.Configure<HdfsDataNodeExporterConfiguration>(baseConfiguration);
            services.Configure<HdfsNameNodeExporterConfiguration>(baseConfiguration);
            services.Configure<ClusterExporterConfiguration>(baseConfiguration);

            services.AddSingleton<IValidatableConfiguration>(resolver => resolver.GetRequiredService<IOptions<YarnResourceManagerExporterConfiguration>>().Value);
            services.AddSingleton<IValidatableConfiguration>(resolver => resolver.GetRequiredService<IOptions<YarnNodeManagerExporterConfiguration>>().Value);
            services.AddSingleton<IValidatableConfiguration>(resolver => resolver.GetRequiredService<IOptions<HdfsDataNodeExporterConfiguration>>().Value);
            services.AddSingleton<IValidatableConfiguration>(resolver => resolver.GetRequiredService<IOptions<HdfsNameNodeExporterConfiguration>>().Value);
            services.AddSingleton<IValidatableConfiguration>(resolver => resolver.GetRequiredService<IOptions<ClusterExporterConfiguration>>().Value);
        }

        public static void AddCoreServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IExporter, YarnResourceManagerExporter>();
            services.AddSingleton<IExporter, YarnNodeManagerExporter>();
            services.AddSingleton<IExporter, HdfsDataNodeExporter>();
            services.AddSingleton<IExporter, HdfsNameNodeExporter>();
            services.AddSingleton<IExporter, ClusterExporter>();
        }
    }
}
