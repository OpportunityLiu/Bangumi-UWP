using System;
using Newtonsoft.Json;

namespace Bangumi.Client.Schema
{
    public readonly struct ImageUri
    {
        [JsonConstructor]
        internal ImageUri(Uri small, Uri medium, Uri large, Uri common, Uri grid)
        {
            this.Large = large;
            this.Medium = medium;
            this.Small = small;
            this.Common = common;
            this.Grid = grid;
        }

        [JsonProperty("large")]
        public Uri Large { get; }
        [JsonProperty("medium")]
        public Uri Medium { get; }
        [JsonProperty("small")]
        public Uri Small { get; }
        [JsonProperty("common")]
        public Uri Common { get; }
        [JsonProperty("grid")]
        public Uri Grid { get; }
    }
}
