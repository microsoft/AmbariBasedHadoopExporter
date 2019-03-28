// <copyright file="FileSecretProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Providers.Concrete
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using Infrastructure.Providers.Abstract;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implementing the <see cref="ISecretProvider"/> interface, reading secrets from local file system.
    /// See README for further information about how to get your secrets into the local FS.
    ///
    /// Limitation - the maximum file size we can read which is 100Kb.
    /// </summary>
    internal class FileSecretProvider : ISecretProvider
    {
        private const int MaxFileSizeInBytes = 100000; // 100 Kb

        private readonly ILogger _logger;
        private readonly string _secretsFolderPath;
        private readonly IEnumerable<string> _secrets;

        public FileSecretProvider(
            string secretsFolderPath,
            IEnumerable<string> secrets,
            ILogger logger)
        {
            _logger = logger;
            _secrets = secrets;
            _secretsFolderPath = secretsFolderPath;
        }

        /// <summary>
        /// Reading all files only in the specified folder, not entering subfolers, and parse them as secrets.
        /// </summary>
        /// <inheritdoc/>
        public IReadOnlyDictionary<string, string> GetSecretNameToValueMap()
        {
            var map = new Dictionary<string, string>();
            using (_logger.BeginScope("Fetching secrets from folder"))
            {
                if (!Directory.Exists(_secretsFolderPath))
                {
                    _logger.LogCritical($"Directory {_secretsFolderPath} is missing");
                    throw new DirectoryNotFoundException("Secrets directory is missing");
                }

                var files = Directory.GetFiles(_secretsFolderPath, "*", SearchOption.TopDirectoryOnly);
                foreach (var file in files)
                {
                    var secretName = Path.GetFileName(file);
                    var secretValue = GetSecret(file);
                    map.Add(secretName, secretValue);
                }

                var missingSecrets = _secrets.Except(map.Keys).ToList();
                if (missingSecrets.Any())
                {
                    _logger.LogCritical(
                        $"Not all mapped secrets could be found [missing: {string.Join(",", missingSecrets)}]");
                    throw new Exception("Missing secrets");
                }

                return new ReadOnlyDictionary<string, string>(map);
            }
        }

        private static string GetSecret(string pathToSecretFile)
        {
            var fileInfo = new FileInfo(pathToSecretFile);
            if (fileInfo.Length > MaxFileSizeInBytes)
            {
                throw new FileLoadException(
                    $"File {pathToSecretFile} exceeded maximum allowed file size {MaxFileSizeInBytes}.");
            }

            return File.ReadAllText(pathToSecretFile);
        }
    }
}
