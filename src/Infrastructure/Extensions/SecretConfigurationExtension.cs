// <copyright file="SecretConfigurationExtension.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Infrastructure.Configuration;
    using Infrastructure.Providers.Concrete;
    using Microsoft.Extensions.Configuration;

    /// <summary>
    /// Extension methods for <see cref="IConfigurationBuilder"/> that will add secrets to it.
    /// </summary>
    public static class SecretConfigurationExtension
    {
        /// <summary>
        /// Adding secrets' mapping as a configuration to the configuration builder.
        ///
        /// The structure of the map should be as follows:
        ///     "Existing file name in the current directory": "Configuration_Section_Name--Configuration_Entry"
        ///
        /// The result will be similar to the following structure:
        ///     "Configuration_Section_Name": {
        ///         "Configuration_Entry": "Value loaded from the file",
        ///         ...
        ///     }
        /// </summary>
        /// <param name="configurationBuilder">Context configuration builder.</param>
        /// <param name="secretsConfigurationSection">The configuration section of the secrets.</param>
        public static void AddSecretProvider(
            this IConfigurationBuilder configurationBuilder,
            SecretsConfigurationSection secretsConfigurationSection)
        {
            if (secretsConfigurationSection?.Mapping == null ||
                secretsConfigurationSection.Mapping.Count == 0)
            {
                throw new ArgumentException($"Invalid arguments were passes." +
                                            $"SecretsConfigurationSection - cannot be null: {secretsConfigurationSection}." +
                                            $"SecretsConfigurationSection.Mapping - cannot be null or empty: {secretsConfigurationSection?.Mapping}.");
            }

            var operationName = $"{nameof(SecretConfigurationExtension)}::{nameof(AddSecretProvider)} -";
            var secrets = secretsConfigurationSection.Mapping.Keys.Cast<string>();

            var secretNameToValueMap = new FileSecretProvider(secretsConfigurationSection.Path, secrets).GetSecretNameToValueMap();
            Console.WriteLine($"{operationName} resolved {secretNameToValueMap.Count} secrets.");

            // Creating a map that will represent the configuration.
            var configToSecretValueMap = new Dictionary<string, string>();
            foreach (var secretName in secretNameToValueMap.Keys)
            {
                var map = MapConfigToSecretValueWithSecretAsKey(secretName, secretsConfigurationSection.Mapping, secretNameToValueMap);
                foreach (var mapEntry in map)
                {
                    Console.WriteLine($"{operationName} mapped the following: '{secretName}' -> '{mapEntry.Key}'.");
                    configToSecretValueMap.Add(mapEntry.Key, mapEntry.Value);
                }
            }

            configurationBuilder.AddInMemoryCollection(configToSecretValueMap);
        }

        private static Dictionary<string, string> MapConfigToSecretValueWithSecretAsKey(
            string secretName,
            IReadOnlyDictionary<string, string> secretsMap,
            IReadOnlyDictionary<string, string> secretNameToValueMap)
        {
            var configPath = secretsMap.ContainsKey(secretName) ? secretsMap[secretName] : secretName;
            return new Dictionary<string, string>
            {
                { TransformConfigurationKey(configPath), secretNameToValueMap[secretName] },
            };
        }

        private static string TransformConfigurationKey(string rawConfigKey)
        {
            // To support section nesting, replace -- with :
            return rawConfigKey.Replace("--", ":");
        }
    }
}
