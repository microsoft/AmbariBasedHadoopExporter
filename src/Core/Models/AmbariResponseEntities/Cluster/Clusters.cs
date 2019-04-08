// <copyright file="Clusters.cs" company="Microsoft">
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
// </copyright>

namespace Core.Models.AmbariResponseEntities.Cluster
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// A representation of the basic Ambari API entry "/v1/clusters", returning a list of clusters
    /// managed by Ambari.
    /// </summary>
    public class Clusters
    {
        [JsonProperty("href")]
        public Uri Href { get; set; }

        [JsonProperty("items")]
        public List<Cluster> ClusterList { get; set; }

        /// <summary>
        /// Representing a simple Cluster object, used only by Clusters object.
        /// </summary>
        public class Cluster
        {
            [JsonProperty("href")]
            public Uri Href { get; set; }

            [JsonProperty("Clusters")]
            public ClusterInfo Info { get; set; }

            /// <summary>
            /// Representing a cluster's simple information.
            /// </summary>
            public class ClusterInfo
            {
                [JsonProperty("cluster_name")]
                public string Name { get; set; }

                [JsonProperty("version")]
                public string Version { get; set; }
            }
        }
    }
}
