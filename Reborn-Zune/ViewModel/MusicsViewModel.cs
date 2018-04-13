using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using Reborn_Zune.Model;
using TagLib;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class MusicsViewModel : ViewModelBase
    {
        #region

        private Dictionary<String, LocalArtistModel> _artistsDict;

        #endregion

        public MusicsViewModel()
        {
            ArtistsDict = new Dictionary<string, LocalArtistModel>();
        }
        
        #region Helper

        public async Task BuildMusicDataBaseAsync()
        {
            GetSong();
            await Task.Delay(2500);
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

            foreach (var item in files)
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

            var fileStream = await item.OpenStreamForReadAsync();

            var tagFile = TagLib.File.Create(new StreamFileAbstraction(item.Name,
                             fileStream, fileStream));

            var tags = tagFile.Tag;

            strThumbnail = await GetThumbnail(item);
            strTitle = tags.Title;
            strAlbum = tags.Album;
            strArtist = tags.Performers[0];

            LocalMusicModel music = new LocalMusicModel()
            {
                Title = strTitle,
                Album = strAlbum,
                Artist = strArtist,
                Music = item,
                MusicID = Guid.NewGuid().ToString(),
                Thumbnail = strThumbnail,
                ThumbnailAvailable = (strThumbnail.PixelHeight == 0) ? false : true
            };

            MusicInsert(music);
        }

        private void MusicInsert(LocalMusicModel music)
        {
            if (ArtistsDict.ContainsKey(music.Artist))
            {
                ArtistsDict[music.Artist].AddSong(music);
            }
            else
            {
                var newArtist = new LocalArtistModel(music.Artist);
                newArtist.AddSong(music);
                ArtistsDict[music.Artist] = newArtist;
            }
        }

        private async Task<WriteableBitmap> GetThumbnail(StorageFile item)
        {
            const ThumbnailMode thumbnailMode = ThumbnailMode.MusicView;
            const uint size = 100;
            BitmapDecoder decoder = null;
            using (StorageItemThumbnail thumbnail = await item.GetThumbnailAsync(thumbnailMode, size))
            {
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
                    return bitmap;
                }
                return null;
            }

        }

        #endregion

        #region Properties
        public Dictionary<String, LocalArtistModel> ArtistsDict
        {
            get
            {
                return _artistsDict;
            }
            set
            {
                if(_artistsDict != value)
                {
                    _artistsDict = value;
                    RaisePropertyChanged(() => ArtistsDict);
                }
            }
        }

        public List<LocalArtistModel> GetArtists
        {
            get
            {
                return new List<LocalArtistModel>(ArtistsDict.Values).OrderBy(x=>x.Name).ToList();
            }
        }

        public List<LocalAlbumModel> GetAlbums
        {
            get
            {
                return new List<LocalAlbumModel>(ArtistsDict.Values.SelectMany(
                    artist => artist.AlbumDict.Values.Select(
                        album => album)).OrderBy(
                    x=>x.AlbumTitle));
            }
        }

        public List<LocalMusicModel> GetMusics
        {
            get
            {
                return new List<LocalMusicModel>(ArtistsDict.Values.SelectMany(
                    artist => artist.AlbumDict.Values.SelectMany(
                        album => album.MusicDict.Values.Select(
                            x => x).OrderBy(x=>x.Title))));
            }
        }

        public List<WriteableBitmap> GetThumbnails
        {
            get
            {
                return new List<WriteableBitmap>(ArtistsDict.Values.SelectMany(
                    artist => artist.AlbumDict.Values.Select(
                        album => album.Thumbnail)));
            }
        }
        #endregion
    }
}