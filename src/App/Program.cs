// <copyright file="Program.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace App
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Threading.Tasks;
    using App.Logging;
    using App.Services;
    using Infrastructure.Configuration;
    using Infrastructure.Extensions;
    using Infrastructure.Utils;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Serilog;
    using Serilog.Events;

    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = new HostBuilder()
                .ConfigureAppConfiguration((hostContext, configApp) =>
                {
                    configApp.SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json", false, true)
                        .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("environment") ?? hostContext.HostingEnvironment.EnvironmentName}.json", true, true)
                        .AddEnvironmentVariables();

                    // Adding the secrets to our configuration context
                    var configuration = configApp.Build();
                    var secretsConfigurationSection = configuration.GetSection("Secrets").Get<SecretsConfigurationSection>();
                    if (secretsConfigurationSection == null)
                    {
                        throw new Exception("Couldn't find configuration section - Secrets. Please validate your configuration.");
                    }

                    secretsConfigurationSection.Validate();
                    configApp.AddSecretProvider(secretsConfigurationSection);
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    var logLevelString = hostContext.Configuration.GetSection("LogLevel").Value;
                    if (Enum.TryParse(logLevelString, out LogEventLevel serilogMinimumLevel))
                    {
                        Trace.TraceWarning($"Unable to resolve LogLevel: {logLevelString}, using Debug as default.");
                        serilogMinimumLevel = LogEventLevel.Debug;
                    }

                    loggerConfiguration.MinimumLevel.Is(serilogMinimumLevel).Enrich.FromLogContext().WriteTo.Console(new LogFormatter());
                })
                .ConfigureServices((hostContext, services) =>
                {
                    // Adding configuration and services
                    services.AddAppConfigurations(hostContext.Configuration);
                    services.AddAppServices(hostContext.Configuration);
                })
                .UseConsoleLifetime()
                .Build();

            await host.ValidateConfigurationAsync();

            await host.RunAsync();
        }
    }
}
