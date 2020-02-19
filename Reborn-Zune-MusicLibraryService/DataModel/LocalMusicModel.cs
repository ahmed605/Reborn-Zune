using GalaSoft.MvvmLight;
using Reborn_Zune_MusicLibraryService.DataModel;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class LocalMusicModel
    {
        public const String MediaItemIdKey = "mediaItemId";
        public StorageFile File { get; set; }
        public string Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public string Artist { get; set; }
        public string Year { get; set; }
        public string ThumbnailId { get; set; }
        public byte[] ImageBytes { get; set; }
        public BitmapImage Image { get; set; }

        public String GetMediaItemIdKey
        {
            get
            {
                return MediaItemIdKey;
            }
        }

        public MediaPlaybackItem ToPlaybackItem()
        {
            var source = MediaSource.CreateFromStorageFile(File);

            var playbackItem = new MediaPlaybackItem(source);

            var displayProperties = playbackItem.GetDisplayProperties();

            playbackItem.ApplyDisplayProperties(displayProperties);

            source.CustomProperties[GetMediaItemIdKey] = Id;

            return playbackItem;
        }

        public async Task GetFileAsync()
        {
            File = await StorageFile.GetFileFromPathAsync(Path);
        }

        public void GetBitmapImage()
        {
            if (ImageBytes.Length == 0)
            {
                Image = new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg"));
            }
            else
            {
                using (MemoryStream mem = new MemoryStream(ImageBytes, 0, ImageBytes.Length))
                {
                    mem.Position = 0;
                    Image = new BitmapImage();
                    Image.SetSource(mem.AsRandomAccessStream());
                }
            }
        }
    }
}
