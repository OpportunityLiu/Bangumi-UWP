using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Opportunity.MvvmUniverse.Settings;

namespace Bangumi.Client.Internal
{
    internal static class Storage
    {
        public static LocalStorage Local { get; } = new LocalStorage();
        internal sealed class LocalStorage : SettingCollection
        {
            internal LocalStorage() : base(ApplicationData.Current.LocalSettings, "Bangumi.Client") { }

            private static SettingProperty<string> userData
                = new SettingProperty<string>(nameof(UserData), typeof(LocalStorage));
            public string UserData { get => GetFromContainer(userData); set => SetToContainer(userData, value); }

        }

        public static RoamingStorage Roaming { get; } = new RoamingStorage();
        internal sealed class RoamingStorage : SettingCollection
        {
            internal RoamingStorage() : base(ApplicationData.Current.LocalSettings, "Bangumi.Client") { }
        }
    }
}
