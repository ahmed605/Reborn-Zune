using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
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
            Thumbnails = new ObservableCollection<BitmapImage>();
            Musics = new ObservableCollection<LocalMusicModel>();
            Albums = new ObservableCollection<LocalAlbumModel>();
            Library = MusicLibrary.FetchAll();
            Finialize();
        }

        private async void Finialize()
        {
            await GetThumbnails();
            await GetLocalMusics();
            await GetLocalAlbums();

            InitializeFinished?.Invoke(this, EventArgs.Empty);
        }

        private Library _library;

        public Library Library
        {
            get { return _library; }
            set { _library = value; }
        }

        private ObservableCollection<BitmapImage> _thumbnails;
        public ObservableCollection<BitmapImage> Thumbnails
        {
            get
            {
                return _thumbnails;
            }
            set
            {
                Set<ObservableCollection<BitmapImage>>(() => this.Thumbnails, ref _thumbnails, value);
            }
        }

        private ObservableCollection<LocalMusicModel> _musics;
        public ObservableCollection<LocalMusicModel> Musics
        {
            get
            {
                return _musics;
            }
            set
            {
                Set<ObservableCollection<LocalMusicModel>>(() => this.Musics, ref _musics, value);
            }
        }

        private ObservableCollection<LocalAlbumModel> _albums;
        public ObservableCollection<LocalAlbumModel> Albums
        {
            get
            {
                return _albums;
            }
            set
            {
                Set<ObservableCollection<LocalAlbumModel>>(() => this.Albums, ref _albums, value);
            }
        }

        public async Task GetLocalMusics()
        {
            foreach(Music music in Library.musics)
            {
                var localmusic = new LocalMusicModel(music);
                await localmusic.GetThumbnail();
                await localmusic.GetStorageFile();
                Musics.Add(localmusic);
            }
        }

        public async Task GetLocalAlbums()
        {
            foreach (Album album in Library.albums)
            {
                var localalbum = new LocalAlbumModel(album);
                await localalbum.GetThumbanail();
                Albums.Add(localalbum);
            }
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

        public async Task GetThumbnails()
        {
            foreach(Thumbnail thumbnail in Library.thumbnails)
            {
                var thumb = await Utility.ImageFromBytes(thumbnail.Image);
                Thumbnails.Add(thumb);
            }
        }


        
        
    }
}