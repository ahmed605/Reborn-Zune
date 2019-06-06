using Microsoft.Toolkit.Uwp.Helpers;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Reborn_Zune.Control;
using Reborn_Zune.Model;
using Reborn_Zune.Utilities;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;
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

        private MainViewModel MainVM { get; set; }
        private Compositor _compositor;
        private Visual _floatingVisual;
        public MainPage()
        {
            this.InitializeComponent();
            
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            TitleBarSetting();
            MainVM = new MainViewModel(Dispatcher);
            _floatingVisual = PlayerFloating.GetVisual();
        }
        

        private void ShowFloating()
        {
            PlayerFloating.Visibility = Visibility.Visible;
            PlayerFloating.IsHitTestVisible = true;
            var startY = (float)Window.Current.Bounds.Height + _floatingVisual.Size.Y / 2;
            var endY = 0f;


            var offsetAnim = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnim.Duration = TimeSpan.FromMilliseconds(200);
            offsetAnim.InsertKeyFrame(0f, new Vector3(0f, startY, 0f));
            offsetAnim.InsertKeyFrame(1f, new Vector3(0f, endY, 0f));
            _floatingVisual.StartAnimation("Translation", offsetAnim);
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

        private void GridView_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
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
            container.Tapped += Container_Tapped;
            
            args.ItemContainer = container;
        }

        private async void Container_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var rootElement = sender as FrameworkElement;
            var shadow = rootElement.FindDescendant<DropShadowPanel>();
            if (shadow != null)
            {

                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(shadow);
            }

            var spotlight = rootElement.FindDescendant<ShadowSpotLightControl>();
            if (spotlight != null)
            {

                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(spotlight);
            }

            var border = rootElement.FindDescendantByName("border");
            if (border != null)
            {

                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(border);
            }

            var buttons = rootElement.FindDescendantByName("Buttons");
            if (buttons != null)
            {

                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(buttons);
            }
            await Task.Delay(200);
            shadow.Visibility = Visibility.Collapsed;
            spotlight.Visibility = Visibility.Collapsed;
            border.Visibility = Visibility.Collapsed;
            buttons.Visibility = Visibility.Collapsed;

            
        }

        private async void ItemContainer_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var rootElement = sender as FrameworkElement;
            var shadow = rootElement.FindDescendant<DropShadowPanel>();
            if (shadow != null)
            {
                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(shadow);
            }

            var spotlight = rootElement.FindDescendant<ShadowSpotLightControl>();
            if (spotlight != null)
            {
                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(spotlight);
            }

            var border = rootElement.FindDescendantByName("border");
            if (border != null)
            {
                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(border);
            }

            var buttons = rootElement.FindDescendantByName("Buttons");
            if (buttons != null)
            {
                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(200) };
                animation.StartAnimation(buttons);
            }
            await Task.Delay(200);
            shadow.Visibility = Visibility.Collapsed;
            spotlight.Visibility = Visibility.Collapsed;
            border.Visibility = Visibility.Collapsed;
            buttons.Visibility = Visibility.Collapsed;
        }

        private void ItemContainer_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            var rootElement = sender as FrameworkElement;
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse || e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                var shadow = rootElement.FindDescendant<DropShadowPanel>();
                if (shadow != null)
                {
                    shadow.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(200) };
                    animation.StartAnimation(shadow);
                }

                var spotlight = rootElement.FindDescendant<ShadowSpotLightControl>();
                if (spotlight != null)
                {
                    spotlight.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(200) };
                    animation.StartAnimation(spotlight);
                }

                var border = rootElement.FindDescendantByName("border");
                if (border != null)
                {
                    border.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(200) };
                    animation.StartAnimation(border);
                }

                var buttons = rootElement.FindDescendantByName("Buttons");
                if (buttons != null)
                {
                    buttons.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(200), Delay = TimeSpan.FromMilliseconds(300) };
                    animation.StartAnimation(buttons);
                }
            }
        }

        private void GridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            MainVM.SetClickList(e.ClickedItem as ILocalListModel);
            Frame.Navigate(typeof(PlaylistDetailPage),  MainVM);
        }

        private void PlayerFloating_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Mouse || e.Pointer.PointerDeviceType == Windows.Devices.Input.PointerDeviceType.Pen)
            {
                var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
                if (panel != null)
                {
                    panel.Visibility = Visibility.Visible;
                    var animation = new OpacityAnimation() { To = 1, Duration = TimeSpan.FromMilliseconds(400) };
                    animation.StartAnimation(panel);

                    var parentAnimation = new ScaleAnimation() { To = "1.1", Duration = TimeSpan.FromMilliseconds(400) };
                    parentAnimation.StartAnimation(panel.Parent as UIElement);
                }
            }
        }

        private void PlayerFloating_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            var panel = (sender as FrameworkElement).FindDescendant<DropShadowPanel>();
            if (panel != null)
            {
                var animation = new OpacityAnimation() { To = 0, Duration = TimeSpan.FromMilliseconds(900) };
                animation.StartAnimation(panel);

                var parentAnimation = new ScaleAnimation() { To = "1", Duration = TimeSpan.FromMilliseconds(900) };
                parentAnimation.StartAnimation(panel.Parent as UIElement);
            }
            GC.Collect();
        }

        private void PlayerFloating_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TilePage), MainVM);
        }


        private void NewPlaylistButton_Click(object sender, RoutedEventArgs e)
        {
            MainVM.LibraryViewModel.CreatePlaylist(PlaylistName.Text);
        }

        private void PlayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var album = (sender as Button).DataContext as ILocalListModel;
            MainVM.SetMediaList(album);
            
            

            if(MainVM.FloatingVisible == Visibility.Collapsed)
            {
                MainVM.FloatingVisible = Visibility.Visible;
                var offSetAnimation = _compositor.CreateVector3KeyFrameAnimation();
                offSetAnimation.InsertKeyFrame(0f, new Vector3(_floatingVisual.Offset.X, _floatingVisual.Offset.Y + 100, 0));
                offSetAnimation.InsertKeyFrame(1f, new Vector3(_floatingVisual.Offset.X, _floatingVisual.Offset.Y, 0));
                offSetAnimation.Duration = TimeSpan.FromMilliseconds(500);
                _floatingVisual.StartAnimation("Translation", offSetAnimation);
            }
            
            e.Handled = true;
        }

        private void AddToButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }
    }
}
