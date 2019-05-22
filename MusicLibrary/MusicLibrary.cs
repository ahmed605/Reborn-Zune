using Microsoft.EntityFrameworkCore;
using Microsoft.Toolkit.Uwp.Helpers;
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
using Windows.Storage.Search;

namespace MusicLibraryService
{
    public static class MusicLibrary
    {
        public static event EventHandler InitializeFinished;
        public static event EventHandler FetchAllFinished;
        public static async void Initialize(bool IsFirstUse)
        {
            try
            {
                DataBaseService.Initialize();
                var result = await LibraryService.Initialize(IsFirstUse);
                foreach (var i in result)
                {
                    if (i.Value.GetType().Name == "StorageFile") //Add/Update DataBase
                    {
                        Debug.WriteLine("StorageFile");
                        if (i.Key == StorageLibraryChangeType.ContentsChanged)
                        {
                            Debug.WriteLine("ContentChanged");
                            await DataBaseService.Update(i.Value as StorageFile);
                        }
                        else if (i.Key == StorageLibraryChangeType.MovedIntoLibrary)
                        {
                            Debug.WriteLine("MovedIntoLibrary");
                            await DataBaseService.Add((StorageFile)i.Value);
                        }

                    }
                    else if (i.Value.GetType().Name == "String") //Moved Out
                    {
                        Debug.WriteLine("Move out");
                        DataBaseService.Delete(i.Value.ToString());
                    }
                    else if (i.Value.GetType().Name == "KeyValuePair`2") //Moved or Renamed
                    {
                        Debug.WriteLine("Moved or Renamed");
                        DataBaseService.Update((KeyValuePair<string, string>)i.Value);
                    }
                }

                InitializeFinished?.Invoke(null, EventArgs.Empty);
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public static void AddSongsToPlaylist(string v, List<Music> musics)
        {
            DataBaseService.AddSongsToPlaylist(v, musics);
        }

        public static Library FetchAll()
        {
            try
            {
                var result = DataBaseService.FetchAll();
                FetchAllFinished?.Invoke(null, EventArgs.Empty);
                return result;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.ToString());
                return new Library();
            }
        }

        public static async void AddSong(StorageFile file)
        {
            await DataBaseService.Add(file);
        }

        public static void DeleteSong(string filepath)
        {
            DataBaseService.Delete(filepath);
        }

        public static void CreatePlaylist(string playlistName)
        {
            if(DataBaseService.CheckPlaylistName(playlistName))
                DataBaseService.CreatePlaylist(playlistName);
        }
    }

    static class DataBaseService
    {
        public static void Initialize()
        {
            try
            {
                using (var db = new MusicLibraryDbContext())
                {
                    db.Database.Migrate();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }

        public static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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

                var bytearray = tags.Pictures.Length == 0 ? new byte[] { } : tags.Pictures[0].Data.Data;
                var artistname = tags.Performers.Length == 0 ? "Unknown Artist" : tags.Performers[0];
                var albumtitle = tags.Album != null ? tags.Album : "Unknown Album";

                using (var _context = new MusicLibraryDbContext())
                {
                    var thumb = _context.Thumbnails.Where(m => m.Image == bytearray).ToList();
                    if (thumb.Count == 0)
                    {
                        thumb.Add(new Thumbnail
                        {
                            Image = bytearray
                        });
                        _context.Thumbnails.Add(thumb[0]);
                        _context.SaveChanges();
                    }
                    Debug.WriteLine("Thumbnail Done");

                    var artist = _context.Artists.Where(a => a.Name == artistname).ToList();
                    if (artist.Count == 0)
                    {
                        artist.Add(new Artist
                        {
                            Name = artistname
                        });
                        _context.Artists.Add(artist[0]);
                        _context.SaveChanges();
                    }
                    Debug.WriteLine("Artist Done");

                    var album = _context.Albums.Where(a => a.Title == albumtitle).ToList();
                    if (album.Count == 0)
                    {
                        album.Add(new Album
                        {
                            Title = albumtitle,
                            ArtistId = artist[0].Id,
                            ThumbnailId = thumb[0].Id
                        });
                        _context.Albums.Add(album[0]);
                        _context.SaveChanges();
                    }
                    Debug.WriteLine("Album Done");

                    Music Music = new Music
                    {
                        Path = File.Path,
                        Title = tags.Title == null ? Path.GetFileNameWithoutExtension(File.Path) : tags.Title,
                        AlbumId = album[0].Id,
                        ArtistId = artist[0].Id,
                        ThumbnailId = thumb[0].Id
                    };
                    _context.Musics.Add(Music);
                    _context.SaveChanges();
                    Debug.WriteLine("Music Done");
                }
                Debug.WriteLine("DataBase Succeed");
            }
            catch (Exception e)
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
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
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

                    if (music.Title != Title)
                    {
                        music.Title = Title;
                        _context.Musics.Update(music);
                    }
                    if (thumbnail.Image != imageBytes)
                    {
                        thumbnail.Image = imageBytes;
                        _context.Thumbnails.Update(thumbnail);
                    }
                    if (album.Title != AlbumTitle)
                    {
                        album.Title = AlbumTitle;
                        _context.Albums.Update(album);
                    }
                    if (artist.Name != artistname)
                    {
                        artist.Name = artistname;
                        _context.Artists.Update(artist);
                    }
                    _context.SaveChanges();
                }
                Debug.WriteLine("Update Succeed");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public static void Update(KeyValuePair<string, string> pathChange)
        {
            try
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
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }
        }

        public static Library FetchAll()
        {
            Library viewModel = new Library();
            try
            {
                using (var _context = new MusicLibraryDbContext())
                {
                    var musics = _context.Musics.Select(m => m).ToList();
                    var albums = _context.Albums.Select(a => a).ToList();
                    var artists = _context.Artists.Select(a => a).ToList();
                    var musicplaylists = _context.MusicInPlaylists.Select(m => m).ToList();
                    var playlists = _context.Playlists.Select(p => p).ToList();
                    var thumbnails = _context.Thumbnails.Select(t => t).ToList();
                    viewModel.musics = musics;
                    viewModel.albums = albums;
                    viewModel.artists = artists;
                    viewModel.playlists = playlists;
                    viewModel.thumbnails = thumbnails;
                    viewModel.mInP = musicplaylists;
                }
                return viewModel;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return viewModel;
            }
        }

        public static void CreatePlaylist(string playlistName)
        {
            Playlist playlist = new Playlist
            {
                Name = playlistName
            };
            using (var _context = new MusicLibraryDbContext())
            {
                _context.Playlists.Add(playlist);
                _context.SaveChanges();
            }
        }

        public static void EditPlaylist(string oldPlaylistName, string newPlaylistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == oldPlaylistName).First();
                playlist.Name = newPlaylistName;
                _context.Playlists.Update(playlist);
                _context.SaveChanges();
            }
        }

        public static void DelPlaylist(string playlistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
                _context.Playlists.Remove(playlist);
                _context.SaveChanges();
            }
        }

        public static void AddSongsToPlaylist(string playlistName, List<Music> musics)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
                foreach (var item in musics)
                {
                    var mInP = new MusicInPlaylist
                    {
                        Music = item,
                        MusicId = item.Id,
                        Playlist = playlist,
                        PlaylistId = playlist.Id
                    };
                    _context.MusicInPlaylists.Add(mInP);
                    _context.SaveChanges();
                }
                
            }
        }

        public static void RemoveSongsFromPlaylist(string playlistName, List<Music> musics)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Name == playlistName).First();
                foreach (var item in musics)
                {
                    var mInP = _context.MusicInPlaylists.Where(m => m.PlaylistId == playlist.Id && m.MusicId == item.Id).First();
                    _context.MusicInPlaylists.Remove(mInP);
                }
                _context.SaveChanges();
            }
        }

        public static bool CheckPlaylistName(string playlistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return (_context.Playlists.Where(p => p.Name == playlistName).Count() == 0);
            }
        }
    }

    static class LibraryService
    {
        public static async Task<List<KeyValuePair<StorageLibraryChangeType, object>>> Initialize(bool IsFirstUse)
        {
            if (IsFirstUse)
            {
                return await LoadLibrary();
            }
            else
            {
                return await LoadChanges();
            }
        }

        private static async Task<List<KeyValuePair<StorageLibraryChangeType, object>>> LoadLibrary()
        {
            List<KeyValuePair<StorageLibraryChangeType, object>> changes = new List<KeyValuePair<StorageLibraryChangeType, object>>();
            try
            {
                QueryOptions queryOption = new QueryOptions
                (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".m4a", ".mp4" });

                queryOption.FolderDepth = FolderDepth.Deep;

                Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

                IReadOnlyList<StorageFile> files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
                  (queryOption).GetFilesAsync();

                foreach (StorageFile file in files)
                {
                    changes.Add(
                        new KeyValuePair<StorageLibraryChangeType, object>(StorageLibraryChangeType.MovedIntoLibrary, file)
                    );
                }


                StorageLibrary musicsLib = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
                StorageLibraryChangeTracker musicTracker = musicsLib.ChangeTracker;
                musicTracker.Enable();

                Debug.WriteLine("Get songs succeed");
                return changes;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return changes;
            }

        }

        private static async Task<List<KeyValuePair<StorageLibraryChangeType, object>>> LoadChanges()
        {
            List<KeyValuePair<StorageLibraryChangeType, object>> changes = new List<KeyValuePair<StorageLibraryChangeType, object>>();
            try
            {
                StorageLibrary musicLibray = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
                musicLibray.ChangeTracker.Enable();
                StorageLibraryChangeReader musicChangeReader = musicLibray.ChangeTracker.GetChangeReader();
                IReadOnlyList<StorageLibraryChange> changeSet = await musicChangeReader.ReadBatchAsync();


                //Below this line is for the blog post. Above the line is for the magazine
                foreach (StorageLibraryChange change in changeSet)
                {
                    if (change.ChangeType == StorageLibraryChangeType.ChangeTrackingLost)
                    {
                        //We are in trouble. Nothing else is going to be valid.
                        Debug.WriteLine("Tracking lost");
                        musicLibray.ChangeTracker.Reset();
                        return changes;
                    }
                    if (change.IsOfType(StorageItemTypes.Folder))
                    {
                        Debug.WriteLine("Folder changes detected");
                    }
                    else if (change.IsOfType(StorageItemTypes.File))
                    {
                        Debug.WriteLine("File changes detected");
                        switch (change.ChangeType)
                        {
                            case StorageLibraryChangeType.ContentsChanged:
                                StorageFile file = await change.GetStorageItemAsync() as StorageFile;
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType, file));
                                break;
                            case StorageLibraryChangeType.MovedOrRenamed:
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType,
                                    new KeyValuePair<string, string>(change.Path, change.PreviousPath)));
                                break;
                            case StorageLibraryChangeType.MovedOutOfLibrary:
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType, change.Path));
                                break;
                            case StorageLibraryChangeType.MovedIntoLibrary:
                                StorageFile File = await change.GetStorageItemAsync() as StorageFile;
                                changes.Add(new KeyValuePair<StorageLibraryChangeType, object>(change.ChangeType, File));
                                break;
                        }
                    }
                }
                await musicChangeReader.AcceptChangesAsync();
                Debug.WriteLine("Get changes succeed");
                return changes;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return changes;
            }

        }
    }
    public class Library
    {
        public List<Music> musics { get; set; }
        public List<Album> albums { get; set; }
        public List<Artist> artists { get; set; }
        public List<Playlist> playlists { get; set; }
        public List<Thumbnail> thumbnails { get; set; }
        public List<MusicInPlaylist> mInP { get; set; }
    }
}
