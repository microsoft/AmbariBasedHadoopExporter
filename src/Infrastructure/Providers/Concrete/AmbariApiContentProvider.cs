// <copyright file="AmbariApiContentProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Infrastructure.Providers.Concrete
{
    using System;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Core.Providers;
    using Infrastructure.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Serving metrics content to the exporters by invoking Ambari API http requests.
    /// </summary>
    internal class AmbariApiContentProvider : IContentProvider
    {
        private readonly AmbariClientConfiguration _configuration;
        private readonly ILogger<AmbariApiContentProvider> _logger;
        private readonly HttpClient _httpClient;

        public AmbariApiContentProvider(IOptions<AmbariClientConfiguration> configuration, ILogger<AmbariApiContentProvider> logger)
        {
            _configuration = configuration.Value;
            _logger = logger;

            _httpClient = new HttpClient(new HttpClientHandler
            {
                Credentials = new NetworkCredential(_configuration.Username, _configuration.Password),
                PreAuthenticate = true,
                MaxConnectionsPerServer = 256,
            });
        }

        /// <inheritdoc/>
        public async Task<string> GetResponseContentAsync(string uriEndpoint)
        {
            if (uriEndpoint == null || uriEndpoint.Equals(string.Empty) ||
                !Uri.IsWellFormedUriString(uriEndpoint, UriKind.Absolute))
            {
                throw new ArgumentException($"Invalid argument was passed." +
                                            $"UriEndpoint - cannot be null/empty: {uriEndpoint}.");
            }

            try
            {
                using (var response = await _httpClient.GetAsync(uriEndpoint))
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        throw new HttpRequestException($"Status code: {response.StatusCode} isnt Ok.");
                    }

                    return await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Failed to get response from Ambari: {uriEndpoint}.");
                throw;
            }
        }
    }
}
