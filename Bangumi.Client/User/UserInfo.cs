using Bangumi.Client.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opportunity.MvvmUniverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.User
{
    public sealed class UserInfo : ResponseObject
    {
        public static Task<UserInfo> FetchAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UID 应为正整数");
            return FetchAsync(userId.ToString());
        }

        public static async Task<UserInfo> FetchAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("用户名或 UID 不能为空");
            return await MyHttpClient.GetJsonAsync<UserInfo>(new Uri(Uris.ApiUri, $"/user/{userName.Trim()}"));
        }

        internal UserInfo() { }

        protected override void CheckResponse(string request, int code, string error)
        {
            if (error != null)
            {
                if (error == "Unauthorized")
                    throw new ArgumentException("用户不存在或密码错误");
                else if (error.StartsWith("40102 "))
                    throw new ArgumentException("为保证账户安全，当前仅支持使用 Email 方式登录，请返回重试");
                else if (code == 404)
                    throw new ArgumentException("未找到指定的用户");
            }
            base.CheckResponse(request, code, error);
            OnPropertyChanged("");
        }

        internal void Reset()
        {
            Id = default;
            Uri = default;
            UserName = default;
            NickName = default;
            Avater = default;
            Signature = default;
            Email = default;
            Auth = default;
            OnPropertyChanged("");
        }

        [JsonProperty("id")]
        public int Id { get; private set; }
        [JsonProperty("url")]
        public Uri Uri { get; private set; }
        [JsonProperty("username")]
        public string UserName { get; private set; }
        [JsonProperty("nickname")]
        public string NickName { get; private set; }
        [JsonProperty("avatar")]
        public AvatarUri Avater { get; private set; }
        [JsonProperty("sign")]
        public string Signature { get; private set; }

        [JsonProperty("email")]
        internal string Email { get; set; }
        [JsonProperty("auth")]
        internal string Auth { get; set; }


        public struct AvatarUri
        {
            [JsonConstructor]
            internal AvatarUri(Uri small, Uri medium, Uri large)
            {
                this.Large = large;
                this.Medium = medium;
                this.Small = small;
            }

            [JsonProperty("large")]
            public Uri Large { get; }
            [JsonProperty("medium")]
            public Uri Medium { get; }
            [JsonProperty("small")]
            public Uri Small { get; }
        }
    }

}
