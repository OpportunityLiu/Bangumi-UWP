using Bangumi.Client.Internal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Opportunity.Helpers.ObjectModel;
using Opportunity.MvvmUniverse;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Schema
{
    [DebuggerDisplay(@"Id = {Id} Username = {Username}")]
    public sealed class User : ResponseObject
    {
        public static IAsyncOperationWithProgress<User, HttpProgress> FetchAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("UID 应为正整数");
            return FetchAsync(id.ToString());
        }

        public static IAsyncOperationWithProgress<User, HttpProgress> FetchAsync(string userName)
        {
            if (string.IsNullOrWhiteSpace(userName))
                throw new ArgumentException("用户名或 UID 不能为空");
            return MyHttpClient.GetJsonAsync<User>(new Uri(Config.ApiUri, $"/user/{userName.Trim()}"));
        }

        [JsonConstructor]
        public User(int id)
        {
            if (id <= 0)
                throw new ArgumentException("UID 应为正整数");
            this.Id = id;
        }

        protected override void CheckResponse(string request, int code, string error)
        {
            if (error != null)
            {
                if (code == 404)
                    throw new ArgumentException("未找到指定的用户");
            }
            base.CheckResponse(request, code, error);
        }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync()
            => MyHttpClient.GetJsonAsync(new Uri(Config.ApiUri, $"/user/{Id}"), this);

        [JsonProperty("id")]
        public int Id { get; }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Uri uri;
        [JsonProperty("url")]
        public Uri Uri { get => this.uri; set => Set(ref this.uri, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string username;
        [JsonProperty("username")]
        public string Username { get => this.username; set => Set(ref this.username, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string nickname;
        [JsonProperty("nickname")]
        public string Nickname { get => this.nickname; set => Set(ref this.nickname, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private ImageUri avatar;
        [JsonProperty("avatar")]
        public ImageUri Avatar { get => this.avatar; set => Set(ref this.avatar, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string signature;
        [JsonProperty("sign")]
        public string Signature { get => this.signature; set => Set(ref this.signature, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private UserGroup group;
        [JsonProperty("usergroup")]
        public UserGroup Group { get => this.group; set => Set(ref this.group, value); }
    }

    [DebuggerDisplay(@"Id = {Id} Username = {Username}")]
    public sealed class Collections : ResponseObject
    {

        [JsonConstructor]
        public Collections(int id)
        {
            if (id <= 0)
                throw new ArgumentException("UID 应为正整数");
            this.UserId = id;
        }

        [JsonIgnore]
        public int UserId { get; }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync()
        {
            return MyHttpClient.GetJsonAsync(new Uri(Config.ApiUri, $"/user/{UserId}/collections/book?app_id={Auth.AuthManager.AuthInfo.GetAppId()}"), new[] { this });
        }

        public CollectionStatus type { get; set; }
        public string name { get; set; }
        public string name_cn { get; set; }
        public Collect[] collects { get; set; }

        public class Collect
        {
            public Status status { get; set; }
            public int count { get; set; }
            public List[] list { get; set; }
        }

        public class Status
        {
            public int id { get; set; }
            public string type { get; set; }
            public string name { get; set; }
        }

        public class List
        {
            public Subject subject { get; set; }
        }
    }

    public enum CollectionStatus
    {
        Unknown = 0,
        /// <summary>1 = wish = 想做 </summary>
        Wish = 1,
        /// <summary>2 = collect = 做过 </summary>
        Collect = 2,
        /// <summary>3 = do = 在做 </summary>
        Do = 3,
        /// <summary>4 = on_hold = 搁置 </summary>
        OnHold = 4,
        /// <summary>5 = dropped = 抛弃</summary>
        Dropped = 5,
    }

    public enum UserGroup
    {
        Unknown = 0,
        /// <summary>
        /// 管理员
        /// </summary>
        Administrator = 1,
        /// <summary>
        /// Bangumi 管理猿
        /// </summary>
        BangumiAdministrator = 2,
        /// <summary>
        /// 天窗管理猿
        /// </summary>
        DoujinAdministrator = 3,
        /// <summary>
        /// 禁言用户
        /// </summary>
        BannedUser = 4,
        /// <summary>
        /// 禁止访问用户
        /// </summary>
        ForbiddenUser = 5,
        /// <summary>
        /// 人物管理猿
        /// </summary>
        MonoAdministrator = 8,
        /// <summary>
        /// 维基条目管理猿
        /// </summary>
        WikiAdministrator = 9,
        /// <summary>
        /// 用户
        /// </summary>
        User = 10,
        /// <summary>
        /// 维基人
        /// </summary>
        WikiEditor = 11,
    }
}
