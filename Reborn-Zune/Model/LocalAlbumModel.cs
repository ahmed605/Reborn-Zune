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
        #region Constructors
        public LocalAlbumModel()
        {
            MusicDict = new Dictionary<string, LocalMusicModel>();
        }

        public LocalAlbumModel(string album)
        {
            AlbumTitle = album;
            MusicDict = new Dictionary<string, LocalMusicModel>();
        }
        #endregion

        #region Fields
        private String _artist;
        private String _albumTitle;
        private BitmapImage _thumbnail;
        private Dictionary<String, LocalMusicModel> _musicDict;
        #endregion
        
        #region Properties
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

        public BitmapImage Thumbnail
        {
            get
            {
                return _thumbnail;
            }
            set
            {
                Set<BitmapImage>(() => this.Thumbnail, ref _thumbnail, value);
            }
        }

        public Dictionary<String, LocalMusicModel> MusicDict
        {
            get
            {
                return _musicDict;
            }
            set
            {
                if(_musicDict != value)
                {
                    _musicDict = value;
                    RaisePropertyChanged(() => MusicDict);
                }
            }
        }

        public ObservableCollection<LocalMusicModel> GetMusics
        {
            get
            {
                return new ObservableCollection<LocalMusicModel>(MusicDict.Values.OrderBy(x => x.Title));
            }
        }
        #endregion
        
        #region Helpers
        public void AddSong(LocalMusicModel music)
        {
            MusicDict[music.Title] = music;
        }
        #endregion


    }
}
