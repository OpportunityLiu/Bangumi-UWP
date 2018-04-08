using Bangumi.Client.Auth;
using HtmlAgilityPack;
using Opportunity.MvvmUniverse;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Core;
using Windows.UI.ApplicationSettings;
using Windows.Security.Authentication.Web;
using Bangumi.UWP.Internal;
using Bangumi.Client.Schema;
using System.Threading.Tasks;
using Opportunity.MvvmUniverse.Views;
using Opportunity.MvvmUniverse.Services.Navigation;
using Bangumi.UWP.Views;

// https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x804 上介绍了“空白页”项模板

namespace Bangumi.UWP
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void imgCaptcha_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Task.Run(async () => await AuthManager.AuthAsync());
                this.tbInfo.Text = $"Succeed {AuthManager.UserId}";
            }
            catch (Exception ex)
            {
                // Authentication failed. Handle parameter, SSL/TLS, and Network Unavailable errors here. 
                this.tbInfo.Text = ex.Message;
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await AuthManager.RefreshAsync();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var id = long.Parse(this.tbMail.Text);
            await Navigator.GetForCurrentView().NavigateAsync(typeof(SubjectPage), id);
        }
    }
}
