using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Reborn_Zune.Control
{
    public sealed partial class PlaylistPopUp : UserControl
    {
        private MainViewModel MainVM;
        private ILocalListModel model;
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

        public void SetVM(MainViewModel mainVM)
        {
            MainVM = mainVM;
        }

        public void SetModel(ILocalListModel model)
        {
            this.model = model;
            //LocalPlaylistModel
            if (model.isEditable)
            {
                PlaylistImages.Visibility = Visibility.Visible;
                AlbumImage.Visibility = Visibility.Collapsed;
                AlbumArtist.Visibility = Visibility.Collapsed;
                AddPlaylistButton.Visibility = Visibility.Collapsed;
                PlaylistImages.ThumbanilSource = new ObservableCollection<BitmapImage>(this.model.Musics.Select(m => m.ImageSource).ToList());
                Title.Text = (this.model as LocalPlaylistModel).Playlist.Name;
                playlist.ItemsSource = this.model.Musics;
            }
            //LocalAlbumModel
            else
            {
                AddPlaylistButton.Visibility = Visibility.Visible;
                PlaylistImages.Visibility = Visibility.Collapsed;
                AlbumImage.Visibility = Visibility.Visible;
                AlbumArtist.Visibility = Visibility.Visible;
                AlbumImage.Source = (this.model as LocalAlbumModel).Image;
                Title.Text = (this.model as LocalAlbumModel).Album.Title;
                AlbumArtist.Text = (this.model as LocalAlbumModel).Album.Artist.Name;
                playlist.ItemsSource = this.model.Musics;
            }
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

        private void MenuFlyoutItem_Click(object sender, RoutedEventArgs e)
        {
            var playlistName = (sender as MenuFlyoutItem).Text;
            MainVM.LibraryViewModel.AddSongsToPlaylist(playlistName, model.Musics.Select(m => m.Music).ToList());
        }
    }
}
