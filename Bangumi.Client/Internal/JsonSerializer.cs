using Newtonsoft.Json;
using Opportunity.MvvmUniverse.Storage;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Bangumi.Client.Internal
{
    class JsonSerializer<T> : ISerializer<T>
        where T : ResponseObject
    {
        public void Serialize(in T value, DataWriter storage)
        {
            if (value == null)
            {
                storage.WriteUInt32(uint.MaxValue);
                return;
            }
            var str = JsonConvert.SerializeObject(value, ResponseObject.JsonSettings);
            storage.WriteUInt32(storage.MeasureString(str));
            storage.WriteString(str);
        }

        public void Deserialize(DataReader storage, ref T value)
        {
            var length = storage.ReadUInt32();
            if (length == uint.MaxValue)
            {
                value = default;
                return;
            }
            var str = storage.ReadString(length);
            if (value == null)
                value = Activator.CreateInstance<T>();
            JsonConvert.PopulateObject(str, value, ResponseObject.JsonSettings);
        }
    }
}
