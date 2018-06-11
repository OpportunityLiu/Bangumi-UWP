using Newtonsoft.Json;
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

        public Person[] actors { get; set; }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => throw new System.NotImplementedException();
    }
}
