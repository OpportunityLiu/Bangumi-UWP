using Newtonsoft.Json;
using System.Diagnostics;

namespace Bangumi.Client.Schema
{
    public class SubjectCharacter : Character
    {
        public SubjectCharacter(long id) : base(id)
        {
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string role_name;
        [JsonProperty("role_name")]
        public string Role { get => this.role_name; set => Set(ref this.role_name, value); }
    }
}
