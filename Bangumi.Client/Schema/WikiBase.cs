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

namespace Bangumi.Client.Schema
{
    public abstract class WikiBase : ResponseObject
    {
        protected WikiBase(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }

        public long Id { get; }

        private Uri uri;
        [JsonProperty("url")]
        public Uri Uri { get => this.uri; set => Set(ref this.uri, value); }

        private string name;
        [JsonProperty("name")]
        public string Name { get => this.name; protected set => Set(ref this.name, value); }

        private string nameCN;
        [JsonProperty("name_cn")]
        public string NameCN { get => this.nameCN; protected set => Set(ref this.nameCN, value); }

        private string summary;
        [JsonProperty("summary")]
        public string Summary { get => this.summary; protected set => Set(ref this.summary, value); }
    }
}
