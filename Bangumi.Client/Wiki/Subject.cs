using Opportunity.MvvmUniverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bangumi.Client.Internal;
using HtmlAgilityPack;
using Opportunity.MvvmUniverse.Collections;

namespace Bangumi.Client.Wiki
{
    public enum SubjectType
    {
        Unknown = 0,
        Book = 1,
        Anime = 2,
        Music = 3,
        Game = 4,
        Real = 6,
    }

    public class Subject : Topic
    {
        public Subject(long id) : base(id)
        {
        }

        public override Uri Uri => new Uri(Config.RootUri, $"/subject/{Id}");

        private ObservableList<Tag> tags;
        public ObservableListView<Tag> Tags => this.tags?.AsReadOnly();

        private SubjectType type;
        public SubjectType Type { get => this.type; set => Set(ref this.type, value); }

        protected override void Populate(HtmlDocument document)
        {
            var searchNode = document.GetElementbyId("siteSearchSelect").ChildNodes.FirstOrDefault(i => i.GetAttribute("selected", false));
            if (searchNode != null)
                this.Type = (SubjectType)searchNode.GetAttribute("value", 0);
            var nameNode = document.GetElementbyId("headerSubject")?.Element("h1")?.Element("a");
            if (nameNode != null)
            {
                this.Name = nameNode.GetInnerText();
                this.NameCN = nameNode.GetAttribute("title", "");
            }
            var despNode = document.GetElementbyId("subject_summary");
            if (despNode != null)
            {
                this.Description = despNode.GetInnerText();
            }
            var tagsNodes = document.GetElementbyId("subject_detail")
                ?.Element("div", "subject_tag_section")
                ?.Element("div", "inner")
                ?.Elements("a");
            if (tagsNodes != null)
            {
                if (this.tags == null)
                {
                    this.tags = new ObservableList<Tag>();
                    OnPropertyChanged(nameof(Tags));
                }
                this.tags.Update(tagsNodes.Select(n => Tag.Create(n)).ToList());
            }
            base.Populate(document);
        }
    }
}
