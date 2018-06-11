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
        private int comment;
        [JsonProperty("comment")]
        public int Comment { get => this.comment; private set => Set(ref this.comment, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int collects;
        [JsonProperty("collects")]
        public int Collects { get => this.collects; private set => Set(ref this.collects, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonProperty]
        private InfoBox info;
        [JsonIgnore]
        public InfoBox Info { get => this.info; set => Set(ref this.info, value); }
    }
}
