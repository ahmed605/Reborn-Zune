using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
            Artists = new ObservableCollection<LocalArtistModel>();
            Library = MusicLibrary.FetchAll();
            Finialize();
        }

        private async void Finialize()
        {
            try
            {
                await GetLocalMusics();
                await GetLocalAlbums();
                await GetThumbnails();
                GetLocalArtists();
                InitializeFinished?.Invoke(this, EventArgs.Empty);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            
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

        private ObservableCollection<LocalArtistModel> _artists;

        public ObservableCollection<LocalArtistModel> Artists
        {
            get
            {
                return _artists;
            }
            set
            {
                Set<ObservableCollection<LocalArtistModel>>(() => this.Artists, ref _artists, value);
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
                var localalbum = new LocalAlbumModel(album, Musics);
                await localalbum.GetThumbanail();
                Albums.Add(localalbum);
            }
        }

        public void GetLocalArtists()
        {
            foreach(Artist artist in Library.artists)
            {
                Artists.Add(new LocalArtistModel(artist, Albums));
            }
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