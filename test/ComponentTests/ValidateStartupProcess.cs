// <copyright file="ValidateStartupProcess.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace ComponentTests
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using App.Middlewares.Abstract;
    using App.Middlewares.Concrete;
    using App.Services;
    using Core.Configurations;
    using Core.Configurations.Exporters;
    using FluentAssertions;
    using Infrastructure.Configuration;
    using Infrastructure.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class ValidateStartupProcess
    {
        [Fact]
        public void Should_Validate_Startup_Successfully()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .AddEnvironmentVariables();

                    var configuration = configApp.Build();
                    var secretsConfigurationSection = configuration.GetSection("Secrets").Get<SecretsConfigurationSection>();
                    secretsConfigurationSection.Path =
                        Directory.GetCurrentDirectory() + "/" + secretsConfigurationSection.Path;
                    secretsConfigurationSection.Validate();
                    configApp.AddSecretProvider(secretsConfigurationSection);
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Adding configuration and services
                    services.AddAppConfigurations(hostContext.Configuration);
                    services.AddAppServices(hostContext.Configuration);
                })
                .UseConsoleLifetime()
                .Build();

            Func<Task> func = async () => { await host.ValidateConfigurationAsync(); };
            func.Should().NotThrow();
        }

        [Fact]
        public void Should_Fail_On_Validation()
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .AddEnvironmentVariables();
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Adding configuration and services
                    services.AddTransient<IValidator, ConfigurationsValidator>();
                    services.AddOptions();

                    var exporterConf = new ClusterExporterConfiguration()
                    {
                        ClusterName = string.Empty,
                    };
                    var mockOptions = new Mock<IOptions<ClusterExporterConfiguration>>();
                    mockOptions.Setup(f => f.Value).Returns(exporterConf);

                    services.AddSingleton<IValidatableConfiguration>(mockOptions.Object.Value);
                })
                .UseConsoleLifetime()
                .Build();

            Func<Task> func = async () => { await host.ValidateConfigurationAsync(); };
            func.Should().Throw<Exception>();
        }
    }
}
