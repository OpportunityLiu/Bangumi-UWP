using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Web.Http;
using System;

namespace Bangumi.Client.Schema
{
    public abstract class MonoBase : ResponseObject
    {
        public int id { get; set; }
        public string url { get; set; }
        public string name { get; set; }

        private ImageUri images;
        [JsonProperty("images", NullValueHandling = NullValueHandling.Ignore)]
        public ImageUri Images { get => this.images; set => Set(ref this.images, value); }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => throw new System.NotImplementedException();
    }
}
