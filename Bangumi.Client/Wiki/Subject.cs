using Opportunity.MvvmUniverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bangumi.Client.Internal;

namespace Bangumi.Client.Wiki
{
    public class Subject : Topic
    {
        public Subject(long id) : base(id)
        {
        }

        public override Uri Uri => new Uri(Uris.RootUri, $"/subject/{Id}");
    }
}
