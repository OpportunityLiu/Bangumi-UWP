using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Bangumi.Client.Internal;
using Opportunity.MvvmUniverse;
using Windows.Foundation;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Web.Http;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Opportunity.MvvmUniverse.Storage;
using Opportunity.Helpers.Universal.AsyncHelpers;

namespace Bangumi.Client.Auth
{
    public static class SessionManager
    {
        internal static Token Current => TokenStorage.Value;

        private static readonly StorageProperty<Token> TokenStorage = StorageProperty.Create(Windows.Storage.ApplicationDataLocality.Local, "Bangumi.Client/AuthToken", () => new Token(), null, serializer: JsonSerializer<Token>.Instance);

        public static bool NeedAuth => Current.AccessToken == null;

        public static IAsyncAction AuthAsync(IAuthInfo authInfo, Uri callbackUri)
        {
            return AsyncInfo.Run(async token =>
            {
                TokenStorage.Value = await Token.FetchAsync(authInfo, callbackUri);
            });
        }

        public static IAsyncAction RefreshAsync(IAuthInfo authInfo)
        {
            if (NeedAuth)
                throw new InvalidOperationException("请先登录");
            return AsyncInfo.Run(async token =>
            {
                await Current.RefershAsync(authInfo);
                TokenStorage.Flush();
            });
        }

        public static void Clear()
        {
            TokenStorage.Value = new Token();
            MyHttpClient.SetAuthorization(null);
        }
    }
}
