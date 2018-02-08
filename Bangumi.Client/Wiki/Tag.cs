using Bangumi.Client.Internal;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.Client.Wiki
{
    public readonly struct Tag : IEquatable<Tag>
    {
        internal static Tag Create(HtmlNode tagANode)
        {
            var href = new Uri(Config.RootUri, HtmlEntity.DeEntitize(tagANode.GetAttributeValue("href", "")));
            var v = HtmlEntity.DeEntitize(tagANode.FirstChild.InnerText);
            var n = int.Parse(tagANode.LastChild.InnerText);
            return new Tag(href, v, n);
        }

        public bool Equals(Tag other)
        {
            return this.Count == other.Count
                && this.Value == other.Value
                && this.Uri == other.Uri;
        }

        public override bool Equals(object obj)
        {
            if (obj is Tag t)
                return Equals(t);
            return false;
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode() ^ Count * 97 ^ Uri.GetHashCode() * 17;
        }

        private Tag(Uri uri, string value, int count)
        {
            this.Uri = uri;
            this.Value = value;
            this.Count = count;
        }

        public Uri Uri { get; }
        public string Value { get; }
        public int Count { get; }
    }
}
