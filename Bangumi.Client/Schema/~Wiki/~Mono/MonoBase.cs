using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Web.Http;
using System;
using System.Diagnostics;

namespace Bangumi.Client.Schema
{
    public abstract class MonoBase : WikiBase
    {
        [JsonConstructor]
        protected MonoBase(long id) : base(id) { }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonProperty]
        private int comment;
        [JsonIgnore]
        public int Comment { get => this.comment; set => Set(ref this.comment, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonProperty]
        private int collects;
        [JsonIgnore]
        public int Collects { get => this.collects; set => Set(ref this.collects, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonProperty]
        private MonoInfo info;
        [JsonIgnore]
        public MonoInfo Info { get => this.info; set => Set(ref this.info, value); }
    }
}
