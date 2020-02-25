﻿using Microsoft.Toolkit.Uwp.Helpers;
using Reborn_Zune.Control;
using Reborn_Zune.ViewModel;
using System;
using System.Numerics;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.System.Threading;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TilePage : Page
    {
        private const float lightDepth = 200f;
        private const int animationDelay = 600;
        private const int animationDuration = 120;
        private CompositionEffectFactory _effectFactory;
        private Random _random = new Random();
        private Compositor _compositor;
        private PointLight _pointLight;
        private PointLight _secondPointLight;
        private AmbientLight _ambientLight;
        private Visual _root;
        private MainViewModel MainVM;
        private TileViewModel TileVM;
        public TilePage()
        {
            this.InitializeComponent();
            TitleBarSetting();
            
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

        private void TileRoot_Loaded(object sender, RoutedEventArgs e)
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            var screenSize = new Size((int)displayInformation.ScreenWidthInRawPixels,
                                      (int)displayInformation.ScreenHeightInRawPixels);
            Vector2 sizeLightBounds = new Vector2((float)screenSize.Width, (float)screenSize.Height);
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _root = ElementCompositionPreview.GetElementVisual(tilesPanel);
            _pointLight = _compositor.CreatePointLight();
            _secondPointLight = _compositor.CreatePointLight();
            _pointLight.Offset = new Vector3(-2500f, -2500f, 300f);
            _secondPointLight.Offset = new Vector3(-2500f, -2500f, 300f);
            _pointLight.Intensity = 1.3f;
            _secondPointLight.Intensity = 1.3f;
            _ambientLight = _compositor.CreateAmbientLight();
            _ambientLight.Intensity = 0.20f;
            _ambientLight.Color = "#d3d3d3".ToColor();
            
            _pointLight.CoordinateSpace = _root;
            _pointLight.Targets.Add(_root);
            _secondPointLight.CoordinateSpace = _root;
            _secondPointLight.Targets.Add(_root);
            _ambientLight.Targets.Add(_root);


            #region First light animation
            Vector3KeyFrameAnimation lightPositionAnimation;
            lightPositionAnimation = _compositor.CreateVector3KeyFrameAnimation();
            lightPositionAnimation.InsertKeyFrame(.0f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
            lightPositionAnimation.InsertKeyFrame(.16f, new Vector3(825f, 70f, lightDepth), _compositor.CreateLinearEasingFunction());
            lightPositionAnimation.InsertKeyFrame(.32f, new Vector3(2100f, 40f, lightDepth), _compositor.CreateLinearEasingFunction());
            lightPositionAnimation.InsertKeyFrame(.48f, new Vector3(825f, 70f, lightDepth), _compositor.CreateLinearEasingFunction());
            lightPositionAnimation.InsertKeyFrame(.64f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
            lightPositionAnimation.InsertKeyFrame(.8f, new Vector3(200f, 100f, lightDepth), _compositor.CreateLinearEasingFunction());
            lightPositionAnimation.InsertKeyFrame(1f, new Vector3(200f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
            lightPositionAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
            lightPositionAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
            lightPositionAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            ColorKeyFrameAnimation lightColorAnimation;
            lightColorAnimation = _compositor.CreateColorKeyFrameAnimation();
            lightColorAnimation.InsertKeyFrame(0f, "#865c2a".ToColor());
            lightColorAnimation.InsertKeyFrame(.2f, "#955a21".ToColor());
            lightColorAnimation.InsertKeyFrame(.4f, "#6c891b".ToColor());
            lightColorAnimation.InsertKeyFrame(.6f, "#62962c".ToColor());
            lightColorAnimation.InsertKeyFrame(.8f, "#6c401b".ToColor());
            lightColorAnimation.InsertKeyFrame(1f, "#865c2a".ToColor());
            lightColorAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
            lightColorAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
            lightColorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _pointLight.StartAnimation("Offset", lightPositionAnimation);
            _pointLight.StartAnimation("Color", lightColorAnimation);
            #endregion

            #region Second light animation
            Vector3KeyFrameAnimation secondLightPositionAnimation;
            secondLightPositionAnimation = _compositor.CreateVector3KeyFrameAnimation();
            secondLightPositionAnimation.InsertKeyFrame(.0f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
            secondLightPositionAnimation.InsertKeyFrame(.16f, new Vector3(1800f, 900f, lightDepth), _compositor.CreateLinearEasingFunction());
            secondLightPositionAnimation.InsertKeyFrame(.32f, new Vector3(400f, 1200f, lightDepth), _compositor.CreateLinearEasingFunction());
            secondLightPositionAnimation.InsertKeyFrame(.48f, new Vector3(1800f, 900f, lightDepth), _compositor.CreateLinearEasingFunction());
            secondLightPositionAnimation.InsertKeyFrame(.64f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
            secondLightPositionAnimation.InsertKeyFrame(.8f, new Vector3(2300f, 100f, lightDepth), _compositor.CreateLinearEasingFunction());
            secondLightPositionAnimation.InsertKeyFrame(1f, new Vector3(2300f, 700f, lightDepth), _compositor.CreateLinearEasingFunction());
            secondLightPositionAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
            secondLightPositionAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
            secondLightPositionAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            ColorKeyFrameAnimation secondLightColorAnimation;
            secondLightColorAnimation = _compositor.CreateColorKeyFrameAnimation();
            secondLightColorAnimation.InsertKeyFrame(0f, "#81268b".ToColor());
            secondLightColorAnimation.InsertKeyFrame(.2f, "#68446c".ToColor());
            secondLightColorAnimation.InsertKeyFrame(.4f, "#368843".ToColor());
            secondLightColorAnimation.InsertKeyFrame(.6f, "#315b8a".ToColor());
            secondLightColorAnimation.InsertKeyFrame(.8f, "#7d2185".ToColor());
            secondLightColorAnimation.InsertKeyFrame(1f, "#81268b".ToColor());
            secondLightColorAnimation.Duration = TimeSpan.FromSeconds(animationDuration);
            secondLightColorAnimation.DelayTime = TimeSpan.FromMilliseconds(animationDelay);
            secondLightColorAnimation.IterationBehavior = AnimationIterationBehavior.Forever;

            _secondPointLight.StartAnimation("Offset", secondLightPositionAnimation);
            _secondPointLight.StartAnimation("Color", secondLightColorAnimation);
            #endregion
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //MainVM = e.Parameter as MainViewModel;
            //TileVM = new TileViewModel(MainVM.LibraryViewModel.Thumbnails);
            //TileMediaElement.SetMediaPlayer(MainVM._player);
            //base.OnNavigatedTo(e);
        }
        

        private void CustomMTC_ExitButtonClicked(object sender, EventArgs e)
        {
            Frame.GoBack();
        }

        private void CustomMTC_FullScreenButtonClicked(object sender, EventArgs e)
        {
            var view = ApplicationView.GetForCurrentView();
            if (view.IsFullScreenMode)
            {
                view.ExitFullScreenMode();
            }
            else
            {
                view.TryEnterFullScreenMode();
            }
        }

        private void CustomMTC_ListViewGridChecked(object sender, EventArgs e)
        {

        }

        private void CustomMTC_ListViewGridUnChecked(object sender, EventArgs e)
        {

        }

        private void Tiles_Loaded(object sender, RoutedEventArgs e)
        {
            TimeSpan period = TimeSpan.FromSeconds(5);
            ThreadPoolTimer PeriodicTimer = ThreadPoolTimer.CreatePeriodicTimer(async (source) =>
            {
                Random rnd = new Random();
                await Dispatcher.RunAsync(CoreDispatcherPriority.High,
                    () =>
                    {

                        int imgSouceIdx = rnd.Next(TileVM.BitmapList.Count);
                        int tileIdx = rnd.Next((int)(TileVM.Tiles.Count * 0.8));
                        var imgSource = TileVM.BitmapList[imgSouceIdx];
                        var tile = TileVM.Tiles[tileIdx] as Tile;

                        tile.UpdateThumbnail(_compositor, imgSource);
                    });

            }, period);
        }

        
        private void Listview_ChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer != null)
            {
                return;
            }

            ListViewItem container = (ListViewItem)args.ItemContainer ?? new ListViewItem();
            container.IsDoubleTapEnabled = true;
            container.DoubleTapped += Container_DoubleTapped;

            args.ItemContainer = container;
        }

        private void Container_DoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            //TODO
            //int doubleClickedIdx = MainVM.PlayerViewModel.MediaList.MediaList.IndexOf((e.OriginalSource as FrameworkElement).DataContext as LocalMusicModel);
            //if (MainVM.PlayerViewModel.MediaList.CurrentItemIndex != doubleClickedIdx)
            //{
            //    MainVM.PlayerViewModel.SetCurrentItem(doubleClickedIdx);
            //}
        }
    }
}
