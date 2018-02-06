using Bangumi.Client.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.Client.Wiki
{
    public sealed class Character : Mono
    {
        public Character(long id) : base(id) { }

        public override Uri Uri => new Uri(Config.RootUri, $"/character/{Id}");
    }
}
