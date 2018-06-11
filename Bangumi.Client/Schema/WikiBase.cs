using Opportunity.MvvmUniverse;
using System;
using Bangumi.Client.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Threading;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Bangumi.Client.Schema
{
    [DebuggerDisplay(@"Id = {Id} Name = {Name}")]
    public abstract class WikiBase : ResponseObject, IEquatable<WikiBase>
    {
        [JsonConstructor]
        protected WikiBase(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private long id;
        [JsonProperty("id")]
        public long Id { get => this.id; protected set => Set(ref this.id, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Uri uri;
        [JsonProperty("url")]
        public Uri Uri { get => this.uri; protected set => Set(ref this.uri, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string name;
        [JsonProperty("name")]
        public string Name { get => this.name; protected set => Set(ref this.name, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string nameCN;
        [JsonProperty("name_cn")]
        public string NameCN { get => this.nameCN; protected set => Set(ref this.nameCN, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ImageUri images;
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public ImageUri Images { get => this.images; set => Set(ref this.images, value); }

        public bool Equals(WikiBase other)
        {
            if (other is null)
                return false;
            if (this.GetType() != other.GetType())
                return false;
            return this.id == other.id;
        }

        public override int GetHashCode() => GetType().GetHashCode() ^ this.id.GetHashCode();

        public override bool Equals(object obj) => obj is WikiBase other && Equals(other);
    }
}
