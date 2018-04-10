using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalAlbumModel : ObservableObject
    {
        public LocalAlbumModel()
        {
            Musics = new ObservableCollection<LocalMusicModel>();
        }

        public LocalAlbumModel(string album)
        {
            AlbumTitle = album;
            Musics = new ObservableCollection<LocalMusicModel>();
        }

        private String _artist;
        private String _albumTitle;
        private WriteableBitmap _thumbnail;
        private ObservableCollection<LocalMusicModel> _musics;

        public String Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                Set<String>(() => this.Artist, ref _artist, value);
            }
        }

        public String AlbumTitle
        {
            get
            {
                return _albumTitle;
            }
            set
            {
                Set<String>(() => this.AlbumTitle, ref _albumTitle, value);
            }
        }

        public WriteableBitmap Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                Set<WriteableBitmap>(() => this.Thumbnail, ref _thumbnail, value);
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
                Set<ObservableCollection<LocalMusicModel>>(() => this.Musics, ref _musics, value);
            }
        }

        public void AddSong(LocalMusicModel music)
        {
            Musics.Add(music);
        }
    }
}
