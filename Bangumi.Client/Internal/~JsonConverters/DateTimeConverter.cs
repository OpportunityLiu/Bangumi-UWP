using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;

namespace Bangumi.Client.Internal
{
    internal sealed class DateTimeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
            => objectType == typeof(DateTime);

        private static readonly Regex dt = new Regex(@"^\s*(\d+)\s*?[^\d]\s*?(\d+)\s*?[^\d]\s*?(\d+)\s*$", RegexOptions.Compiled);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null)
                return default(DateTime);
            var str = reader.Value.ToString();
            if (str.IsNullOrWhiteSpace())
                return default(DateTime);
            if (DateTime.TryParse(str, out var r0))
                return r0;
            var sec1 = 0;
            var sec2 = 0;
            var sec3 = 0;
            var m1 = dt.Match(str);
            if (m1.Success)
            {
                sec1 = int.Parse(m1.Groups[1].Value);
                sec2 = int.Parse(m1.Groups[2].Value);
                sec3 = int.Parse(m1.Groups[3].Value);
            }
            else if (str.All(char.IsDigit))
            {
                if (str.Length == 8 && (str.StartsWith("19") || str.StartsWith("20")))
                {
                    sec1 = int.Parse(str.Substring(0, 4));
                    sec2 = int.Parse(str.Substring(4, 2));
                    sec3 = int.Parse(str.Substring(6, 2));
                }
                else if (str.Length == 6)
                {
                    sec1 = int.Parse(str.Substring(0, 2));
                    sec2 = int.Parse(str.Substring(2, 2));
                    sec3 = int.Parse(str.Substring(4, 2));
                }
            }
            if (sec1 == 0 && sec2 == 0 && sec3 == 0)
            {
                Debugger.Break();
                return default;
            }
            // YMD
            if (sec2 <= 12 && sec3 <= 31)
                goto YMD;
            // DMY
            if (sec2 <= 12 && sec1 <= 31)
                goto DMY;
            // MDY
            if (sec1 <= 12 && sec2 <= 31)
                goto MDY;

            YMD:
            return new DateTime(toY(sec1), sec2, sec3);
            DMY:
            return new DateTime(toY(sec3), sec2, sec1);
            MDY:
            return new DateTime(toY(sec3), sec1, sec2);

            int toY(int y)
            {
                if (y >= 100)
                    return y;
                if (y >= 40)
                    return 1900 + y;
                else
                    return 2000 + y;
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is DateTime time && time != default)
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
