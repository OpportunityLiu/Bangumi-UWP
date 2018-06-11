using Opportunity.MvvmUniverse.Collections;

namespace Bangumi.Client.Schema
{
    //    [DebuggerDisplay(@"J = {Japanese}
    //R = {Romaji}
    //K = {Kana}
    //C = {Chinese}
    //N = {Nickname}")]
    //    public readonly struct Alias : IEquatable<Alias>
    //    {
    //        [JsonConstructor]
    //        internal Alias(string jp, string romaji, string kana, string zh, string nick)
    //        {
    //            this.Japanese = jp;
    //            this.Romaji = romaji;
    //            this.Kana = kana;
    //            this.Chinese = zh;
    //            this.Nickname = nick;
    //        }

    //        public string Japanese { get; }
    //        public string Romaji { get; }
    //        public string Kana { get; }
    //        public string Chinese { get; }
    //        public string Nickname { get; }

    //        public bool Equals(Alias other)
    //            => this.Japanese == other.Japanese
    //            && this.Romaji == other.Romaji
    //            && this.Kana == other.Kana
    //            && this.Chinese == other.Chinese
    //            && this.Nickname == other.Nickname;

    //        public override bool Equals(object obj) => obj is Alias other && Equals(other);

    //        public override int GetHashCode()
    //        {
    //            return code(Japanese) ^ code(Romaji) * 7 ^ code(Kana) * 37 ^ code(Chinese) * 17 ^ code(Nickname) * 11;
    //            int code(string v) => v is null ? -1 : v.GetHashCode();
    //        }
    //    }

    public class InfoBox : ObservableDictionary<string, object>
    {
        //public string birth { get; set; }
        //public string height { get; set; }
        //public string gender { get; set; }
        //public Alias alias { get; set; }
        //// string or string[]
        //public object source { get; set; }
        //public string name_cn { get; set; }
        //public string cv { get; set; }

        protected override ObservableDictionaryView<string, object> ReadOnlyViewFactory() => new InfoBoxView(this);

        public new InfoBoxView AsReadOnly() => (InfoBoxView)base.AsReadOnly();
    }

    public class InfoBoxView : ObservableDictionaryView<string, object>
    {
        public InfoBoxView(InfoBox dictionary) : base(dictionary)
        {
        }
    }
}
