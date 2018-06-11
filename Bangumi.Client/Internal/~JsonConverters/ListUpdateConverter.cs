using Newtonsoft.Json;
using Opportunity.MvvmUniverse.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bangumi.Client.Internal
{
    internal sealed class ListUpdateConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            if (!objectType.GetTypeInfo().IsGenericType)
                return false;
            return objectType.GetGenericTypeDefinition() == typeof(ObservableList<>);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var eleType = objectType.GenericTypeArguments[0];
            var data = serializer.Deserialize(reader, eleType.MakeArrayType());
            return updateMethod.MakeGenericMethod(eleType).Invoke(null, new object[] { existingValue, data, serializer });
        }

        private static readonly MethodInfo updateMethod = typeof(ListUpdateConverter).GetMethod("update", BindingFlags.NonPublic | BindingFlags.Static);
        private static ObservableList<T> update<T>(ObservableList<T> d, T[] data, JsonSerializer serializer)
        {
            if (d is null)
            {
                return new ObservableList<T>(data);
            }
            else if (data.IsNullOrEmpty())
            {
                d.Clear();
                return d;
            }
            d.Update(data, EqualityComparer<T>.Default, (o, n) =>
            {
                using (var w = new StringWriter())
                {
                    serializer.Serialize(w, n);
                    var r = new StringReader(w.ToString());
                    serializer.Populate(r, o);
                }
            });
            return d;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}
