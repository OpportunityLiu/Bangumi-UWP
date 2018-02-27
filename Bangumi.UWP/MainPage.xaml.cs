using Bangumi.Client.Wiki;
using Bangumi.Client.User;
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
            Dispatcher.Begin(async () =>
            {
                if (SessionManager.IsGuest)
                    this.imgCaptcha.Source = await SessionManager.GetCaptchaAsync();
                else
                    this.tbInfo.Text = SessionManager.Current.Uri.ToString();
            });
        }

        private async void imgCaptcha_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            this.imgCaptcha.Source = await SessionManager.GetCaptchaAsync();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await SessionManager.LogOnAsync(this.tbMail.Text, this.pbPass.Password, this.tbCaptcha.Text);
                this.tbInfo.Text = SessionManager.Current.Uri.ToString();
            }
            catch (Exception ex)
            {
                this.tbInfo.Text = ex.Message;
            }
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SessionManager.LogOff();
            this.tbInfo.Text = "LogOff";
            this.imgCaptcha.Source = await SessionManager.GetCaptchaAsync();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var s = new Subject(168395);
            await s.FetchDataAsync();
        }
    }
}
