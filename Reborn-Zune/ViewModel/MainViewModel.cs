using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using Reborn_Zune.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using Windows.Graphics.Imaging;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<LocalArtistModel> _firstPanelList;
        private ObservableCollection<LocalAlbumModel> _secondPanelList;
        private ObservableCollection<LocalMusicModel> _thirdPanelList;
        
        private CoreDispatcher dispatcher;
        private MusicsViewModel _musicsViewModel;
        private PlayerViewModel _playerViewModel;
        private String _thirdPanelTitle;
        private bool _isThirdPanelAltShown;
        public MediaPlayer _player = PlaybackService.Instance.Player;
        #endregion

        #region Constructor
        public MainViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            MusicsViewModel = new MusicsViewModel();
            PlayerViewModel = new PlayerViewModel(_player, this.dispatcher);

            BuildMusicDataBaseAsync();

            FirstPanelList = new ObservableCollection<LocalArtistModel>();
            SecondPanelList = new ObservableCollection<LocalAlbumModel>();
            ThirdPanelList = new ObservableCollection<LocalMusicModel>();

            IsThirdPanelAltShown = false;
        }
        #endregion

        #region Properties
        public ObservableCollection<LocalArtistModel> FirstPanelList
        {
            get
            {
                return _firstPanelList;
            }
            set
            {
                if (_firstPanelList != value)
                {
                    _firstPanelList = value;
                    RaisePropertyChanged(() => FirstPanelList);
                }
            }
        }

        public ObservableCollection<LocalAlbumModel> SecondPanelList
        {
            get
            {
                return _secondPanelList;
            }
            set
            {
                _secondPanelList = value;
                RaisePropertyChanged(() => SecondPanelList);
            }
        }

        public ObservableCollection<LocalMusicModel> ThirdPanelList
        {
            get
            {
                return _thirdPanelList;
            }
            set
            {
                _thirdPanelList = value;
                RaisePropertyChanged(() => ThirdPanelList);
            }
        }

        public PlayerViewModel PlayerViewModel
        {
            get
            {
                return _playerViewModel;
            }
            set
            {
                _playerViewModel = value;
                RaisePropertyChanged(() => PlayerViewModel);
            }
        }

        public MusicsViewModel MusicsViewModel
        {
            get
            {
                return _musicsViewModel;
            }
            set
            {
                if (_musicsViewModel != value)
                {
                    _musicsViewModel = value;
                    RaisePropertyChanged(() => MusicsViewModel);
                }
            }
        }

        public String ThirdPanelTitle
        {
            get
            {
                return _thirdPanelTitle;
            }
            set
            {
                _thirdPanelTitle = value;
                RaisePropertyChanged(() => ThirdPanelTitle);
            }
        }

        public bool IsThirdPanelAltShown
        {
            get
            {
                return _isThirdPanelAltShown;
            }
            set
            {
                _isThirdPanelAltShown = value;
                RaisePropertyChanged(() => IsThirdPanelAltShown);
            }
        }

        MediaPlaybackList PlaybackList
        {
            get { return _player.Source as MediaPlaybackList; }
            set { _player.Source = value; }
        }
        #endregion

        #region Helpers
        private async void BuildMusicDataBaseAsync()
        {
            await MusicsViewModel.BuildMusicDataBaseAsync();
            FirstPanelList = new ObservableCollection<LocalArtistModel>(MusicsViewModel.GetArtists);
            SecondPanelList = new ObservableCollection<LocalAlbumModel>(MusicsViewModel.GetAlbums);
            ThirdPanelList = new ObservableCollection<LocalMusicModel>(MusicsViewModel.GetMusics);

        }

        private async Task<WriteableBitmap> GrayScaleBitmap(WriteableBitmap thumbnail)
        {
            byte[] srcPixels = new byte[4 * thumbnail.PixelWidth * thumbnail.PixelHeight];
            using (Stream pixelStream = thumbnail.PixelBuffer.AsStream())
            {
                await pixelStream.ReadAsync(srcPixels, 0, srcPixels.Length);
            }
            WriteableBitmap dstBitmap =
               new WriteableBitmap(thumbnail.PixelWidth, thumbnail.PixelHeight);
            byte[] dstPixels = new byte[4 * dstBitmap.PixelWidth * dstBitmap.PixelHeight];
            for (int i = 0; i < srcPixels.Length; i += 4)
            {
                double b = (double)srcPixels[i] / 255.0;
                double g = (double)srcPixels[i + 1] / 255.0;
                double r = (double)srcPixels[i + 2] / 255.0;

                byte a = srcPixels[i + 3];

                double e = (0.21 * r + 0.71 * g + 0.07 * b) * 255;
                byte f = Convert.ToByte(e);

                dstPixels[i] = f;
                dstPixels[i + 1] = f;
                dstPixels[i + 2] = f;
                dstPixels[i + 3] = a;
            }
            // Move the pixels into the destination bitmap
            using (Stream pixelStream = dstBitmap.PixelBuffer.AsStream())
            {
                await pixelStream.WriteAsync(dstPixels, 0, dstPixels.Length);
            }
            dstBitmap.Invalidate();
            return dstBitmap;
        }

        private void UpdateThirdPanelList()
        {
            if (PlayBackListConsistencyDetect(ThirdPanelList))
                PlaybackList = ToPlayBackList(ThirdPanelList);
            PlayerViewModel.MediaList = new MediaListViewModel(ThirdPanelList, PlaybackList, dispatcher);

            GC.Collect();
        }

        private MediaPlaybackList ToPlayBackList(ObservableCollection<LocalMusicModel> musics)
        {
            var playbackList = new MediaPlaybackList();


            // Add playback items to the list
            foreach (var mediaItem in musics)
            {
                playbackList.Items.Add(mediaItem.ToPlaybackItem());
            }

            return playbackList;
        }

        private bool PlayBackListConsistencyDetect(ObservableCollection<LocalMusicModel> currentList)
        {
            if (PlaybackList == null)
                return true;

            // Verify consistency of the lists that were passed in
            var mediaListIds = currentList.Select(i => i.MusicID);
            var playbackListIds = PlaybackList.Items.Select(
                i => (string)i.Source.CustomProperties.SingleOrDefault(
                    p => p.Key == LocalMusicModel.MediaItemIdKey).Value);

            if (!mediaListIds.SequenceEqual(playbackListIds))
                return true;

            return false;

        }
        #endregion

        #region Events
        public void AlbumTapped(object sender, TappedRoutedEventArgs e)
        {

            var album = (e.OriginalSource as FrameworkElement).DataContext as LocalAlbumModel;
            if (album == null)
                return;
            IsThirdPanelAltShown = true;
            var title = album.AlbumTitle;
            ThirdPanelTitle = title;

            ThirdPanelList.Clear();
            ThirdPanelList = album.GetMusics;

            GC.Collect();
        }

        public void ArtistTapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsThirdPanelAltShown)
                IsThirdPanelAltShown = false;
            var artist = (e.OriginalSource as FrameworkElement).DataContext as LocalArtistModel;
            if (artist == null)
                return;
            SecondPanelList = MusicsViewModel.ArtistsDict[artist.Name].GetAlbums;

            ThirdPanelList = MusicsViewModel.ArtistsDict[artist.Name].GetMusics;

            GC.Collect();
        }

        public void DoubleTapped_Music(object sender, DoubleTappedRoutedEventArgs e)
        {
            int selectedIndex = (sender as ListView).SelectedIndex;
            if (PlayBackListConsistencyDetect(ThirdPanelList))
                PlaybackList = ToPlayBackList(ThirdPanelList);
            PlayerViewModel.MediaList = new MediaListViewModel(ThirdPanelList, PlaybackList, dispatcher);

            PlayerViewModel.SetCurrentItem(selectedIndex);

            GC.Collect();
        }

        public void AlbumDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            UpdateThirdPanelList();
        }

        public void ArtistDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            if (PlayBackListConsistencyDetect(ThirdPanelList))
                PlaybackList = ToPlayBackList(ThirdPanelList);
            PlayerViewModel.MediaList = new MediaListViewModel(ThirdPanelList, PlaybackList, dispatcher);

            GC.Collect();
        }
        #endregion
    }
}
