using System;

namespace Bangumi.Client.Internal
{
    internal static class Uris
    {
        public static Uri RootUri { get; } = new Uri("https://bgm.tv/");
        public static Uri ApiUri { get; } = new Uri("https://api.bgm.tv/");

        public static Uri CreateHttps(string uri)
        {
            if (uri == null)
                return null;
            if (uri.StartsWith("http://"))
                uri = uri.Insert(4, "s");
            return new Uri(uri);
        }
    }
}
