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

namespace Bangumi.Client.Authentication
{
    public static class SessionManager
    {
        private static readonly Uri logOnUri = new Uri(Uris.RootUri, "FollowTheRabbit");

        public static IAsyncOperation<ImageSource> GetCaptchaAsync()
        {
            return AsyncInfo.Run<ImageSource>(async token =>
            {
                if (getCookieValue(CookieNames.SessionID) == null)
                    await MyHttpClient.GetAsync(logOnUri);
                var uri = new Uri(Uris.RootUri, $"signup/captcha?{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{new Random().Next(1, 7)}");
                using (var stream = (await MyHttpClient.GetBufferAsync(uri)).AsStream().AsRandomAccessStream())
                {
                    await DispatcherHelper.Yield();
                    var image = new BitmapImage();
                    await image.SetSourceAsync(stream);
                    return image;
                }
            });
        }

        public static IAsyncAction LogOnAsync(string email, string password, string captchaResponse)
        {
            if (!IsGuest)
                throw new InvalidOperationException("您已经登录，请先退出登录后继续操作。");
            return AsyncInfo.Run(async token =>
            {
                var r = await MyHttpClient.PostDocumentAsync(logOnUri, getData());
                if (IsGuest)
                {
                    var text = r.GetElementbyId("colunmNotice").SelectSingleNode("descendant::*[@class='text']");
                    var result = HtmlEntity.DeEntitize(text.InnerText);
                    throw new InvalidOperationException(result);
                }
                IEnumerable<KeyValuePair<string, string>> getData()
                {
                    yield return new KeyValuePair<string, string>("formhash", "d36bd6b0");
                    yield return new KeyValuePair<string, string>("email", email);
                    yield return new KeyValuePair<string, string>("password", password);
                    yield return new KeyValuePair<string, string>("captcha_challenge_field", captchaResponse);
                    yield return new KeyValuePair<string, string>("loginsubmit", "登录");
                }
            });
        }

        public static bool IsGuest => string.IsNullOrEmpty(getCookieValue(CookieNames.Authentication));

        private static string getCookieValue(string cookieName)
        {
            return GetCookies().FirstOrDefault(c => c.Name == cookieName)?.Value;
        }

        public static IList<HttpCookie> GetCookies()
        {
            return MyHttpClient.CookieManager.GetCookies(Uris.RootUri).ToList();
        }

        public static void Clear()
        {
            foreach (var item in GetCookies())
            {
                MyHttpClient.CookieManager.DeleteCookie(item);
            }
        }
    }
}
