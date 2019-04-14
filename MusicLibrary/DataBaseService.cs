using Microsoft.EntityFrameworkCore;
using MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using TagLib;
using Windows.Storage;

namespace MusicLibraryService
{
    static class DataBaseService
    {
        public static void Initialize()
        {
            using (var db = new MusicLibraryDbContext())
            {
                db.Database.Migrate();
            }
        }

        public static async Task Add(StorageFile File)
        {
            try
            {
                Debug.WriteLine(File.Name + " Music meta data start retreiving");
                var fileStream = await File.OpenStreamForReadAsync();

                var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name,
                                 fileStream, fileStream));

                var types = tagFile.TagTypes;

                if ((types & (TagTypes.Id3v1 | TagTypes.Id3v2)) == (TagTypes.Id3v1 | TagTypes.Id3v2))
                {
                    types = TagTypes.Id3v2;
                }

                var tags = tagFile.GetTag(types);


                Thumbnail Thumbnail = new Thumbnail
                {
                    Image = tags.Pictures.Length == 0 ? new byte[] { } : tags.Pictures[0].Data.Data
                };
                Debug.WriteLine("Thumbnail Succeed");
                Artist Artist = new Artist
                {
                    Name = tags.Performers.Length == 0 ? "Unknown Artist" : tags.Performers[0]
                };
                Debug.WriteLine("Artist Succeed");
                Album Album = new Album
                {
                    Title = tags.Album != null ? tags.Album : "Unknown Album",
                    ArtistId = Artist.Id
                };
                Debug.WriteLine("Album Succeed");
                Music Music = new Music
                {
                    Path = File.Path,
                    Title = tags.Title == null ? Path.GetFileNameWithoutExtension(File.Path) : tags.Title,
                    AlbumId = Album.Id,
                    ArtistId = Artist.Id,
                    ThumbnailId = Thumbnail.Id
                };
                Debug.WriteLine("Music Succeed");

                Debug.WriteLine("Insert into database");
                using (var _context = new MusicLibraryDbContext())
                {
                    _context.Thumbnails.Add(Thumbnail);
                    _context.SaveChanges();
                    _context.Artists.Add(Artist);
                    _context.SaveChanges();
                    _context.Albums.Add(Album);
                    _context.SaveChanges();
                    _context.Musics.Add(Music);
                    _context.SaveChanges();
                }
                Debug.WriteLine("DataBase Succeed");
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public static void Delete(string path)
        {
            try
            {
                using (var _context = new MusicLibraryDbContext())
                {
                    var music = _context.Musics.Where(m => m.Path == path).First();
                    _context.Musics.Remove(music);
                    _context.SaveChanges();
                }
            }
            catch(Exception e)
            {

            }
        }

        public static async Task Update(StorageFile File)
        {
            try
            {
                Debug.WriteLine(File.Name + " Music meta data start retreiving");
                var fileStream = await File.OpenStreamForReadAsync();

                var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name,
                                 fileStream, fileStream));

                var types = tagFile.TagTypes;

                if ((types & (TagTypes.Id3v1 | TagTypes.Id3v2)) == (TagTypes.Id3v1 | TagTypes.Id3v2))
                {
                    types = TagTypes.Id3v2;
                }

                var tags = tagFile.GetTag(types);

                byte[] imageBytes = tags.Pictures.Length == 0 ? new byte[] { } : tags.Pictures[0].Data.Data;
                string artistname = tags.Performers.Length == 0 ? "Unknown Artist" : tags.Performers[0];
                string AlbumTitle = tags.Album != null ? tags.Album : "Unknown Album";
                string path = File.Path;
                string Title = tags.Title == null ? Path.GetFileNameWithoutExtension(File.Path) : tags.Title;

                Debug.WriteLine("Access into database");
                using (var _context = new MusicLibraryDbContext())
                {
                    Music music = _context.Musics.Where(m => m.Path == path).First();
                    Thumbnail thumbnail = _context.Thumbnails.Where(t => t.Id == music.ThumbnailId).First();
                    Artist artist = _context.Artists.Where(a => a.Id == music.ArtistId).First();
                    Album album = _context.Albums.Where(a => a.Id == music.AlbumId).First();

                    if(music.Title != Title)
                    {
                        music.Title = Title;
                        _context.Musics.Update(music);
                    }
                    if(thumbnail.Image != imageBytes)
                    {
                        thumbnail.Image = imageBytes;
                        _context.Thumbnails.Update(thumbnail);
                    }
                    if(album.Title != AlbumTitle)
                    {
                        album.Title = AlbumTitle;
                        _context.Albums.Update(album);
                    }
                    if(artist.Name != artistname)
                    {
                        artist.Name = artistname;
                        _context.Artists.Update(artist);
                    }
                    _context.SaveChanges();
                }
                Debug.WriteLine("Update Succeed");
            }
            catch(Exception e)
            {

            }
        }

        public static void Update(KeyValuePair<string, string> pathChange)
        {
            Debug.WriteLine("Access into database");
            using (var _context = new MusicLibraryDbContext())
            {
                Music music = _context.Musics.Where(m => m.Path == pathChange.Key).First();
                music.Path = pathChange.Value;
                _context.Musics.Update(music);
                _context.SaveChanges();
            }
            Debug.WriteLine("Update Succeed");
        }

        public static LibraryViewModel FetchAll()
        {
            LibraryViewModel viewModel = new LibraryViewModel();
            using(var _context = new MusicLibraryDbContext())
            {
                var musics = _context.Musics.Select(m => m).ToList();
                var albums = _context.Albums.Select(m => m).ToList();
                var artists = _context.Artists.Select(m => m).ToList();
                var musicplaylists = _context.MusicInPlaylists.Select(m => m).ToList();
                var playlists = _context.Playlists.Select(m => m).ToList();
                foreach(Artist artist in artists)
                {
                    artist.Albums = albums.Where(a => a.ArtistId == artist.Id).ToList();
                    artist.Musics = musics.Where(m => m.ArtistId == artist.Id).ToList();
                }
                foreach(Album album in albums)
                {
                    album.Artist = artists.Where(a => a.Id == album.ArtistId).First();
                    album.Musics = musics.Where(m => m.AlbumId == album.Id).ToList();
                }
                foreach(Music music in musics)
                {
                    music.Album = albums.Where(a => a.Id == music.AlbumId).First();
                    music.Artist = artists.Where(a => a.Id == music.ArtistId).First();
                }
                foreach(Playlist pl in playlists)
                {
                    var something = musicplaylists.Where(m => m.PlaylistId == pl.Id).Select(m => m.MusicId);
                    var musicsinList = something.SelectMany(s => musics.Where(m => m.Id == s)).ToList();
                    pl.Musics = musicsinList;
                }
                viewModel.musics = musics;
                viewModel.albums = albums;
                viewModel.artists = artists;
                viewModel.playlists = playlists;
            }
            return viewModel;
        }
    }

    public class LibraryViewModel
    {
        public List<Music> musics { get; set; }
        public List<Album> albums { get; set; }
        public List<Artist> artists { get; set; }
        public List<Playlist> playlists { get; set; }

    }
}
