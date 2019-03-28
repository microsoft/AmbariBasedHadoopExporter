// <copyright file="ServiceExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App.Services
{
    using App.Configuration;
    using App.Middlewares.Abstract;
    using App.Middlewares.Concrete;
    using App.Services.Hosted;
    using Core.Configurations;
    using Core.Extensions;
    using Infrastructure.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extenstion methods for IServiceCollection, adding required configuration and services for
    /// the current project.
    /// </summary>
    public static class ServiceExtensions
    {
        public static void AddAppConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IValidator, ConfigurationsValidator>();
            services.AddOptions();

            services.Configure<PrometheusExporterConfiguration>(configuration.GetSection("PrometheusExporterConfiguration"));
            services.AddSingleton<IValidatableConfiguration>(resolver =>
                resolver.GetRequiredService<IOptions<PrometheusExporterConfiguration>>().Value);

            services.AddCoreConfiguration(configuration);
            services.AddInfrastructureConfiguration(configuration);
        }

        public static void AddAppServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHostedService<PrometheusExporterHostedService>();

            services.AddCoreServices(configuration);
            services.AddInfrastructureServices(configuration);
        }
    }
}
