using Microsoft.EntityFrameworkCore;
using MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TagLib;
using Windows.Storage;

namespace MusicLibraryService
{
    //static class DataBaseService
    //{
    //    public static void Initialize()
    //    {
    //        try
    //        {
    //            using (var db = new MusicLibraryDbContext())
    //            {
    //                db.Database.Migrate();
    //            }
    //        }
    //        catch(Exception e)
    //        {
    //            Debug.WriteLine(e.ToString());
    //        }
            
    //    }

    //    public static string ByteArrayToString(byte[] ba)
    //    {
    //        StringBuilder hex = new StringBuilder(ba.Length * 2);
    //        foreach (byte b in ba)
    //            hex.AppendFormat("{0:x2}", b);
    //        return hex.ToString();
    //    }

    //    public static async Task Add(StorageFile File)
    //    {
    //        try
    //        {
    //            Debug.WriteLine(File.Name + " Music meta data start retreiving");
    //            var fileStream = await File.OpenStreamForReadAsync();

    //            var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name,
    //                             fileStream, fileStream));

    //            var types = tagFile.TagTypes;

    //            if ((types & (TagTypes.Id3v1 | TagTypes.Id3v2)) == (TagTypes.Id3v1 | TagTypes.Id3v2))
    //            {
    //                types = TagTypes.Id3v2;
    //            }

    //            var tags = tagFile.GetTag(types);

    //            var bytearray = tags.Pictures.Length == 0 ? new byte[] { } : tags.Pictures[0].Data.Data;
    //            var artistname = tags.Performers.Length == 0 ? "Unknown Artist" : tags.Performers[0];
    //            var albumtitle = tags.Album != null ? tags.Album : "Unknown Album";

    //            using (var _context = new MusicLibraryDbContext())
    //            {
    //                var thumb = _context.Thumbnails.Where(m => m.Image == bytearray).ToList();
    //                if(thumb.Count == 0)
    //                {
    //                    thumb.Add(new Thumbnail
    //                    {
    //                        Image = bytearray
    //                    });
    //                    _context.Thumbnails.Add(thumb[0]);
    //                    _context.SaveChanges();
    //                }
    //                Debug.WriteLine("Thumbnail Done");

    //                var artist = _context.Artists.Where(a => a.Name == artistname).ToList();
    //                if(artist.Count == 0)
    //                {
    //                    artist.Add(new Artist
    //                    {
    //                        Name = artistname
    //                    });
    //                    _context.Artists.Add(artist[0]);
    //                    _context.SaveChanges();
    //                }
    //                Debug.WriteLine("Artist Done");

    //                var album = _context.Albums.Where(a => a.Title == albumtitle).ToList();
    //                if(album.Count == 0)
    //                {
    //                    album.Add(new Album
    //                    {
    //                        Title = albumtitle,
    //                        ArtistId = artist[0].Id,
    //                        ThumbnailId = thumb[0].Id
    //                    });
    //                    _context.Albums.Add(album[0]);
    //                    _context.SaveChanges();
    //                }
    //                Debug.WriteLine("Album Done");

    //                Music Music = new Music
    //                {
    //                    Path = File.Path,
    //                    Title = tags.Title == null ? Path.GetFileNameWithoutExtension(File.Path) : tags.Title,
    //                    AlbumId = album[0].Id,
    //                    ArtistId = artist[0].Id,
    //                    ThumbnailId = thumb[0].Id
    //                };
    //                _context.Musics.Add(Music);
    //                _context.SaveChanges();
    //                Debug.WriteLine("Music Done");
    //            }
    //            Debug.WriteLine("DataBase Succeed");
    //        }
    //        catch(Exception e)
    //        {
    //            Debug.WriteLine(e.ToString());
    //        }
    //    }

    //    public static void Delete(string path)
    //    {
    //        try
    //        {
    //            using (var _context = new MusicLibraryDbContext())
    //            {
    //                var music = _context.Musics.Where(m => m.Path == path).First();
    //                _context.Musics.Remove(music);
    //                _context.SaveChanges();
    //            }
    //        }
    //        catch(Exception e)
    //        {
    //            Debug.WriteLine(e.ToString());
    //        }
    //    }

    //    public static async Task Update(StorageFile File)
    //    {
    //        try
    //        {
    //            Debug.WriteLine(File.Name + " Music meta data start retreiving");
    //            var fileStream = await File.OpenStreamForReadAsync();

    //            var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name,
    //                             fileStream, fileStream));

    //            var types = tagFile.TagTypes;

    //            if ((types & (TagTypes.Id3v1 | TagTypes.Id3v2)) == (TagTypes.Id3v1 | TagTypes.Id3v2))
    //            {
    //                types = TagTypes.Id3v2;
    //            }

    //            var tags = tagFile.GetTag(types);

    //            byte[] imageBytes = tags.Pictures.Length == 0 ? new byte[] { } : tags.Pictures[0].Data.Data;
    //            string artistname = tags.Performers.Length == 0 ? "Unknown Artist" : tags.Performers[0];
    //            string AlbumTitle = tags.Album != null ? tags.Album : "Unknown Album";
    //            string path = File.Path;
    //            string Title = tags.Title == null ? Path.GetFileNameWithoutExtension(File.Path) : tags.Title;

    //            Debug.WriteLine("Access into database");
    //            using (var _context = new MusicLibraryDbContext())
    //            {
    //                Music music = _context.Musics.Where(m => m.Path == path).First();
    //                Thumbnail thumbnail = _context.Thumbnails.Where(t => t.Id == music.ThumbnailId).First();
    //                Artist artist = _context.Artists.Where(a => a.Id == music.ArtistId).First();
    //                Album album = _context.Albums.Where(a => a.Id == music.AlbumId).First();

    //                if(music.Title != Title)
    //                {
    //                    music.Title = Title;
    //                    _context.Musics.Update(music);
    //                }
    //                if(thumbnail.Image != imageBytes)
    //                {
    //                    thumbnail.Image = imageBytes;
    //                    _context.Thumbnails.Update(thumbnail);
    //                }
    //                if(album.Title != AlbumTitle)
    //                {
    //                    album.Title = AlbumTitle;
    //                    _context.Albums.Update(album);
    //                }
    //                if(artist.Name != artistname)
    //                {
    //                    artist.Name = artistname;
    //                    _context.Artists.Update(artist);
    //                }
    //                _context.SaveChanges();
    //            }
    //            Debug.WriteLine("Update Succeed");
    //        }
    //        catch(Exception e)
    //        {
    //            Debug.WriteLine(e.ToString());
    //        }
    //    }

    //    public static void Update(KeyValuePair<string, string> pathChange)
    //    {
    //        try
    //        {
    //            Debug.WriteLine("Access into database");
    //            using (var _context = new MusicLibraryDbContext())
    //            {
    //                Music music = _context.Musics.Where(m => m.Path == pathChange.Key).First();
    //                music.Path = pathChange.Value;
    //                _context.Musics.Update(music);
    //                _context.SaveChanges();
    //            }
    //            Debug.WriteLine("Update Succeed");
    //        }
    //        catch(Exception e)
    //        {
    //            Debug.WriteLine(e.ToString());
    //        }
    //    }

    //    public static Library FetchAll()
    //    {
    //        Library viewModel = new Library();
    //        try
    //        {
    //            using (var _context = new MusicLibraryDbContext())
    //            {
    //                var musics = _context.Musics.Select(m => m).ToList();
    //                var albums = _context.Albums.Select(a => a).ToList();
    //                var artists = _context.Artists.Select(a => a).ToList();
    //                var musicplaylists = _context.MusicInPlaylists.Select(m => m).ToList();
    //                var playlists = _context.Playlists.Select(p => p).ToList();
    //                var thumbnails = _context.Thumbnails.Select(t => t).ToList();
    //                viewModel.musics = musics;
    //                viewModel.albums = albums;
    //                viewModel.artists = artists;
    //                viewModel.playlists = playlists;
    //                viewModel.thumbnails = thumbnails;
    //            }
    //            return viewModel;
    //        }
    //        catch(Exception e)
    //        {
    //            Debug.WriteLine(e.ToString());
    //            return viewModel;
    //        }
    //    }

    //    public static void CreatePlaylist(string playlistName)
    //    {
    //        Playlist playlist = new Playlist
    //        {
    //            Name = playlistName
    //        };
    //        using (var _context = new MusicLibraryDbContext())
    //        {
    //            _context.Playlists.Add(playlist);
    //            _context.SaveChanges();
    //        }
    //    }

    //    public static void EditPlaylist(string oldPlaylistName, string newPlaylistName)
    //    {
    //        using(var _context = new MusicLibraryDbContext())
    //        {
    //            var playlist = _context.Playlists.Where(p => p.Name == oldPlaylistName).First();
    //            playlist.Name = newPlaylistName;
    //            _context.Playlists.Update(playlist);
    //            _context.SaveChanges();
    //        }
    //    }

    //    public static void DelPlaylist(string playlistName)
    //    {
    //        using(var _context = new MusicLibraryDbContext())
    //        {
    //            var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
    //            _context.Playlists.Remove(playlist);
    //            _context.SaveChanges();
    //        }
    //    }

    //    public static void AddSongsToPlaylist(string playlistName, List<Music> musics)
    //    {
    //        using(var _context = new MusicLibraryDbContext())
    //        {
    //            var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
    //            foreach(var item in musics)
    //            {
    //                var mInP = new MusicInPlaylist
    //                {
    //                    Music = item,
    //                    MusicId = item.Id,
    //                    Playlist = playlist,
    //                    PlaylistId = playlist.Id
    //                };
    //                _context.MusicInPlaylists.Add(mInP);
    //            }
    //            _context.SaveChanges();
    //        }
    //    }

    //    public static void RemoveSongsFromPlaylist(string playlistName, List<Music> musics)
    //    {
    //        using(var _context = new MusicLibraryDbContext())
    //        {
    //            var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
    //            foreach(var item in musics)
    //            {
    //                var mInP = _context.MusicInPlaylists.Where(m => m.PlaylistId == playlist.Id && m.MusicId == item.Id).First();
    //                _context.MusicInPlaylists.Remove(mInP);
    //            }
    //            _context.SaveChanges();
    //        }
    //    }
    //}

    //public class Library
    //{
    //    public List<Music> musics { get; set; }
    //    public List<Album> albums { get; set; }
    //    public List<Artist> artists { get; set; }
    //    public List<Playlist> playlists { get; set; }
    //    public List<Thumbnail> thumbnails { get; set; }

    //}
}
