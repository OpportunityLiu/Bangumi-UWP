using Newtonsoft.Json;

namespace Bangumi.Client.Schema
{
    public readonly struct Alias
    {
        [JsonConstructor]
        internal Alias(string jp, string romaji, string kana, string zh, string nick)
        {
            this.Japanese = jp;
            this.Romaji = romaji;
            this.Kana = kana;
            this.Chinese = zh;
            this.Nickname = nick;
        }


        public string Japanese { get; }
        public string Romaji { get; }
        public string Kana { get; }
        public string Chinese { get; }
        public string Nickname { get; }
    }
}
