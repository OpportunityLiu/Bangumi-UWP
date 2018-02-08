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
    public class Subject : Topic
    {
        public Subject(long id) : base(id)
        {
        }

        public override Uri Uri => new Uri(Config.RootUri, $"/subject/{Id}");

        private ObservableList<Tag> tags;
        public ObservableListView<Tag> Tags => this.tags?.AsReadOnly();

        protected override void Populate(HtmlDocument document)
        {
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
