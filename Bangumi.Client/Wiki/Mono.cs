using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.Client.Wiki
{
    public abstract class Mono : Topic
    {
        protected Mono(long id) : base(id)
        {
        }
    }
}
