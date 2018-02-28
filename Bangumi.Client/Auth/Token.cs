using Bangumi.Client.Internal;
using Bangumi.Client.Schema;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Auth
{
    internal class Token : ResponseObject
    {
        public static Uri TokenUri { get; } = new Uri("https://bgm.tv/oauth/access_token");

        public static IAsyncOperationWithProgress<Token, HttpProgress> FetchAsync(Uri callbackUri)
        {
            if (callbackUri == null)
                throw new ArgumentNullException(nameof(callbackUri));
            var query = callbackUri.Query.Split(new[] { '?', '&', '=' }, StringSplitOptions.RemoveEmptyEntries);
            if (query.Length == 2)
                return MyHttpClient.PostJsonAsync<Token>(TokenUri, getData("authorization_code", query[1], null, null));
            else if (query.Length == 4)
            {
                var ci = Array.IndexOf(query, "code") + 1;
                var si = Array.IndexOf(query, "state") + 1;
                if (ci > 0 && si > 0)
                    return MyHttpClient.PostJsonAsync<Token>(TokenUri, getData("authorization_code", query[ci], query[si], null));
            }
            throw new ArgumentException("回调参数错误");
        }

        private static IEnumerable<KeyValuePair<string, string>> getData(string grantType, string code, string state, string refreshToken)
        {
            var info = AuthManager.AuthInfo;
            if (info == null)
                throw new InvalidOperationException("Auth.SessionManager.AuthInfo has not set.");
            yield return new KeyValuePair<string, string>("grant_type", grantType);
            yield return new KeyValuePair<string, string>("client_id", info.GetAppId());
            yield return new KeyValuePair<string, string>("client_secret", info.GetAppSecret());
            yield return new KeyValuePair<string, string>("redirect_uri", info.GetRedirectUri().ToString());
            if (code != null)
                yield return new KeyValuePair<string, string>("code", code);
            if (state != null)
                yield return new KeyValuePair<string, string>("state", state);
            if (refreshToken != null)
                yield return new KeyValuePair<string, string>("refresh_token", refreshToken);
        }

        public IAsyncActionWithProgress<HttpProgress> RefershAsync()
        {
            return MyHttpClient.PostJsonAsync(TokenUri, getData("refresh_token", null, null, this.RefreshToken), this);
        }

        protected override void CheckResponse(string request, int code, string error)
        {
            base.CheckResponse(request, code, error);
            MyHttpClient.SetAuthorization(this);
            IsValid = true;
        }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => RefershAsync();

        public bool IsValid { get; private set; }

        [JsonProperty("user_id")]
        public int UserId { get; private set; } = -1;
        [JsonProperty("access_token")]
        public string AccessToken { get; private set; }
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; private set; }
        [JsonProperty("token_type")]
        public string TokenType { get; private set; }
        [JsonProperty("scope")]
        public string Scope { get; private set; }
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; private set; }
    }
}
