using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Animations;
using MusicLibraryService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashScreen : Page
    {
        public bool IsFirstUse => SystemInformation.IsFirstRun;
        public bool IsBusy { get; set; }
        public SplashScreen()
        {
            this.InitializeComponent();
            TitleBarSetting();
            LoadingControl.IsLoading = true;
            MusicLibrary.Initialize(IsFirstUse);
            MusicLibrary.InitializeFinished += MusicLibrary_InitializeFinished;
            
        }

        private static void TitleBarSetting()
        {
            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.InactiveBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = "#00000000".ToColor();
            titleBar.ButtonForegroundColor = Colors.DarkGray;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
            titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonPressedBackgroundColor = "#f5f5f5".ToColor();
            titleBar.ButtonPressedForegroundColor = Colors.Black;
        }

        private async void MusicLibrary_InitializeFinished(object sender, EventArgs e)
        {
            Thread.Sleep(500);
            LoadingControl.IsLoading = false;
            await Hint.Fade(1.0f,500,1000).StartAsync();
            Frame.Navigate(typeof(MainPage), EventArgs.Empty);
        }
    }
}
