using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using Reborn_Zune.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
#pragma warning disable 0169
namespace Reborn_Zune.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        #region Fields
        private ObservableCollection<LocalArtistModel> _firstPanelList;
        private ObservableCollection<LocalAlbumModel> _secondPanelList;
        private ObservableCollection<LocalMusicModel> _thirdPanelList;
        
        private CoreDispatcher dispatcher;
        private LibraryViewModel _libraryViewModel;
        private PlayerViewModel _playerViewModel;
        private TileViewModel _tileViewModel;
        private String _thirdPanelTitle;
        private bool _isThirdPanelAltShown;
        private bool _isRepeated;
        private bool _isShuffled;
        public MediaPlayer _player = PlaybackService.Instance.Player;
        #endregion

        #region Constructor
        public MainViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            LibraryViewModel = new LibraryViewModel();
            PlayerViewModel = new PlayerViewModel(_player, this.dispatcher);
            TileViewModel = new TileViewModel();
            FirstPanelList = new ObservableCollection<LocalArtistModel>();
            SecondPanelList = new ObservableCollection<LocalAlbumModel>();
            ThirdPanelList = new ObservableCollection<LocalMusicModel>();
            IsThirdPanelAltShown = false;
            LibraryViewModel.InitializeFinished += LibraryViewModel_InitializeFinished;
        }

        private async void LibraryViewModel_InitializeFinished(object sender, EventArgs e)
        {
            TileViewModel.CreateTiles(await LibraryViewModel.GetThumbnails());
            FirstPanelList = new ObservableCollection<LocalArtistModel>(LibraryViewModel.GetLocalArtists());
            SecondPanelList = new ObservableCollection<LocalAlbumModel>(LibraryViewModel.GetLocalAlbums());
            ThirdPanelList = new ObservableCollection<LocalMusicModel>(LibraryViewModel.GetLocalMusics());
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

        public LibraryViewModel LibraryViewModel
        {
            get
            {
                return _libraryViewModel;
            }
            set
            {
                if (_libraryViewModel != value)
                {
                    _libraryViewModel = value;
                    RaisePropertyChanged(() => LibraryViewModel);
                }
            }
        }

        public TileViewModel TileViewModel
        {
            get
            {
                return _tileViewModel;
            }
            set
            {
                if(_tileViewModel != value)
                {
                    _tileViewModel = value;
                    RaisePropertyChanged(() => TileViewModel);
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

        public bool IsRepeated
        {
            get
            {
                return _isRepeated;
            }
            set
            {
                if(_isRepeated != value)
                {
                    _isRepeated = value;
                    RaisePropertyChanged(() => IsRepeated);
                }
            }
        }

        public bool IsShuffled
        {
            get
            {
                return _isShuffled;
            }
            set
            {
                if(_isShuffled != value)
                {
                    _isShuffled = value;
                    RaisePropertyChanged(() => IsShuffled);
                }
            }
        }

        MediaPlaybackList PlaybackList
        {
            get { return _player.Source as MediaPlaybackList; }
            set { _player.Source = value; }
        }
        #endregion

        #region Helpers

        public void CreatTiles()
        {
            //TileViewModel.CreateTiles(MusicsViewModel.GetThumbnails);
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

        public void ClearTiles()
        {
            TileViewModel.ClearTiles();
        }
        #endregion

        #region Events
        
        public void MTC_RepeatCheckBoxChecked(object sender, EventArgs e)
        {
            if(PlayerViewModel.MediaList != null)
            {
                PlayerViewModel.MediaList.PlaybackList.AutoRepeatEnabled = true;
                IsRepeated = true;
            }
            
        }

        public void MTC_RepeatCheckBoxUnchecked(object sender, EventArgs e)
        {
            if (PlayerViewModel.MediaList != null)
            {
                PlayerViewModel.MediaList.PlaybackList.AutoRepeatEnabled = false;
                IsRepeated = false;
            }

        }

        public void MTC_ShuffleCheckBoxChecked(object sender, EventArgs e)
        {
            if (PlayerViewModel.MediaList != null)
            {
                PlayerViewModel.MediaList.PlaybackList.ShuffleEnabled = true;
                IsShuffled = true;
            }
            
        }

        public void MTC_ShuffleCheckBoxUnchecked(object sender, EventArgs e)
        {
            if (PlayerViewModel.MediaList != null)
            {
                PlayerViewModel.MediaList.PlaybackList.ShuffleEnabled = false;
                IsShuffled = false;
            }
            
        }
        #endregion
    }
}
