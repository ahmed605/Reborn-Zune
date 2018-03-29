using Reborn_Zune.Control;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class SplashScreen : Page
    {
        public SplashScreen()
        {
            this.InitializeComponent();
            LoadThumbNails();
        }

        private async void LoadThumbNails()
        {
            List<StorageFile> songs = await getSongs();

            List<WriteableBitmap> uniqueSongs = await ProcessSongsAsync(songs);

            List<UIElement> _Thumbnail = ThumbnailProcess(uniqueSongs);

            if (_Thumbnail != null && _Thumbnail.Count > 0)
            {
                Frame.Navigate(typeof(TilePage), _Thumbnail);
            }
        }

        private List<UIElement> ThumbnailProcess(List<WriteableBitmap> uniqueSongs)
        {
            List<UIElement> thumbnails = new List<UIElement>();
            Random rnd = new Random();
            for (int i = 0; i < 300; i++)
            {
                int factor = Spans(i);
                int id = rnd.Next(uniqueSongs.Count);
                Tile tile = new Tile()
                {
                    Width = factor * 75,
                    Height = factor * 75,
                    Thumbnail = uniqueSongs[id],
                    Index = i
                };
                thumbnails.Add(tile);
            }
            return thumbnails;
        }

        private async Task<List<WriteableBitmap>> ProcessSongsAsync(List<StorageFile> songs)
        {
            List<String> albums = new List<String>();
            List<WriteableBitmap> processedSongs = new List<WriteableBitmap>();
            foreach (StorageFile item in songs)
            {
                MusicProperties property = await item.Properties.GetMusicPropertiesAsync();

                if (!albums.Contains(property.Album))
                {
                    albums.Add(property.Album);

                    const ThumbnailMode thumbnailMode = ThumbnailMode.MusicView;
                    const uint size = 100;
                    BitmapDecoder decoder = null;
                    using (StorageItemThumbnail thumbnail = await item.GetThumbnailAsync(thumbnailMode, size))
                    {
                        // Also verify the type is ThumbnailType.Image (album art) instead of ThumbnailType.Icon
                        // (which may be returned as a fallback if the file does not provide album art)
                        if (thumbnail != null && thumbnail.Type == ThumbnailType.Image)
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
                            byte[] srcPixels = new byte[4 * bitmap.PixelWidth * bitmap.PixelHeight];
                            using (Stream pixelStream = bitmap.PixelBuffer.AsStream())
                            {
                                await pixelStream.ReadAsync(srcPixels, 0, srcPixels.Length);
                            }
                            WriteableBitmap dstBitmap =
                               new WriteableBitmap(bitmap.PixelWidth, bitmap.PixelHeight);
                            byte[] dstPixels = new byte[4 * dstBitmap.PixelWidth * dstBitmap.PixelHeight];
                            for (int i = 0; i < srcPixels.Length; i += 4)
                            {
                                double b = (double)srcPixels[i] / 255.0;
                                double g = (double)srcPixels[i + 1] / 255.0;
                                double r = (double)srcPixels[i + 2] / 255.0;

                                byte a = srcPixels[i + 3];

                                double e = (0.21 * r + 0.71 * g + 0.07 * b) * 255;
                                byte f = Convert.ToByte(e);

                                dstPixels[i] = f;
                                dstPixels[i + 1] = f;
                                dstPixels[i + 2] = f;
                                dstPixels[i + 3] = a;
                            }
                            // Move the pixels into the destination bitmap
                            using (Stream pixelStream = dstBitmap.PixelBuffer.AsStream())
                            {
                                await pixelStream.WriteAsync(dstPixels, 0, dstPixels.Length);
                            }
                            dstBitmap.Invalidate();
                            processedSongs.Add(dstBitmap);
                        }
                    }
                }
            }
            return processedSongs;
        }

        private async Task<List<StorageFile>> getSongs()
        {
            List<StorageFile> result = new List<StorageFile>();
            QueryOptions queryOption = new QueryOptions
                (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".mp4", ".m4a" });

            queryOption.FolderDepth = FolderDepth.Shallow;

            Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

            var files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
              (queryOption).GetFilesAsync();

            return new List<StorageFile>(files);
        }

        private int Spans(int i)
        {
            switch (i)
            {
                case 1:
                    return 4;
                case 3:
                    return 3;
                case 9:
                    return 3;
                case 23:
                    return 3;
                case 27:
                    return 3;
                case 32:
                    return 3;
                case 39:
                    return 3;
                case 42:
                    return 3;
                case 50:
                    return 4;
                case 52:
                    return 3;
                case 59:
                    return 3;
                case 65:
                    return 3;
                case 70:
                    return 3;
                case 82:
                    return 3;
                case 90:
                    return 3;
                case 98:
                    return 3;
                case 100:
                    return 3;
                case 103:
                    return 4;
                case 110:
                    return 3;
                case 118:
                    return 3;
                case 120:
                    return 3;
                case 121:
                    return 3;
                case 130:
                    return 3;
                case 140:
                    return 3;
                case 147:
                    return 4;
                case 155:
                    return 3;
                case 160:
                    return 3;
                case 166:
                    return 3;
                case 170:
                    return 3;
                case 175:
                    return 3;
                case 185:
                    return 4;
                case 200:
                    return 4;
                case 210:
                    return 3;
                case 217:
                    return 3;
                case 220:
                    return 4;
                case 252:
                    return 3;
                default:
                    return 1;

            }
        }
    }
}
