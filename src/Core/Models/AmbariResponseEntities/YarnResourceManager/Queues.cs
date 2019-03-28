// <copyright file="Queues.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.YarnResourceManager
{
    using Newtonsoft.Json;

    public class Queues
    {
        [JsonProperty(PropertyName = "root")]
        public Queue Root { get; set; }
    }
}
