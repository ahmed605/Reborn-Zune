using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            TitleBarSetting();
            List<BitmapImage> imageList = new List<BitmapImage>
            {
                new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png")),
                new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png"))
            };
            CarouselControl.ItemsSource = imageList;
            CarouselControl.SelectedIndex = 3;
            albums.ItemsSource = imageList;
            playlists.ItemsSource = imageList;

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
    }
}
