using Opportunity.MvvmUniverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public string Name { get; }

        public string Description { get; }

        public InfoCollection InfoBox { get; }
    }
}
