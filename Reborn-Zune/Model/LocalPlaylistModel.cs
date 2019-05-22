using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Reborn_Zune.Model
{
    public class LocalPlaylistModel : ObservableObject, ILocalListModel
    {
        public LocalPlaylistModel()
        {

        }


        private Playlist _playlist;
        public Playlist Playlist
        {
            get
            {
                return _playlist;
            }
            set
            {
                Set<Playlist>(() => this.Playlist, ref _playlist, value);
            }
        }

        private ObservableCollection<LocalMusicModel> _musics;
        public ObservableCollection<LocalMusicModel> Musics
        {
            get
            {
                return _musics;
            }
            set
            {
                Set<ObservableCollection<LocalMusicModel>>(() => this.Musics, ref _musics, value);
            }
        }

        public bool isEditable
        {
            get
            {
                return true;
            }
        }

        public Visibility isVisible
        {
            get
            {
                return Visibility.Collapsed;
            }
        }
    }
}
