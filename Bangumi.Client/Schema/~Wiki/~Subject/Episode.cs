using System;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Schema
{
    public class Episode : ResponseObject
    {
        public int id { get; set; }
        public string url { get; set; }
        public int type { get; set; }
        public int sort { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public string duration { get; set; }
        public string airdate { get; set; }
        public int comment { get; set; }
        public string desc { get; set; }
        public string status { get; set; }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => throw new NotImplementedException();
    }
}
