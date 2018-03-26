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
        public LocalArtistModel()
        {
            Albums = new Dictionary<string, LocalAlbumModel>();
        }

        public LocalArtistModel(string strArtist)
        {
            Name = strArtist;
            Albums = new Dictionary<string, LocalAlbumModel>();
        }

        public void AddSong(LocalMusicModel music)
        {
            if (Albums.ContainsKey(music.Album))
            {
                Albums[music.Album].AddSong(music);
            }
            else
            {
                var newAlbum = new LocalAlbumModel(music.Album);
                newAlbum.AddSong(music);
                newAlbum.Artist = Name;
                newAlbum.Thumbnail = music.Thumbnail;
                Albums[music.Album] = newAlbum;
            }
        }

        private String _name;
        private Dictionary<string, LocalAlbumModel> _albums;

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

        public Dictionary<string, LocalAlbumModel> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                Set<Dictionary<string, LocalAlbumModel>>(() => this.Albums, ref _albums, value);
            }
        }
    }
}
