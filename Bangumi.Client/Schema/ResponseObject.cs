using Newtonsoft.Json;
using Opportunity.MvvmUniverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Schema
{
    public abstract class ResponseObject : ObservableObject
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private string request;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private int code;
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate)]
        private string error;

        internal static JsonSerializerSettings JsonSettings { get; } =
            new JsonSerializerSettings
            {
                Converters =
                {
                    new Internal.UriJsonConverter(),
                },
            };

        public abstract IAsyncActionWithProgress<HttpProgress> PopulateAsync();

        protected virtual void CheckResponse(string request, int code, string error)
        {
            if (!string.IsNullOrWhiteSpace(error))
                throw new InvalidOperationException(error);
        }

        public static void Check(ResponseObject obj)
        {
            var req = obj.request;
            var code = obj.code;
            var error = obj.error;
            obj.request = null;
            obj.code = 0;
            obj.error = null;

            obj.CheckResponse(req, code, error);
        }
    }
}
