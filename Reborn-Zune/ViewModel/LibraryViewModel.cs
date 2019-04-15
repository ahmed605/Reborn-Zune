using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using MusicLibraryService;
using Reborn_Zune.Model;
using Reborn_Zune.Utilities;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class LibraryViewModel : ViewModelBase
    {
        public event EventHandler InitializeFinished;
        public LibraryViewModel()
        {
            MusicLibrary.InitializeFinished += MusicLibrary_InitializeFinished;
        }

        private void MusicLibrary_InitializeFinished(object sender, EventArgs e)
        {
            Library = MusicLibrary.FetchAll();
            InitializeFinished?.Invoke(this, EventArgs.Empty);
        }

        private Library _library;

        public Library Library
        {
            get { return _library; }
            set { _library = value; }
        }

        public List<LocalMusicModel> GetLocalMusics()
        {
            List<LocalMusicModel> result = new List<LocalMusicModel>();
            foreach(Music music in Library.musics)
            {
                result.Add(new LocalMusicModel(music));
            }
            return result;
        }

        public List<LocalAlbumModel> GetLocalAlbums()
        {
            List<LocalAlbumModel> result = new List<LocalAlbumModel>();
            foreach (Album album in Library.albums)
            {
                result.Add(new LocalAlbumModel(album));
            }
            return result;
        }

        public List<LocalArtistModel> GetLocalArtists()
        {
            List<LocalArtistModel> result = new List<LocalArtistModel>();
            foreach(Artist artist in Library.artists)
            {
                result.Add(new LocalArtistModel(artist));
            }
            return result;
        }

        public async Task<List<BitmapImage>> GetThumbnails()
        {
            List<BitmapImage> result = new List<BitmapImage>();
            foreach(Thumbnail thumbnail in Library.thumbnails)
            {
                result.Add(await Utility.ImageFromBytes(thumbnail.Image));
            }
            return result;
        }


        
        
    }
}