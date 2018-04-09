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
using Windows.Graphics.Imaging;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Dictionary<String, LocalArtistModel> _artistsDict;
        private ObservableCollection<LocalArtistModel> _artists;
        private ObservableCollection<LocalAlbumModel> _albums;
        private ObservableCollection<LocalMusicModel> _musics;
        private ObservableCollection<LocalMusicModel> _thirdPanelList;
        private ObservableCollection<LocalAlbumModel> _secondPanelList;
        private CoreDispatcher dispatcher;
        private PlayerViewModel _playerViewModel;
        private String _thirdPanelTitle;
        private bool _isThirdPanelAltShown;

        public MediaPlayer _player = PlaybackService.Instance.Player;

        public MainViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            _artistsDict = new Dictionary<string, LocalArtistModel>();
            Artists = new ObservableCollection<LocalArtistModel>();
            Albums = new ObservableCollection<LocalAlbumModel>();
            Musics = new ObservableCollection<LocalMusicModel>();
            
            GetSong();

            ThirdPanelList = new ObservableCollection<LocalMusicModel>();
            SecondPanelList = new ObservableCollection<LocalAlbumModel>();


            IsThirdPanelAltShown = false;

            PlayerViewModel = new PlayerViewModel(_player, this.dispatcher);
        }


        private async void GetSong()
        {
            List<StorageFile> result = new List<StorageFile>();
            QueryOptions queryOption = new QueryOptions
                (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".mp4", ".m4a" });

            queryOption.FolderDepth = FolderDepth.Shallow;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            var files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
              (queryOption).GetFilesAsync();

            foreach(var item in files)
            {
                ProcessSongs(item);
            }
            
        }

        private async void ProcessSongs(StorageFile item)
        {
            String strAlbum;
            String strArtist;
            String strTitle;
            BitmapImage strThumbnail;

            strThumbnail = await GetThumbnail(item);
            MusicProperties property = await item.Properties.GetMusicPropertiesAsync();
            strTitle = property.Title;
            strAlbum = property.Album;
            strArtist = property.Artist;
            


            LocalMusicModel music = new LocalMusicModel()
            {
                Title = strTitle,
                Album = strAlbum,
                Artist = strArtist,
                Thumbnail = strThumbnail,
                Music = item,
                MusicID = Guid.NewGuid().ToString(),
                ThumbnailAvailable = (strThumbnail.PixelHeight == 0) ? false : true
            };

            if (_artistsDict.ContainsKey(strArtist))
            {
                _artistsDict[strArtist].AddSong(music);
            }
            else
            {
                var newArtist = new LocalArtistModel(strArtist);
                newArtist.AddSong(music);
                _artistsDict[strArtist] = newArtist;
                Artists.Add(newArtist);
            }

            GetAllAlbums();
            Musics.Add(music);
            ThirdPanelList.Add(music);
        }

        private void GetAllAlbums()
        {
            foreach(LocalArtistModel artist in Artists)
            {
                foreach(var item in artist.Albums)
                {
                    if (!Albums.Contains(item))
                    {
                        Albums.Add(item);
                        SecondPanelList.Add(item);
                    }
                }
            }
        }

        private async Task<BitmapImage> GetThumbnail(StorageFile item)
        {
            const ThumbnailMode thumbnailMode = ThumbnailMode.MusicView;
            const uint size = 100;
            BitmapDecoder decoder = null;
            using(StorageItemThumbnail thumbnail = await item.GetThumbnailAsync(thumbnailMode, size))
            {
                if (thumbnail != null && thumbnail.Type == ThumbnailType.Image)
                {
                    //decoder = await BitmapDecoder.CreateAsync(thumbnail);

                    //// Get the first frame
                    //BitmapFrame bitmapFrame = await decoder.GetFrameAsync(0);

                    //// Save the resolution (will be used for saving the file later)
                    //var dpiX = bitmapFrame.DpiX;
                    //var dpiY = bitmapFrame.DpiY;

                    //// Get the pixels
                    //PixelDataProvider dataProvider =
                    //    await bitmapFrame.GetPixelDataAsync(BitmapPixelFormat.Bgra8,
                    //                                        BitmapAlphaMode.Premultiplied,
                    //                                        new BitmapTransform(),
                    //                                        ExifOrientationMode.RespectExifOrientation,
                    //                                        ColorManagementMode.ColorManageToSRgb);

                    //byte[] pixels = dataProvider.DetachPixelData();

                    //// Create WriteableBitmap and set the pixels
                    //WriteableBitmap bitmap = new WriteableBitmap((int)bitmapFrame.PixelWidth,
                    //                                             (int)bitmapFrame.PixelHeight);

                    //using (Stream pixelStream = bitmap.PixelBuffer.AsStream())
                    //{
                    //    await pixelStream.WriteAsync(pixels, 0, pixels.Length);
                    //}

                    //// Invalidate the WriteableBitmap and set as Image source
                    //bitmap.Invalidate();
                    //return bitmap;
                    BitmapImage bitmap = new BitmapImage();
                    bitmap.SetSource(thumbnail);
                    return bitmap;
                }
                return new BitmapImage();
            }
           
        }

        public ObservableCollection<LocalArtistModel> Artists
        {
            get
            {
                return _artists;
            }
            set
            {
                if(_artists != value)
                {
                    _artists = value;
                    RaisePropertyChanged(() => Artists);
                }
            }
        }

        public ObservableCollection<LocalAlbumModel> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                if(_albums != value)
                {
                    _albums = value;
                    RaisePropertyChanged(() => Albums);
                }
            }
        }

        public ObservableCollection<LocalMusicModel> Musics
        {
            get
            {
                return _musics;
            }
            set
            {
                if(_musics != value)
                {
                    _musics = value;
                    RaisePropertyChanged(() => Musics);
                }
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

        public void AlbumTapped(object sender, TappedRoutedEventArgs e)
        {
            
            var album = (e.OriginalSource as FrameworkElement).DataContext as LocalAlbumModel;
            if (album == null)
                return;
            IsThirdPanelAltShown = true;
            var title = album.AlbumTitle;
            ThirdPanelTitle = title;

            ThirdPanelList.Clear();
            ThirdPanelList = new ObservableCollection<LocalMusicModel>(album.Musics);

            GC.Collect();
        }

        public void ArtistTapped(object sender, TappedRoutedEventArgs e)
        {
            var artist = (e.OriginalSource as FrameworkElement).DataContext as LocalArtistModel;
            if (artist == null)
                return;
            SecondPanelList = new ObservableCollection<LocalAlbumModel>(artist.Albums);

            ThirdPanelList = new ObservableCollection<LocalMusicModel>(artist.Musics);

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

    }
}
