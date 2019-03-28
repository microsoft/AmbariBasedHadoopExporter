// <copyright file="IContentProvider.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Providers
{
    using System.Threading.Tasks;

    /// <summary>
    /// Responsible for serving the metrics content to the exporters.
    /// </summary>
    public interface IContentProvider
    {
        /// <summary>
        /// Invoking a HTTP request to the specified uri and returning its content.
        /// </summary>
        /// <param name="uriEndpoint">The URI endpoint.</param>
        /// <returns>Response content.</returns>
        Task<string> GetResponseContentAsync(string uriEndpoint);
    }
}
