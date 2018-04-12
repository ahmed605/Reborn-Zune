using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.Helpers;
using Reborn_Zune.Model;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Windows.Graphics.Display;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private MainViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            this.NavigationCacheMode = NavigationCacheMode.Required;

            viewModel = new MainViewModel(Dispatcher);
            mediaPlayer.SetMediaPlayer(viewModel._player);
            TitleBarSetting();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var para = e.Parameter as PlayerViewModel;
            if(para != null)
            {
                viewModel.PlayerViewModel = para;
                mediaPlayer.SetMediaPlayer(viewModel.PlayerViewModel.GetPlayer());
            }
            base.OnNavigatedTo(e);
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

        private async void CustomMediaTransportControl_Clicked(object sender, EventArgs e)
        {
            //ObservableCollection<WriteableBitmap> thumblist = await viewModel.GetThumbnails();
            //TileViewModel tileViewModel = new TileViewModel()
            //{
            //    BitmapList = thumblist,
            //    PlayerViewModel = viewModel.PlayerViewModel
            //};
            //Frame.Navigate(typeof(TilePage), tileViewModel);
        }

        private void CustomMTC_RepeatCheckBoxChecked(object sender, EventArgs e)
        {
            viewModel.PlayerViewModel.MediaList.PlaybackList.AutoRepeatEnabled = true;
            App.Repeated = true;
        }

        private void CustomMTC_RepeatCheckBoxUnchecked(object sender, EventArgs e)
        {
            viewModel.PlayerViewModel.MediaList.PlaybackList.AutoRepeatEnabled = false;
            App.Repeated = false;
        }

        private void CustomMTC_ShuffleCheckBoxChecked(object sender, EventArgs e)
        {
            viewModel.PlayerViewModel.MediaList.PlaybackList.ShuffleEnabled = true;
            App.Shuffled = true;
        }

        private void CustomMTC_ShuffleCheckBoxUnchecked(object sender, EventArgs e)
        {
            viewModel.PlayerViewModel.MediaList.PlaybackList.ShuffleEnabled = false;
            App.Shuffled = false;
        }
    }
}
