using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Reborn_Zune.Model;
using Reborn_Zune.Utilities;
using Reborn_Zune.ViewModel;
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

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Reborn_Zune.Control
{
    public sealed partial class PlaylistPopUp : UserControl
    {
        private MainViewModel MainVM;
        private LocalAlbumModel model;
        private FrameworkElement _gridviewItem;
        private Compositor _compositor;
        private Visual _gridviewItemVisual;
        private Visual _playlistPopupVisual;
        public event EventHandler InvokeMediaPopUpEvent;

        public PlaylistPopUp()
        {
            this.InitializeComponent();
            _playlistPopupVisual = PlaylistPopUpGrid.GetVisual();
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
        }

        //public Vector2 GetTargetPosition()
        //{
        //    var size = new Vector2(400f, 400f);
        //    var x = (Window.Current.Bounds.Width - size.X) / 2;
        //    var y = (Window.Current.Bounds.Height - size.Y) / 2;
        //    return new Vector2((float)x, (float)y);
        //}

        public void SetVM(MainViewModel mainVM)
        {
            MainVM = mainVM;
        }

        public void SetModel(LocalAlbumModel localAlbumModel)
        {
            model = localAlbumModel;
            AlbumImage.Source = localAlbumModel.Image;
            AlbumTitle.Text = localAlbumModel.Album.Title;
            AlbumArtist.Text = localAlbumModel.Album.Artist.Name;
            playlist.ItemsSource = localAlbumModel.Musics;
        }

        public void Show(FrameworkElement parentPanel)
        {
            this.IsHitTestVisible = true;
            _gridviewItem = parentPanel;
            _gridviewItemVisual = _gridviewItem.GetVisual();
            ToggleListItemAnimationAsync(true);
            MaskBorder.Opacity = 1f;
            MaskBorder.IsHitTestVisible = true;
        }

        private void ToggleListItemAnimationAsync(bool v)
        {
            if (v)
            {
                var fadeAnim = _compositor.CreateScalarKeyFrameAnimation();
                fadeAnim.Duration = TimeSpan.FromMilliseconds(200);
                fadeAnim.InsertKeyFrame(0f, 1f);
                fadeAnim.InsertKeyFrame(1f, 0f);
                _gridviewItemVisual.StartAnimation("Opacity", fadeAnim);
            }

            this.Visibility = Visibility.Visible;

            var startY = v ? (float)Window.Current.Bounds.Height + _gridviewItemVisual.Size.Y / 2 : 0f;
            var endY = v ? 0f : (float)Window.Current.Bounds.Height + _gridviewItemVisual.Size.Y / 2;


            var offsetAnim = _compositor.CreateVector3KeyFrameAnimation();
            offsetAnim.Duration = TimeSpan.FromMilliseconds(400);
            offsetAnim.InsertKeyFrame(0f, new Vector3(0f, startY, 0f));
            offsetAnim.InsertKeyFrame(1f, new Vector3(0f, endY, 0f));
            _playlistPopupVisual.StartAnimation("Translation", offsetAnim);

            
        }

        public async void Hide()
        {
            this.IsHitTestVisible = false;
            ToggleListItemAnimationAsync(false);
            MaskBorder.IsHitTestVisible = false;
            await Task.Delay(300);
            var fadeAnim = _compositor.CreateScalarKeyFrameAnimation();
            fadeAnim.Duration = TimeSpan.FromMilliseconds(200);
            fadeAnim.InsertKeyFrame(0f, 0f);
            fadeAnim.InsertKeyFrame(1f, 1f);
            _gridviewItemVisual.StartAnimation("Opacity", fadeAnim);
            this.Visibility = Visibility.Collapsed;
        }

        private void MaskBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Hide();
        }

        private void PlayButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FillPlabackList();
            MainVM.PlayerViewModel.SetCurrentItem(0);
            MainVM.PlayerViewModel.GetPlayer().Play();

            InvokeMediaPopUpEvent?.Invoke(this, EventArgs.Empty);
        }

        private void FillPlabackList()
        {
            MainVM.PlaybackList = MainVM.ToPlayBackList(model.Musics);
            MainVM.PlayerViewModel.MediaList = new MediaListViewModel(model.Musics, MainVM.PlaybackList, MainVM.dispatcher);
        }

        private void ShuffleButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FillPlabackList();
            Random rnd = new Random();
            MainVM.PlayerViewModel.SetCurrentItem(rnd.Next(model.Musics.Count));
            MainVM.PlaybackList.ShuffleEnabled = true;
            MainVM.PlayerViewModel.GetPlayer().Play();
            InvokeMediaPopUpEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}
