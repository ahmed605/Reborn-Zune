using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalMusicModel : ObservableObject
    {
        public LocalMusicModel() { }

        private StorageFile _music;
        private String _title;
        private String _album;
        private String _artist;
        private WriteableBitmap _thumbnail;

        public StorageFile Music
        {
            get
            {
                return _music;
            }
            set
            {
                Set<StorageFile>(() => this.Music, ref _music, value);
            }
        }
        
        public String Title
        {
            get
            {
                return _title;
            }
            set
            {
                Set<String>(() => this.Title, ref _title, value);
            }
        }

        public String Album
        {
            get
            {
                return _album;
            }
            set
            {
                Set<String>(() => this.Album, ref _album, value);
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

    }
}
