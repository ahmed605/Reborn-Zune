using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalMusicModel : ObservableObject
    {
        public LocalMusicModel()
        {
            
        }


        public const String MediaItemIdKey = "mediaItemId";

        private StorageFile _music;
        private String _title;
        private String _album;
        private String _artist;
        private BitmapImage _thumbnail;
        private String _musicID;
        private bool _thumbnailAvailable;

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

        public String MusicID
        {
            get
            {
                return _musicID;
            }
            set
            {
                Set<String>(() => this.MusicID, ref _musicID, value);
            }
        }
        
        public String GetMediaItemIdKey
        {
            get
            {
                return MediaItemIdKey;
            }
        }

        public bool ThumbnailAvailable
        {
            get
            {
                return _thumbnailAvailable;
            }
            set
            {
                Set<bool>(() => this.ThumbnailAvailable, ref _thumbnailAvailable, value);
            }
        }

        public MediaPlaybackItem ToPlaybackItem()
        {
            var source = MediaSource.CreateFromStorageFile(Music);

            var playbackItem = new MediaPlaybackItem(source);

           
            var displayProperties = playbackItem.GetDisplayProperties();

            playbackItem.ApplyDisplayProperties(displayProperties);

            source.CustomProperties[GetMediaItemIdKey] = MusicID;

            return playbackItem;
        }

    }
}
