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

namespace Bangumi.Client.User
{
    public static class SessionManager
    {
        private static readonly Uri logOnUri = new Uri(Uris.RootUri, "FollowTheRabbit");

        private static bool firstCallGetCaptchaAsync = true;
        public static async Task<ImageSource> GetCaptchaAsync()
        {
            if (!IsGuest)
                throw new InvalidOperationException("您已经登录，请先退出登录后继续操作");
            if (GetCookieValue(CookieNames.SessionID) == null || firstCallGetCaptchaAsync)
            {
                LogOff();
                await MyHttpClient.GetAsync(logOnUri);
            }
            var uri = new Uri(Uris.RootUri, $"signup/captcha?{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}{new Random().Next(1, 7)}");
            using (var stream = (await MyHttpClient.GetBufferAsync(uri)).AsStream().AsRandomAccessStream())
            {
                await DispatcherHelper.Yield();
                firstCallGetCaptchaAsync = false;
                var image = new BitmapImage();
                await image.SetSourceAsync(stream);
                return image;
            }
        }

        public static async Task LogOnAsync(string email, string password, string captchaResponse)
        {
            if (!IsGuest)
                throw new InvalidOperationException("您已经登录，请先退出登录后继续操作");
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email 不能为空");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空");
            email = email.Trim();
            if (!email.Contains('@') || !email.Contains('.'))
                throw new ArgumentException("为保证账户安全，当前仅支持使用 Email 方式登录，请返回重试");
            SetCookieValue(CookieNames.Authentication, null);
            var r = await MyHttpClient.PostDocumentAsync(logOnUri, getForm(email, password, captchaResponse));
            if (string.IsNullOrEmpty(GetCookieValue(CookieNames.Authentication)))
            {
                var text = r.GetElementbyId("colunmNotice").SelectSingleNode("descendant::*[@class='text']");
                var result = HtmlEntity.DeEntitize(text.InnerText);
                throw new InvalidOperationException(result);
            }
            var rapi = await MyHttpClient.PostJsonAsync<UserInfo.UserInfoData>(new Uri("https://api.bgm.tv/auth?source=onAir"), getData(email, password));
            rapi.Check();
            Current.Populate(rapi, email);
            SetCookieValue(CookieNames.Authentication, GetCookieValue(CookieNames.Authentication));
            return;

            IEnumerable<KeyValuePair<string, string>> getForm(string e, string p, string c)
            {
                yield return new KeyValuePair<string, string>("formhash", "d36bd6b0");
                yield return new KeyValuePair<string, string>("email", e);
                yield return new KeyValuePair<string, string>("password", p);
                yield return new KeyValuePair<string, string>("captcha_challenge_field", c);
                yield return new KeyValuePair<string, string>("loginsubmit", "登录");
            };
            IEnumerable<KeyValuePair<string, string>> getData(string e, string p)
            {
                yield return new KeyValuePair<string, string>("source", "onAir");
                yield return new KeyValuePair<string, string>("username", e);
                yield return new KeyValuePair<string, string>("password", p);
                yield return new KeyValuePair<string, string>("auth", "0");
                yield return new KeyValuePair<string, string>("sysuid", "0");
                yield return new KeyValuePair<string, string>("sysusername", "0");
            };
        }

        public static bool IsGuest => Current.Id <= 0;

        internal static string GetCookieValue(string cookieName)
        {
            return GetCookies().FirstOrDefault(c => c.Name == cookieName)?.Value;
        }

        internal static void SetCookieValue(string cookieName, string cookieValue)
        {
            var cookie = new HttpCookie(cookieName, "bgm.tv", "/");
            if (cookieValue == null)
                MyHttpClient.CookieManager.DeleteCookie(cookie);
            else
            {
                cookie.Value = cookieValue;
                cookie.Expires = DateTimeOffset.Now.AddMonths(1);
                MyHttpClient.CookieManager.SetCookie(cookie);
            }
        }

        internal static IList<HttpCookie> GetCookies()
        {
            return MyHttpClient.CookieManager.GetCookies(Uris.RootUri).ToList();
        }

        public static void LogOff()
        {
            foreach (var item in GetCookies())
            {
                MyHttpClient.CookieManager.DeleteCookie(item);
            }
            Current.Populate(null, null);
            firstCallGetCaptchaAsync = true;
        }

        public static UserInfo Current { get; } = new UserInfo();
    }
}
