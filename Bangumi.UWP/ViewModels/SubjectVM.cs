using Bangumi.Client.Schema;
using Opportunity.MvvmUniverse;
using Opportunity.MvvmUniverse.Collections;
using Opportunity.MvvmUniverse.Commands;
using Opportunity.MvvmUniverse.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Web.Http;

namespace Bangumi.UWP.ViewModels
{
    public class SubjectVM : ViewModelBase
    {
        private static AutoFillCacheStorage<long, SubjectVM> cache = AutoFillCacheStorage.Create<long, SubjectVM>(id => new SubjectVM(id));

        public static SubjectVM Get(long id) => cache.GetOrCreateAsync(id).GetResults();

        public static SubjectVM Set(Subject value) => cache[value.Id] = new SubjectVM(value);

        private SubjectVM(long id)
            : this(new Subject(id)) { }

        private SubjectVM(Subject value)
        {
            this.Value = value;
        }

        protected override IReadOnlyDictionary<string, System.Windows.Input.ICommand> Commands { get; } = new Dictionary<string, System.Windows.Input.ICommand>
        {
            [nameof(Refresh)] = AsyncCommandWithProgress.Create(cmd =>
            {
                var vm = (SubjectVM)cmd.Tag;
                return vm.Value.PopulateAsync();
            })
        };

        public Subject Value { get; }

        public AsyncCommandWithProgress<HttpProgress> Refresh => (AsyncCommandWithProgress<HttpProgress>)Commands[nameof(Refresh)];
    }
}
