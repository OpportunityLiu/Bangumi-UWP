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
using System.Diagnostics;
using Windows.Security.Authentication.Web;
using Windows.UI.Core;

namespace Bangumi.Client.Auth
{
    public static class AuthManager
    {
        internal static Token Current => TokenStorage.Value;

        private static readonly StorageProperty<Token> TokenStorage = StorageProperty.Create(Windows.Storage.ApplicationDataLocality.Local, "Bangumi.Client/AuthToken", () => new Token(), null, serializer: JsonSerializer<Token>.Instance);

        public static bool NeedAuth => !Current.IsValid;

        public static int UserId => NeedAuth ? -1 : Current.UserId;

        public static IAuthInfo AuthInfo { get; set; }

        private static IAsyncAction authCoreAsync()
        {
            return AsyncInfo.Run(async token =>
            {
                var state = Windows.Security.Cryptography.CryptographicBuffer.GenerateRandomNumber().ToString("X");
                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None,
                  new Uri($"https://bgm.tv/oauth/authorize?client_id={AuthInfo.GetAppId()}&response_type=code&state={state}"));
                switch (webAuthenticationResult.ResponseStatus)
                {
                case WebAuthenticationStatus.Success:
                    TokenStorage.Value = await Token.FetchAsync(new Uri(webAuthenticationResult.ResponseData));
                    break;
                case WebAuthenticationStatus.ErrorHttp:
                    throw new InvalidOperationException($"HTTP Error: {webAuthenticationResult.ResponseErrorDetail}");
                case WebAuthenticationStatus.UserCancel:
                    throw new OperationCanceledException(token);
                default:
                    throw new InvalidOperationException(webAuthenticationResult.ResponseData.ToString());
                }
            });
        }

        public static IAsyncAction AuthAsync()
        {
            if (AuthInfo == null)
                throw new InvalidOperationException("Auth.SessionManager.AuthInfo has not set.");
            if (DispatcherHelper.Dispatcher.HasThreadAccess)
                return authCoreAsync();
            else
                return DispatcherHelper.Dispatcher.RunAsync(authCoreAsync);
        }

        public static IAsyncAction RefreshAsync()
        {
            if (NeedAuth)
                throw new InvalidOperationException("请先登录");
            return AsyncInfo.Run(async token =>
            {
                await Current.RefershAsync();
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
