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
        private WriteableBitmap _thumbnail;
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
            // Create the media source from the Uri
            var source = MediaSource.CreateFromStorageFile(Music);

            // Create a configurable playback item backed by the media source
            var playbackItem = new MediaPlaybackItem(source);

            // Populate display properties for the item that will be used
            // to automatically update SystemMediaTransportControls when
            // the item is playing.
            var displayProperties = playbackItem.GetDisplayProperties();

            //displayProperties.Thumbnail = RandomAccessStreamReference.CreateFromStream(ThumbnailStream);

            // Apply properties to the playback item
            playbackItem.ApplyDisplayProperties(displayProperties);

            // It's often useful to save a reference or ID to correlate
            // a particular MediaPlaybackItem with the item from the
            // backing data model. CustomProperties stores serializable
            // types, so here we use the media item's URI as the
            // playback item's unique ID. You are also free to use your own
            // external dictionary if you want to reference non-serializable
            // types.
            source.CustomProperties[GetMediaItemIdKey] = MusicID;

            return playbackItem;
        }

    }
}
