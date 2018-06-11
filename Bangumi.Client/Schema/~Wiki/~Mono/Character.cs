using Newtonsoft.Json;
using Opportunity.MvvmUniverse.Collections;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Schema
{
    public class Character : MonoBase
    {
        [JsonConstructor]
        public Character(long id) : base(id)
        {
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonProperty]
        private readonly ObservableList<Person> actors = new ObservableList<Person>();
        [JsonIgnore]
        public ObservableListView<Person> Actors => this.actors.AsReadOnly();

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => throw new System.NotImplementedException();
    }
}
