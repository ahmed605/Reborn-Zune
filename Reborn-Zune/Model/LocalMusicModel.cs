using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using Reborn_Zune.Utilities;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalMusicModel : ObservableObject
    {
        public LocalMusicModel(Music music)
        {
            Music = music;
            MusicID = Guid.NewGuid().ToString();
        }

        public async Task GetStorageFile()
        {
            StorageFile = await StorageFile.GetFileFromPathAsync(Music.Path);
        }

        public async Task GetThumbnail()
        {
            ImageSource = await Utility.ImageFromBytes(Music.Thumbnail.Image);
        }

        public const String MediaItemIdKey = "mediaItemId";

        private Music _music;

        public Music Music
        {
            get
            {
                return _music;
            }
            set
            {
                Set<Music>(() => this.Music, ref _music, value);
            }
        }

        private BitmapImage _imageSource;
        public BitmapImage ImageSource
        {
            get
            {
                return _imageSource;
            }
            set
            {
                Set<BitmapImage>(() => this.ImageSource, ref _imageSource, value);
            }
        }

        private StorageFile _storageFile;
        public StorageFile StorageFile
        {
            get
            {
                return _storageFile;
            }
            set
            {
                Set<StorageFile>(() => this.StorageFile, ref _storageFile, value);
            }
        }

        private String _musicID;

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

        public MediaPlaybackItem ToPlaybackItem()
        {
            var source = MediaSource.CreateFromStorageFile(StorageFile);

            var playbackItem = new MediaPlaybackItem(source);

            var displayProperties = playbackItem.GetDisplayProperties();

            playbackItem.ApplyDisplayProperties(displayProperties);

            source.CustomProperties[GetMediaItemIdKey] = MusicID;

            return playbackItem;
        }

    }
}
