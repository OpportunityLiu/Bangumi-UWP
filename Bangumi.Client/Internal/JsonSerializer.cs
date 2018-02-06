using Newtonsoft.Json;
using Opportunity.MvvmUniverse.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Bangumi.Client.Internal
{
    class JsonSerializer<T> : ISerializer<T>
        where T : ResponseObject
    {
        public int CaculateSize(in T value)
        {
            if (value == null)
                return 0;
            return JsonConvert.SerializeObject(value, ResponseObject.JsonSettings).Length * sizeof(char);
        }

        public void Serialize(in T value, Span<byte> storage)
        {
            if (value == null)
                return;
            JsonConvert.SerializeObject(value, ResponseObject.JsonSettings).AsSpan().AsBytes().CopyTo(storage);
        }

        public void Deserialize(ReadOnlySpan<byte> storage, ref T value)
        {
            if (storage.IsEmpty)
            {
                value = default;
                return;
            }
            var s = new string(storage.NonPortableCast<byte, char>().ToArray());
            if (value == null)
                value = Activator.CreateInstance<T>();
            JsonConvert.PopulateObject(s, value, ResponseObject.JsonSettings);
        }
    }
}
