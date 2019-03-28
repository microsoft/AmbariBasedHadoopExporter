// <copyright file="AmbariUriAttribute.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Utils
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// User defined data annotation that checks if the Uri is valid and its path is similar
    /// to the Ambari API path.
    /// </summary>
    public class AmbariUriAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            return Uri.TryCreate(value as string, UriKind.Absolute, out Uri uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps)
                   && uriResult.AbsolutePath.Equals("/api/v1/clusters");
        }
    }
}
