using HtmlAgilityPack;
using System.Collections.Generic;
using System.Linq;
using Opportunity.MvvmUniverse.Collections;
using System;
using System.Diagnostics;
using Bangumi.Client.Internal;

namespace Bangumi.Client.Wiki
{
    public class InfoCollection : ObservableDictionary<string, IReadOnlyList<InfoRecord>>
    {
        internal void Populate(HtmlDocument doc)
        {
            var infoBox = doc.GetElementbyId("infobox");
            if (infoBox == null)
                return;
            var info = new Dictionary<string, List<InfoRecord>>();
            foreach (var line in infoBox.Elements("li"))
            {
                var key = HtmlEntity.DeEntitize(line.FirstChild.InnerText);
                if (key.EndsWith(": "))
                    key = key.Substring(0, key.Length - 2);
                if (!info.TryGetValue(key, out var list))
                {
                    list = new List<InfoRecord>();
                    info.Add(key, list);
                }
                foreach (var item in InfoRecord.Create(line))
                {
                    list.Add(item);
                }
            }
            this.Clear();
            foreach (var item in info)
            {
                this.Add(item.Key, item.Value);
            }
        }
    }

    [DebuggerDisplay(@"[{Text,nq}]-[{Title,nq}({Uri?.ToString(),nq})]")]
    public readonly struct InfoRecord
    {
        internal InfoRecord(HtmlNode node)
        {
            this.Text = HtmlEntity.DeEntitize(node.InnerText);
            this.Title = HtmlEntity.DeEntitize(node.GetAttributeValue("title", ""));
            var r = node.GetAttributeValue("href", null);
            if (r != null)
                Uri = new Uri(Config.RootUri, r);
            else
                Uri = null;
        }

        internal bool IsSeperator => string.IsNullOrEmpty(Title) && Uri == null && Text == "、";

        public string Title { get; }
        public string Text { get; }
        public Uri Uri { get; }

        public override string ToString() => Text;

        internal static IEnumerable<InfoRecord> Create(HtmlNode lineNode)
        {
            return lineNode.SelectNodes("node()[position()>1]").Select(n => new InfoRecord(n));
        }
    }
}