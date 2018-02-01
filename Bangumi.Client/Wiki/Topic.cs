using Opportunity.MvvmUniverse;
using System;
using Bangumi.Client.Internal;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using System.Threading;

namespace Bangumi.Client.Wiki
{
    public abstract class Topic : ObservableObject
    {
        protected Topic(long id)
        {
            if (id <= 0)
                throw new ArgumentOutOfRangeException(nameof(id));
            this.Id = id;
        }

        public long Id { get; }

        public abstract Uri Uri { get; }

        private string name;
        public string Name { get => this.name; protected set => Set(ref this.name, value); }

        private string description;
        public string Description { get => this.description; protected set => Set(ref this.description, value); }

        private InfoCollection infoBox;
        public InfoCollection InfoBox => LazyInitializer.EnsureInitialized(ref this.infoBox);

        public async Task FetchDataAsync()
        {
            var doc = await MyHttpClient.GetDocumentAsync(this.Uri);
            Populate(doc);
        }

        protected virtual void Populate(HtmlDocument doc)
        {
            InfoBox.Populate(doc);
        }
    }
}
