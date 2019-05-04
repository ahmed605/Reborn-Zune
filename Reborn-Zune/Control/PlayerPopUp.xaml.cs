using Microsoft.Toolkit.Uwp.UI.Animations;
using Reborn_Zune.Model;
using Reborn_Zune.Utilities;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Reborn_Zune.Control
{
    public sealed partial class PlayerPopUp : UserControl
    {
        private Compositor _compositor;
        private FrameworkElement _panel;
        private Visual _panelVisual;
        private Visual _mediaGridVisual;
        private Visual _maskVisual;

        public event EventHandler<EventArgs> TilePageButtonClicked;

        public MainViewModel VM { get; set; }

        public PlayerPopUp()
        {
            this.InitializeComponent();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            _mediaGridVisual = MediaPopUpGrid.GetVisual();
            _maskVisual = BlurryBackground.GetVisual();
        }

        public void SetVM(MainViewModel mainVM)
        {
            VM = mainVM;
            SetMediaPlayer(VM._player);
        }

        private void SetMediaPlayer(MediaPlayer _player)
        {
            PopUpMediaElement.SetMediaPlayer(_player);
        }

        public void Hide()
        {
        }

        

        public void Show(FrameworkElement panel)
        {
            this.Opacity = 1f;
            _panel = panel;
            _panelVisual = panel.GetVisual();


            BlurryBackground.Visibility = Visibility.Visible;

            this.IsHitTestVisible = true;

            var fadeAnim = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnim.Duration = TimeSpan.FromMilliseconds(200);
            fadeAnim.InsertKeyFrame(0f, 1f);
            fadeAnim.InsertKeyFrame(1f, 0f);
            _panelVisual.StartAnimation("Opacity", fadeAnim);

            _maskVisual.Opacity = 1f;

            var fadeAnim2 = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnim2.Duration = TimeSpan.FromMilliseconds(200);
            fadeAnim2.InsertKeyFrame(0f, 0f);
            fadeAnim2.InsertKeyFrame(1f, 1f);
            _mediaGridVisual.StartAnimation("Opacity", fadeAnim2);
        }

        public void SetModel(LocalAlbumModel clickedItem)
        {
            //_currentPlaylist = clickedItem;
            //CustomMedianTPC.Model = clickedItem;
            //CurrentPlaylist.ItemsSource = clickedItem.Musics;
            //FillSystemMediaList(clickedItem, VM._isStop);
        }


        private void BlurryBackground_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var fadeAnim = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnim.Duration = TimeSpan.FromMilliseconds(200);
            fadeAnim.InsertKeyFrame(0f, 1f);
            fadeAnim.InsertKeyFrame(1f, 0f);
            _mediaGridVisual.StartAnimation("Opacity", fadeAnim);

            _maskVisual.Opacity = 0f;

            var fadeAnim2 = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnim2.Duration = TimeSpan.FromMilliseconds(200);
            fadeAnim2.InsertKeyFrame(0f, 0f);
            fadeAnim2.InsertKeyFrame(1f, 1f);
            _panelVisual.StartAnimation("Opacity", fadeAnim2);


            _panel.IsHitTestVisible = true;

            this.IsHitTestVisible = false;
        }

        private async void HideMaskBackground()
        {
            //BlurryBackground.Fade(0, 250).Start();
            //BlurryBackground.Blur(0, 250).Start();
            //await Task.Delay(400);
            //BlurryBackground.IsHitTestVisible = false;
        }

        private void DisplayMaskBorder()
        {
            //Debug.WriteLine("ShowMaskBorder");
            //BlurryBackground.Blur(5, 600).Start();
            //BlurryBackground.Fade(1, 400).Start();
            //BlurryBackground.IsHitTestVisible = true;
        }

        private void CustomMedianTPC_PreviousButtonClicked(object sender, EventArgs e)
        {

        }

        private void CustomMedianTPC_PlayButtonClicked(object sender, EventArgs e)
        {
           
            

        }

        private void CustomMedianTPC_NextButtonClicked(object sender, EventArgs e)
        {

        }



        public MediaListViewModel MediaList
        {
            get { return (MediaListViewModel)GetValue(MediaListProperty); }
            set { SetValue(MediaListProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MediaList.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MediaListProperty =
            DependencyProperty.Register("MediaList", typeof(MediaListViewModel), typeof(PlayerPopUp), new PropertyMetadata(null, onMediaListChanged));

        private static void onMediaListChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((PlayerPopUp)d).MediaListChanged(e.NewValue as MediaListViewModel);
        }

        private void MediaListChanged(MediaListViewModel newValue)
        {
        }

        private void CustomMedianTPC_TilePageButtonClicked(object sender, EventArgs e)
        {
            TilePageButtonClicked?.Invoke(this, EventArgs.Empty);
        }
    }
}
