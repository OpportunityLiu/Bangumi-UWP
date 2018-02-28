using Bangumi.Client.Wiki;
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
            string result = "";
            try
            {
                var state = Windows.Security.Cryptography.CryptographicBuffer.GenerateRandomNumber().ToString("X");
                var webAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None,
                  new Uri($"https://bgm.tv/oauth/authorize?client_id={AuthInfo.Instance.GetAppId()}&response_type=code&state={state}"));
                switch (webAuthenticationResult.ResponseStatus)
                {
                case WebAuthenticationStatus.Success:
                    // Successful authentication. 
                    result = webAuthenticationResult.ResponseData.ToString();
                    await SessionManager.AuthAsync(AuthInfo.Instance, new Uri(webAuthenticationResult.ResponseData));
                    break;
                case WebAuthenticationStatus.ErrorHttp:
                    // HTTP error. 
                    result = webAuthenticationResult.ResponseErrorDetail.ToString();
                    break;
                case WebAuthenticationStatus.UserCancel:
                    break;
                default:
                    // Other error.
                    result = webAuthenticationResult.ResponseData.ToString();
                    break;
                }
            }
            catch (Exception ex)
            {
                // Authentication failed. Handle parameter, SSL/TLS, and Network Unavailable errors here. 
                result = ex.Message;
            }
            this.tbInfo.Text = result;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
            await SessionManager.RefreshAsync(AuthInfo.Instance);
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SessionManager.Clear();
            var s = new Subject(168395);
            await s.FetchDataAsync();
        }
    }
}
