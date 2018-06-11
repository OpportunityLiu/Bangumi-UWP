using Opportunity.MvvmUniverse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bangumi.Client.Internal;
using HtmlAgilityPack;
using Opportunity.MvvmUniverse.Collections;
using Newtonsoft.Json;
using Windows.Foundation;
using Windows.Web.Http;
using System.Collections;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace Bangumi.Client.Schema
{
    public class Subject : WikiBase
    {
        [JsonConstructor]
        public Subject(long id) : base(id)
        {
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string summary;
        [JsonProperty("summary")]
        public string Summary { get => this.summary; protected set => Set(ref this.summary, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonProperty]
        private readonly ObservableList<Episode> eps = new ObservableList<Episode>();
        [JsonIgnore]
        public ObservableListView<Episode> Episodes => this.eps.AsReadOnly();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private DateTime airDate;
        [JsonProperty("air_date")]
        public DateTime AirDate { get => this.airDate; set => Set(ref this.airDate, value); }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private RatingState rating;
        [JsonProperty("rating")]
        public RatingState Rating { get => this.rating; set => Set(ref this.rating, value); }

        public readonly struct RatingState
        {
            [JsonConstructor]
            internal RatingState(int total, IReadOnlyDictionary<int, int> count, double score)
            {
                this.Total = total;
                this.Score = score;
                this.Count = new RatingCount(count);
            }

            public int Total { get; }
            public RatingCount Count { get; }
            public double Score { get; }

            public readonly struct RatingCount : IReadOnlyDictionary<int, int>, IDictionary
            {
                private static readonly ReadOnlyCollection<int> keys = new ReadOnlyCollection<int>(Enumerable.Range(1, 10).ToArray());

                internal RatingCount(IReadOnlyDictionary<int, int> count)
                {
                    this.data = new int[10];
                    this.data[0] = count[1];
                    this.data[1] = count[2];
                    this.data[2] = count[3];
                    this.data[3] = count[4];
                    this.data[4] = count[5];
                    this.data[5] = count[6];
                    this.data[6] = count[7];
                    this.data[7] = count[8];
                    this.data[8] = count[9];
                    this.data[9] = count[10];
                }

                private readonly int[] data;

                public IEnumerable<int> Keys => keys;
                ICollection IDictionary.Keys => keys;

                public IEnumerable<int> Values => this.data.AsEnumerable();
                ICollection IDictionary.Values => this.data;

                public int Count => 10;

                bool IDictionary.IsFixedSize => true;
                bool IDictionary.IsReadOnly => true;
                bool ICollection.IsSynchronized => false;
                object ICollection.SyncRoot => this.data;

                public int this[int key] => this.data[key - 1];
                object IDictionary.this[object key] { get => this[(int)key]; set => throw new InvalidOperationException(); }

                public bool ContainsKey(int key) => key > 0 && key <= 10;
                bool IDictionary.Contains(object key) => key is int k && ContainsKey(k);
                public bool TryGetValue(int key, out int value)
                {
                    if (ContainsKey(key))
                    {
                        value = this[key];
                        return true;
                    }
                    value = default;
                    return false;
                }
                public IEnumerator<KeyValuePair<int, int>> GetEnumerator()
                {
                    for (var i = 0; i < this.data.Length; i++)
                    {
                        yield return new KeyValuePair<int, int>(i + 1, this.data[i]);
                    }
                }
                IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
                void IDictionary.Add(object key, object value) => throw new InvalidOperationException();
                void IDictionary.Clear() => throw new InvalidOperationException();
                IDictionaryEnumerator IDictionary.GetEnumerator()
                    => ((IDictionary)this.ToDictionary(kv => kv.Key, kv => kv.Value)).GetEnumerator();
                void IDictionary.Remove(object key) => throw new InvalidOperationException();
                void ICollection.CopyTo(Array array, int index) => ((ICollection)this.ToList()).CopyTo(array, index);
            }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private int rank;
        [JsonProperty("rank")]
        public int Rank { get => this.rank; set => Set(ref this.rank, value); }

        public readonly struct CollectionState
        {
            [JsonConstructor]
            internal CollectionState(int wish, int collect, int doing, int on_hold, int dropped)
            {
                this.Wish = wish;
                this.Collect = collect;
                this.Doing = doing;
                this.OnHold = on_hold;
                this.Dropped = dropped;
            }

            [JsonProperty("wish")]
            public int Wish { get; }
            [JsonProperty("collect")]
            public int Collect { get; }
            [JsonProperty("doing")]
            public int Doing { get; }
            [JsonProperty("on_hold")]
            public int OnHold { get; }
            [JsonProperty("dropped")]
            public int Dropped { get; }
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private CollectionState collection;
        public CollectionState Collection { get => this.collection; set => Set(ref this.collection, value); }

        [JsonProperty]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableList<SubjectCharacter> crt = new ObservableList<SubjectCharacter>();
        [JsonIgnore]
        public ObservableListView<SubjectCharacter> Characters => this.crt.AsReadOnly();

        [JsonProperty]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableList<SubjectStaff> staff = new ObservableList<SubjectStaff>();
        [JsonIgnore]
        public ObservableListView<SubjectStaff> Staff => this.staff.AsReadOnly();

        [JsonProperty]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableList<Topic> topic = new ObservableList<Topic>();
        [JsonIgnore]
        public ObservableListView<Topic> Topics => this.topic.AsReadOnly();

        [JsonProperty]
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ObservableList<Blog> blog = new ObservableList<Blog>();
        [JsonIgnore]
        public ObservableListView<Blog> Blogs => this.blog.AsReadOnly();

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private SubjectType type;
        [JsonProperty("type")]
        public SubjectType Type { get => this.type; set => Set(ref this.type, value); }

        public override IAsyncActionWithProgress<HttpProgress> PopulateAsync()
        {
            return MyHttpClient.GetJsonAsync(new Uri(Config.ApiUri, $"/subject/{Id}?responseGroup=large"), this);
        }
    }
}
