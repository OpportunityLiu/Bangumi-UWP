using Newtonsoft.Json;
using System.Diagnostics;
using Opportunity.MvvmUniverse.Collections;

namespace Bangumi.Client.Schema
{
    public class SubjectStaff : Person
    {
        public SubjectStaff(long id) : base(id)
        {
        }

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        [JsonProperty]
        private readonly ObservableList<string> jobs = new ObservableList<string>();
        [JsonIgnore]
        public ObservableListView<string> Jobs => this.jobs.AsReadOnly();

        //[DebuggerBrowsable(DebuggerBrowsableState.Never)]
        //[JsonProperty]
        //private string role_name;
        //[JsonIgnore]
        //public string Role { get => this.role_name; set => Set(ref this.role_name, value); }
    }
}
