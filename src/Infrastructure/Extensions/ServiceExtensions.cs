// <copyright file="ServiceExtensions.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Extensions
{
    using Core.Configurations;
    using Core.Providers;
    using Core.Utils;
    using Infrastructure.Configuration;
    using Infrastructure.Providers.Concrete;
    using Infrastructure.Utils;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Extenstion methods for <see cref="IServiceCollection"/>, adding required configuration and services for
    /// the current project.
    /// </summary>
    public static class ServiceExtensions
    {
        public static void AddInfrastructureConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AmbariClientConfiguration>(configuration.GetSection("AmbariConnection"));

            services.AddSingleton<IValidatableConfiguration>(resolver => resolver.GetRequiredService<IOptions<AmbariClientConfiguration>>().Value);
        }

        public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IPrometheusUtils, PrometheusUtils>();
            services.AddSingleton<IContentProvider, AmbariApiContentProvider>();
        }
    }
}
