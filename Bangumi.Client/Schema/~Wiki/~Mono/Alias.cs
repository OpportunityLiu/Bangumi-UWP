using Newtonsoft.Json;
using System;
using System.Diagnostics;

namespace Bangumi.Client.Schema
{
    [DebuggerDisplay(@"J = {Japanese}
R = {Romaji}
K = {Kana}
C = {Chinese}
N = {Nickname}")]
    public readonly struct Alias : IEquatable<Alias>
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

        public bool Equals(Alias other)
            => this.Japanese == other.Japanese
            && this.Romaji == other.Romaji
            && this.Kana == other.Kana
            && this.Chinese == other.Chinese
            && this.Nickname == other.Nickname;

        public override bool Equals(object obj) => obj is Alias other && Equals(other);

        public override int GetHashCode()
        {
            return code(Japanese) ^ code(Romaji) * 7 ^ code(Kana) * 37 ^ code(Chinese) * 17 ^ code(Nickname) * 11;
            int code(string v) => v is null ? -1 : v.GetHashCode();
        }
    }
}
