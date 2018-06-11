using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Schema
{
    public class Person : MonoBase
    {
        [JsonConstructor]
        public Person(long id) : base(id)
        {
        }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => throw new System.NotImplementedException();
    }
}
