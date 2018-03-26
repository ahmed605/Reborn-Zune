using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class MainViewModel : ViewModelBase
    {
        private Dictionary<String, LocalArtistModel> _artists;
        public MainViewModel()
        {
            _artists = new Dictionary<string, LocalArtistModel>();

            GetSong();
        }

        private async void GetSong()
        {
            List<StorageFile> result = new List<StorageFile>();
            QueryOptions queryOption = new QueryOptions
                (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".mp4", ".m4a" });

            queryOption.FolderDepth = FolderDepth.Shallow;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            var files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
              (queryOption).GetFilesAsync();

            foreach(var item in files)
            {
                ProcessSongs(item);
            }
        }

        private async void ProcessSongs(StorageFile item)
        {
            String strAlbum;
            String strArtist;
            String strTitle;
            WriteableBitmap strThumbnail;


            LocalMusicModel music = new LocalMusicModel();
            MusicProperties property = await item.Properties.GetMusicPropertiesAsync();
            strTitle = property.Title;
            strAlbum = property.Album;
            strArtist = property.Artist;
            strThumbnail = await GetThumbnail(item);

            music.Title = strTitle;
            music.Album = strAlbum;
            music.Artist = strArtist;
            music.Thumbnail = strThumbnail;
            music.Music = item;

            if (_artists.ContainsKey(strArtist))
            {
                _artists[strArtist].AddSong(music);
            }
            else
            {
                var newArtist = new LocalArtistModel(strArtist);
                newArtist.AddSong(music);
                _artists[strArtist] = newArtist;
            }
            
        }

        private async Task<WriteableBitmap> GetThumbnail(StorageFile item)
        {
            const ThumbnailMode thumbnailMode = ThumbnailMode.MusicView;
            const uint size = 100;
            BitmapDecoder decoder = null;
            using(StorageItemThumbnail thumbnail = await item.GetThumbnailAsync(thumbnailMode, size))
            {
                decoder = await BitmapDecoder.CreateAsync(thumbnail);

                // Get the first frame
                BitmapFrame bitmapFrame = await decoder.GetFrameAsync(0);

                // Save the resolution (will be used for saving the file later)
                var dpiX = bitmapFrame.DpiX;
                var dpiY = bitmapFrame.DpiY;

                // Get the pixels
                PixelDataProvider dataProvider =
                    await bitmapFrame.GetPixelDataAsync(BitmapPixelFormat.Bgra8,
                                                        BitmapAlphaMode.Premultiplied,
                                                        new BitmapTransform(),
                                                        ExifOrientationMode.RespectExifOrientation,
                                                        ColorManagementMode.ColorManageToSRgb);

                byte[] pixels = dataProvider.DetachPixelData();

                // Create WriteableBitmap and set the pixels
                WriteableBitmap bitmap = new WriteableBitmap((int)bitmapFrame.PixelWidth,
                                                             (int)bitmapFrame.PixelHeight);

                using (Stream pixelStream = bitmap.PixelBuffer.AsStream())
                {
                    await pixelStream.WriteAsync(pixels, 0, pixels.Length);
                }

                // Invalidate the WriteableBitmap and set as Image source
                bitmap.Invalidate();
                return bitmap;
            }
        }
    }
}
