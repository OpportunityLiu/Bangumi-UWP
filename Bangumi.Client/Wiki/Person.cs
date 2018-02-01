using Bangumi.Client.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.Client.Wiki
{
    public sealed class Person : Mono
    {
        public Person(long id) : base(id) { }

        public override Uri Uri => new Uri(Uris.RootUri, $"/person/{Id}");
    }
}
