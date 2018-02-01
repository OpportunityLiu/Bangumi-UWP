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
    public sealed class UserInfo : ObservableObject
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
            var r = await MyHttpClient.GetJsonAsync<UserInfoData>(new Uri(Uris.ApiUri, $"/user/{userName.Trim()}"));
            r.Check();
            var u = new UserInfo();
            u.Populate(r, null);
            return u;
        }

        internal UserInfo() { }

        internal void Populate(UserInfoData data, string email)
        {
            if (data == null || data.id <= 0)
            {
                Email = default;
                Id = default;
                Uri = default;
                UserName = default;
                NickName = default;
                AvaterUri = default;
                Signature = default;
                Auth = default;
                OnPropertyChanged("");
                return;
            }
            Id = data.id;
            Email = email;
            Uri = Uris.CreateHttps(data.url);
            UserName = data.username;
            NickName = data.nickname;
            AvaterUri = Uris.CreateHttps(data.avatar.large);
            Signature = data.sign;
            Auth = data.auth;
            OnPropertyChanged("");
        }

        public int Id { get; private set; }
        public Uri Uri { get; private set; }
        public string UserName { get; private set; }
        public string NickName { get; private set; }
        public Uri AvaterUri { get; private set; }
        public string Signature { get; private set; }
        internal string Email { get; private set; }
        internal string Auth { get; private set; }

#pragma warning disable IDE1006 // 命名样式
#pragma warning disable CS0649 
        internal sealed class UserInfoData : JsonResponse
        {
            public int id;
            public string url;
            public string username;
            public string nickname;
            public Avatar avatar;
            public string sign;
            public string auth;
            public string auth_encode;


            public override void Check()
            {
                if (this.error != null)
                {
                    if (this.error == "Unauthorized")
                        throw new ArgumentException("用户不存在或密码错误");
                    else if (this.error.StartsWith("40102 "))
                        throw new ArgumentException("为保证账户安全，当前仅支持使用 Email 方式登录，请返回重试");
                    else if (this.code == 404)
                        throw new ArgumentException("未找到指定的用户");
                }
                base.Check();
            }

            internal sealed class Avatar
            {
                public string large;
                public string medium;
                public string small;
            }
        }
#pragma warning restore CS0649 
#pragma warning restore IDE1006 // 命名样式
    }

}
