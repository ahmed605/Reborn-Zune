using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using MusicLibraryService;
using Reborn_Zune.Model;
using Reborn_Zune.Utilities;
using Windows.UI.Xaml;
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
            Playlists = new ObservableCollection<LocalPlaylistModel>();
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
                GetLocalPlaylists();
                RaisePropertyChanged(nameof(hasPlaylistReverse));
                RaisePropertyChanged(nameof(hasPlaylist));
                InitializeFinished?.Invoke(this, EventArgs.Empty);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
            
        }

        private void GetLocalPlaylists()
        {

            foreach(var item in Library.playlists)
            {
                var final = Musics.Where(a => Library.mInP.Where(m => m.PlaylistId == item.Id).Select(m => m.MusicId).Any(b => b == a.Music.Id)).ToList();
                var local = new LocalPlaylistModel
                {
                    Playlist = item,
                    Musics = new ObservableCollection<LocalMusicModel>(final)
                };
                Playlists.Add(local);

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

        private ObservableCollection<LocalPlaylistModel> _playlists;
        public ObservableCollection<LocalPlaylistModel> Playlists
        {
            get
            {
                return _playlists;
            }
            set
            {
                Set<ObservableCollection<LocalPlaylistModel>>(() => this.Playlists, ref _playlists, value);
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

        public void CreatePlaylist(string text)
        {
            MusicLibrary.CreatePlaylist(text);
            Library = MusicLibrary.FetchAll();
            GetLocalPlaylists();
            RaisePropertyChanged(nameof(hasPlaylistReverse));
            RaisePropertyChanged(nameof(hasPlaylist));
        }


        public Visibility hasPlaylist
        {
            get
            {
                return Playlists.Count == 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public Visibility hasPlaylistReverse
        {
            get
            {
                return Playlists.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
        }
    }
}