using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Opportunity.MvvmUniverse.Storage;

namespace Bangumi.Client.Internal
{
    internal static class Storage
    {
        public static LocalStorage Local { get; } = new LocalStorage();
        internal sealed class LocalStorage : LocalStorageObject
        {
            internal LocalStorage() : base(ApplicationData.Current.LocalSettings, "Bangumi.Client") { }

            private static StorageProperty<string> userData
                = new StorageProperty<string>(nameof(UserData));
            public string UserData { get => GetFromContainer(userData); set => SetToContainer(userData, value); }

        }

        public static RoamingStorage Roaming { get; } = new RoamingStorage();
        internal sealed class RoamingStorage : RoamingStorageObject
        {
            internal RoamingStorage() : base(ApplicationData.Current.RoamingSettings, "Bangumi.Client") { }
        }
    }
}
