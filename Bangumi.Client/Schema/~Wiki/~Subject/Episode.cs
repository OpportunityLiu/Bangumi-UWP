using System;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Schema
{
    public class Episode : WikiBase
    {
        public Episode(long id) : base(id)
        {
        }

        public int type { get; set; }
        public int sort { get; set; }
        public string duration { get; set; }
        public string airdate { get; set; }
        public int comment { get; set; }
        public string desc { get; set; }
        public string status { get; set; }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => throw new NotImplementedException();
    }
}
