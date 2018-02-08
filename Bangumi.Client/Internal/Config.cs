using System;

namespace Bangumi.Client.Internal
{
    internal static class Config
    {

        public static Uri RootUri { get; } = new Uri("https://bgm.tv/");
        public static Uri ApiUri { get; } = new Uri("https://api.bgm.tv/");

        internal const string ApiSourceParam = "source=onAir";
        internal static string ApiAuthParam => "auth=" + Uri.EscapeDataString(User.SessionManager.Current.Auth);

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
