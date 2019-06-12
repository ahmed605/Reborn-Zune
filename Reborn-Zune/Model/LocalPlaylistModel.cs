using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalPlaylistModel : ObservableObject, ILocalListModel
    {
        public LocalPlaylistModel()
        {
            Musics = new ObservableCollection<LocalMusicModel>();
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

        public LibraryViewModel LibraryViewModel { get; set; }
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

        public ImageSource GetImage()
        {
            return new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png"));
        }

        public string GetTitle()
        {
            return Playlist.Name;
        }

        public string GetArtist()
        {
            return "Various Artists";
        }
    }
}
