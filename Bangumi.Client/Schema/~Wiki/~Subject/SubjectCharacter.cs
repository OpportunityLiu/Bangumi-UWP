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
        [JsonProperty]
        private string role_name;
        [JsonIgnore]
        public string Role { get => this.role_name; set => Set(ref this.role_name, value); }
    }
}
