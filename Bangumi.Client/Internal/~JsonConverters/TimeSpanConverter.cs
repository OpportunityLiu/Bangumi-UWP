using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Bangumi.Client.Internal
{
    internal sealed class TimeSpanConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(TimeSpan);



        private static readonly string[] formats = genFormats().ToArray();
        private static IEnumerable<string> genFormats()
        {
            yield return "h:m:s";
            yield return "m:s";
            yield return "%m";
            var h = new[] { "h", "hr", "hour", "hours" };
            var m = new[] { "m", "min", "minute", "minutes" };
            var s = new[] { "s", "sec", "second", "seconds" };
            foreach (var hh in h)
            {
                foreach (var mm in m)
                {
                    foreach (var ss in s)
                    {
                        yield return $"m'{mm}'s'{ss}'";
                        yield return $"m'{mm}'s";
                        yield return $"m'{mm}'";
                        yield return $"h'{hh}'m'{mm}'s'{ss}'";
                        yield return $"h'{hh}'m'{mm}'s";
                        yield return $"h'{hh}'m'{mm}'";
                        yield return $"h'{hh}'m";
                        yield return $"h'{hh}'";
                    }
                }
            }
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return default(TimeSpan);
            var str = reader.Value.ToString();
            if (str.IsNullOrWhiteSpace())
                return default(TimeSpan);
            if (TimeSpan.TryParseExact(str, formats, null, out var t1))
                return t1;
            if (TimeSpan.TryParse(str, out var t2))
                return t2;
            Debugger.Break();
            return default(TimeSpan);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is TimeSpan time && time != default)
            {
                writer.WriteValue(time);
                return;
            }
            else
            {
                writer.WriteNull();
                return;
            }
        }
    }
}
