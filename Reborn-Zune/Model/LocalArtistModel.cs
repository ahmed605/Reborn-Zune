using GalaSoft.MvvmLight;
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
        public LocalArtistModel()
        {
            AlbumDict = new Dictionary<string, LocalAlbumModel>();
        }

        public LocalArtistModel(string strArtist)
        {
            Name = strArtist;
            AlbumDict = new Dictionary<string, LocalAlbumModel>();
        }
        #endregion
        
        #region Helper
        public void AddSong(LocalMusicModel music)
        {
            if (AlbumDict.ContainsKey(music.Album))
            {
                AlbumDict[music.Album].AddSong(music);
            }
            else
            {
                var newAlbum = new LocalAlbumModel(music.Album);
                newAlbum.AddSong(music);
                newAlbum.Artist = Name;
                newAlbum.Thumbnail = music.Thumbnail;
                AlbumDict[music.Album] = newAlbum;
            }
        }
        #endregion

        #region Properties
        private String _name;
        private Dictionary<string, LocalAlbumModel> _albumsDict;

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                Set<String>(() => this.Name, ref _name, value);
            }
        }
        public Dictionary<string, LocalAlbumModel> AlbumDict
        {
            get
            {
                return _albumsDict;
            }
            set
            {
                if(_albumsDict != value)
                {
                    _albumsDict = value;
                    RaisePropertyChanged(() => AlbumDict);
                }
            }
        }

        public ObservableCollection<LocalMusicModel> GetMusics
        {
            get
            {
                return new ObservableCollection<LocalMusicModel>(AlbumDict.Values.SelectMany(album => album.MusicDict.Values.OrderBy(music => music.Title)));
            }
        }

        public ObservableCollection<LocalAlbumModel> GetAlbums
        {
            get
            {
                return new ObservableCollection<LocalAlbumModel>(AlbumDict.Values.OrderBy(x => x.AlbumTitle));
            }
        }

        #endregion
    }
}
