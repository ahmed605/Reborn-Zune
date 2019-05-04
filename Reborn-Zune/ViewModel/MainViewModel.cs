using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using Reborn_Zune.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        public CoreDispatcher dispatcher;
        private LibraryViewModel _libraryViewModel;
        private PlayerViewModel _playerViewModel;
        //private TileViewModel _tileViewModel;
        public MediaPlayer _player = PlaybackService.Instance.Player;
        public bool _isStop = true;
        #endregion

        #region Constructor
        public MainViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
            LibraryViewModel = new LibraryViewModel();
            LibraryViewModel.InitializeFinished += LibraryViewModel_InitializeFinished;
            PlayerViewModel = new PlayerViewModel(_player, this.dispatcher);
            //TileViewModel = new TileViewModel();
            
        }

        private void LibraryViewModel_InitializeFinished(object sender, EventArgs e)
        {
            //var thumbnails = LibraryViewModel.Thumbnails;
            //TileViewModel.CreateTiles(thumbnails);
            //FirstPanelList = new ObservableCollection<LocalArtistModel>(LibraryViewModel.GetLocalArtists());
            //SecondPanelList = LibraryViewModel.Albums;
            //ThirdPanelList = LibraryViewModel.Musics;
        }
        #endregion

        #region Properties
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

        public MediaPlaybackList PlaybackList
        {
            get { return _player.Source as MediaPlaybackList; }
            set { _player.Source = value; }
        }

        #endregion

        #region Helpers



        //public void CreatTiles()
        //{
        //    TileViewModel.CreateTiles(LibraryViewModel.Thumbnails);
        //}

        public MediaPlaybackList ToPlayBackList(ObservableCollection<LocalMusicModel> musics)
        {
            var playbackList = new MediaPlaybackList();


            // Add playback items to the list
            foreach (var mediaItem in musics)
            {
                playbackList.Items.Add(mediaItem.ToPlaybackItem());
            }

            return playbackList;
        }

        public void ShowMediaPopUp()
        {
            //throw new NotImplementedException();
        }

        //private bool PlayBackListConsistencyDetect(ObservableCollection<LocalMusicModel> currentList)
        //{
        //    if (PlaybackList == null)
        //        return true;

        //    // Verify consistency of the lists that were passed in
        //    var mediaListIds = currentList.Select(i => i.MusicID);
        //    var playbackListIds = PlaybackList.Items.Select(
        //        i => (string)i.Source.CustomProperties.SingleOrDefault(
        //            p => p.Key == LocalMusicModel.MediaItemIdKey).Value);

        //    if (!mediaListIds.SequenceEqual(playbackListIds))
        //        return true;

        //    return false;

        //}

        #endregion


    }
}
