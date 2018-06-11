using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Web.Http;
using System;

namespace Bangumi.Client.Schema
{
    public abstract class MonoBase : WikiBase
    {
        [JsonConstructor]
        protected MonoBase(long id) : base(id) { }

        public string role_name { get; set; }
        public int comment { get; set; }
        public int collects { get; set; }
        public MonoInfo info { get; set; }
    }
}
