using Newtonsoft.Json;
using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.Web.Http;

namespace Bangumi.Client.Schema
{
    public class Episode : WikiBase
    {
        [JsonConstructor]
        public Episode(long id) : base(id)
        {
        }


        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EpisodeType type;
        [JsonProperty("type")]
        public EpisodeType Type { get => this.type; set => Set(ref this.type, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int index;
        [JsonProperty("sort")]
        public int Index { get => this.index; set => Set(ref this.index, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private TimeSpan duration;
        [JsonProperty("duration")]
        public TimeSpan Duration { get => this.duration; set => Set(ref this.duration, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime airdate;
        [JsonProperty("airdate")]
        public DateTime AirDate { get => this.airdate; set => Set(ref this.airdate, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int comment;
        [JsonProperty("comment")]
        public int Comment { get => this.comment; set => Set(ref this.comment, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string desc;
        [JsonProperty("desc")]
        public string Description { get => this.desc; set => Set(ref this.desc, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private EpisodeStatus status;
        [JsonProperty("status")]
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public EpisodeStatus Status { get => this.status; set => Set(ref this.status, value); }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync() => throw new NotImplementedException();
    }

    public enum EpisodeStatus
    {
        Unkonwn = 0,
        Air = 1,
        Today = 2,
        NA = 3,
    }

    public enum EpisodeType
    {
        /// <summary>本篇</summary>
        Normal = 0,
        /// <summary>特别篇</summary>
        Speical = 1,
        ///<summary>OP</summary>
        Opening = 2,
        ///<summary>ED</summary>
        Ending = 3,
        ///<summary>预告/宣传/广告</summary>
        Preview = 4,
        ///<summary>MAD</summary>
        Mad = 5,
        ///<summary>其他</summary>
        Other = 6,
    }
}
