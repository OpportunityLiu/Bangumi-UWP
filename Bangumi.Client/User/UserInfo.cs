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
        public static IAsyncOperationWithProgress<UserInfo, HttpProgress> FetchAsync(int userId)
        {
            if (userId <= 0)
                throw new ArgumentException("UID 应为正整数");
            return FetchAsync(userId.ToString());
        }

        public static IAsyncOperationWithProgress<UserInfo, HttpProgress> FetchAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("用户名或 UID 不能为空");
            return MyHttpClient.GetJsonAsync<UserInfo>(new Uri(Config.ApiUri, $"/user/{userName.Trim()}"));
        }

        internal UserInfo() { }

        protected override void CheckResponse(string request, int code, string error)
        {
            if (error != null)
            {
                if (code == 404)
                    throw new ArgumentException("未找到指定的用户");
            }
            base.CheckResponse(request, code, error);
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


        public readonly struct AvatarUri
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
