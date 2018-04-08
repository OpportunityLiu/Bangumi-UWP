using Bangumi.UWP.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// https://go.microsoft.com/fwlink/?LinkId=234238 上介绍了“空白页”项模板

namespace Bangumi.UWP.Views
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class SubjectPage : Page
    {
        public SubjectPage()
        {
            this.InitializeComponent();
        }

        public SubjectVM VM
        {
            get => (SubjectVM)GetValue(VMProperty);
            set => SetValue(VMProperty, value);
        }

        /// <summary>
        /// Indentify <see cref="VM"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty VMProperty =
            DependencyProperty.Register(nameof(VM), typeof(SubjectVM), typeof(SubjectPage), new PropertyMetadata(null, VMPropertyChanged));

        private static void VMPropertyChanged(DependencyObject dp, DependencyPropertyChangedEventArgs e)
        {
            var oldValue = (SubjectVM)e.OldValue;
            var newValue = (SubjectVM)e.NewValue;
            if (oldValue == newValue)
                return;
            var sender = (SubjectPage)dp;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            this.VM = SubjectVM.Get((long)e.Parameter);
            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Refresh)
                this.VM.Refresh.Execute();
        }

        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            base.OnNavigatingFrom(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
        }
    }
}
