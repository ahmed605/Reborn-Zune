﻿using System;
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
    }
}
