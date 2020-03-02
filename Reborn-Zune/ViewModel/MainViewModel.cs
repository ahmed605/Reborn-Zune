using GalaSoft.MvvmLight;
using Reborn_Zune_Common.Interface;
using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryService;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicPlayerService;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
#pragma warning disable 0169
namespace Reborn_Zune.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        public CoreDispatcher dispatcher;
        //private DetailViewModel _detailViewmodel;
        //private TileViewModel _tileViewModel;
        private Visibility _floatingVisible = Visibility.Collapsed;
        private MusicLibraryService libraryService = ServiceLocator.GetInstance("MusicLibraryService") as MusicLibraryService;
        private MusicPlaybackService<IPlaybackItem> playerService = ServiceLocator.GetInstance("MusicPlaybackService`1") as MusicPlaybackService<IPlaybackItem>;
        public bool _isStop = true;
        #endregion

        #region Constructor
        public MainViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        #endregion

        #region Properties
        
        public Visibility FloatingVisible
        {
            get
            {
                return _floatingVisible;
            }
            set
            {
                if (_floatingVisible != value)
                {
                    _floatingVisible = value;
                    RaisePropertyChanged(() => FloatingVisible);
                }
            }
        }

        #endregion

        #region Helpers

        public ObservableCollection<LocalAlbumModel> GetAllAlbums()
        {
            return new ObservableCollection<LocalAlbumModel>(libraryService.FetchAllAlbums());
        }

        public ObservableCollection<LocalPlaylistModel> GetAllPlaylists()
        {
            return new ObservableCollection<LocalPlaylistModel>(libraryService.FetchPlaylists());
        }




        //public void CreatTiles()
        //{
        //    TileViewModel.CreateTiles(LibraryViewModel.Thumbnails);
        //}

        //public MediaPlaybackList ToPlayBackList(ObservableCollection<LocalMusicModel> musics)
        //{
        //    var playbackList = new MediaPlaybackList();


        //    // Add playback items to the list
        //    foreach (var mediaItem in musics)
        //    {
        //        playbackList.Items.Add(mediaItem.ToPlaybackItem());
        //    }

        //    return playbackList;
        //}


        //public void SetClickList(ILocalListModel clickedItem)
        //{
        //    if(clickedItem is LocalAlbumModel)
        //    {
        //        var item = clickedItem as LocalAlbumModel;
        //        DetailViewModel = new DetailViewModel(item.Image, item.Title, item.AlbumArtist, item.Year, item.Musics, LibraryViewModel.Playlists);

        //    }
        //    else
        //    {
        //        var item = clickedItem as LocalPlaylistModel;
        //        DetailViewModel = new DetailViewModel(new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg")), item.Playlist.Name, "Various Artist", "",item.Musics, LibraryViewModel.Playlists);
        //    }
        //}

        //private bool PlayBackListConsistencyDetect(ObservableCollection<LocalMusicModel> currentList)
        //{
        //    if (PlaybackList == null)
        //        return true;

        //    // Verify consistency of the lists that were passed in
        //    var mediaListIds = currentList.Select(i => i.Music.Id);
        //    var playbackListIds = PlaybackList.Items.Select(
        //        i => (string)i.Source.CustomProperties.SingleOrDefault(
        //            p => p.Key == LocalMusicModel.MediaItemIdKey).Value);

        //    if (!mediaListIds.SequenceEqual(playbackListIds))
        //        return true;

        //    return false;

        //}

        //public void ShuffleAll()
        //{
        //    if (PlayBackListConsistencyDetect(LibraryViewModel.Musics))
        //        PlaybackList = ToPlayBackList(LibraryViewModel.Musics);
        //    PlayerViewModel.MediaList = new MediaListViewModel(LibraryViewModel.Musics, PlaybackList, dispatcher);
        //}

        //public void ShuffleAllPlaylists()
        //{
        //    if (PlayBackListConsistencyDetect(new ObservableCollection<LocalMusicModel>(LibraryViewModel.Playlists.Select(m => m.Musics).SelectMany(a => a).ToList())))
        //        PlaybackList = ToPlayBackList(new ObservableCollection<LocalMusicModel>(LibraryViewModel.Playlists.Select(m => m.Musics).SelectMany(a => a).ToList()));
        //    PlayerViewModel.MediaList = new MediaListViewModel(new ObservableCollection<LocalMusicModel>(LibraryViewModel.Playlists.Select(m => m.Musics).SelectMany(a => a).ToList()), PlaybackList, dispatcher);
        //}

        #endregion

        public async Task<bool> AddToPlaybackQueue(List<IPlaybackItem> model)
        {
            return await playerService.AddToPlaybackQueue(model);
        }

    }
}
