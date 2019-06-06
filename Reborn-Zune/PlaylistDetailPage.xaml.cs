using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Reborn_Zune.Utilities;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class PlaylistDetailPage : Page
    {
        public MainViewModel MainVM { get; set; }
        public ObservableCollection<string> Items = new ObservableCollection<string>
            {
                "a","a","a","a","a","a","a","a","a","a","a",
            };
        private Compositor _compositor;
        private Visual _floatingVisual;
        private Visual _playButtonVisual;
        private Visual _addToButtonVisual;

        public PlaylistDetailPage()
        {
            this.InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            list.ItemsSource = Items;
            _floatingVisual = PlayerFloating.GetVisual();
            _playButtonVisual = PlayButton.GetVisual();
            _addToButtonVisual = AddToButton.GetVisual();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            MainVM = e.Parameter as MainViewModel;
            base.OnNavigatedTo(e);
        }

        private void ExitButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.GoBack();
        }

        private void PlayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            MainVM.SetMediaList();
            if (MainVM.FloatingVisible == Visibility.Collapsed)
            {
                MainVM.FloatingVisible = Visibility.Visible;
                var offSetAnimation = _compositor.CreateVector3KeyFrameAnimation();
                offSetAnimation.InsertKeyFrame(0f, new Vector3(_floatingVisual.Offset.X, _floatingVisual.Offset.Y + 100, 0));
                offSetAnimation.InsertKeyFrame(1f, new Vector3(_floatingVisual.Offset.X, _floatingVisual.Offset.Y, 0));
                offSetAnimation.Duration = TimeSpan.FromMilliseconds(500);
                _floatingVisual.StartAnimation("Translation", offSetAnimation);
            }
        }


        private void CurrentPlayingThumbnail_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TilePage), MainVM);
        }
        

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
            if (panel != null)
            {
                panel.Fade(1f, 200, 0).Start();
            }

            var parentAnimation = new ScaleAnimation() { To = "1.1", Duration = TimeSpan.FromMilliseconds(200) };
            parentAnimation.StartAnimation(sender as UIElement);
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
            if (panel != null)
            {
                panel.Fade(0f, 200, 0).Start();
            }

            var parentAnimation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(200) };
            parentAnimation.StartAnimation(sender as UIElement);
        }
    }
}
