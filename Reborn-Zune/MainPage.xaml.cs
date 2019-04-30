using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
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
            //CarouselControl.ItemsSource = imageList;
            //CarouselControl.SelectedIndex = 3;
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

        private void Albums_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer != null)
            {
                return;
            }

            GridViewItem container = (GridViewItem)args.ItemContainer ?? new GridViewItem();
            container.PointerEntered -= ItemContainer_PointerEntered;
            container.PointerExited -= ItemContainer_PointerExited;

            container.PointerEntered += ItemContainer_PointerEntered;
            container.PointerExited += ItemContainer_PointerExited;

            args.ItemContainer = container;
        }

        private void ItemContainer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
            if (panel != null)
            {
                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(1200) };
                animation.StartAnimation(panel);

                var parentAnimation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(1200) };
                parentAnimation.StartAnimation(panel.Parent as UIElement);
            }
            
        }

        private void ItemContainer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse)
            {
                var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
                if (panel != null)
                {
                    panel.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(600) };
                    animation.StartAnimation(panel);

                    var parentAnimation = new ScaleAnimation() { To = "1.1", Duration = TimeSpan.FromMilliseconds(600) };
                    parentAnimation.StartAnimation(panel.Parent as UIElement);
                }
            }
        }
    }
}
