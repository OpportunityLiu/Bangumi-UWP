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
            DispatcherHelper.BeginInvoke(async () => this.imgCaptcha.Source = await Client.Authentication.SessionManager.GetCaptchaAsync());
        }

        private async void imgCaptcha_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.imgCaptcha.Source = await Client.Authentication.SessionManager.GetCaptchaAsync();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Client.Authentication.SessionManager.LogOnAsync(this.tbMail.Text, this.pbPass.Password, this.tbCaptcha.Text);
                this.tbInfo.Text = "Succeed";
            }
            catch (Exception ex)
            {
                this.tbInfo.Text = ex.Message;
            }
        }
    }
}
