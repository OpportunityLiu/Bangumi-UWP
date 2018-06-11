using System;
using Newtonsoft.Json;

namespace Bangumi.Client.Schema
{
    public sealed class ImageUri : IEquatable<ImageUri>
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

        public bool Equals(ImageUri other)
        {
            if (other is null)
                return false;
            return this.Large == other.Large
                && this.Medium == other.Medium
                && this.Small == other.Small
                && this.Common == other.Common
                && this.Grid == other.Grid;
        }

        public override bool Equals(object obj) => obj is ImageUri other && Equals(other);

        public override int GetHashCode() => (Large ?? Medium ?? Small ?? Common ?? Grid ?? (object)"").GetHashCode();
    }
}
