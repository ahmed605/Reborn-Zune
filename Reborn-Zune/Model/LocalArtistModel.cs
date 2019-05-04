using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune.Model
{
    public class LocalArtistModel : ObservableObject
    {
        #region Constructor
        public LocalArtistModel(Artist artist, ObservableCollection<LocalAlbumModel> albums)
        {
            Artist = artist;
            Albums = new ObservableCollection<LocalAlbumModel>(albums.Where(a => a.Album.Artist == Artist).ToList());
            Musics = new ObservableCollection<LocalMusicModel>(Albums.SelectMany(a => a.Musics).ToList());
        }
        #endregion

        private Artist _artist;
        public Artist Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                Set<Artist>(() => this.Artist, ref _artist, value);
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

        private ObservableCollection<LocalAlbumModel> _albums;
        public ObservableCollection<LocalAlbumModel> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                Set<ObservableCollection<LocalAlbumModel>>(() => this.Albums, ref _albums, value);
            }
        }
    }
}
