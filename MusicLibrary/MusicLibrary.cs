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
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Search;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicLibraryService
{
    public static class MusicLibrary
    {
        public static Library Library { get; set; }
        public static event EventHandler InitializeFinished;
        public static event EventHandler FetchSucceed;
        
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

        public static async Task FetchLibrary()
        {
            try
            {
                var library = DataBaseService.FetchAll();
                library.RenderThumbnail();
                await library.GetFiles();
                Library = library;
                FetchSucceed?.Invoke(Library, EventArgs.Empty);
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

        public static bool CreatePlaylist(string playlistName)
        {
            if (!DataBaseService.PlaylistNameAvailable(playlistName))
            {
                return false;
            }
            else
            {
                DataBaseService.CreatePlaylist(playlistName);
                return true;
            }
        }

        public static Library FetchPlaylist()
        {
            var library = DataBaseService.FetchAll();
            return library;
        }
    }

    static class DataBaseService
    {
        private const string UNKNOWN_ARTIST = "Unknown Artist";
        private const string UNKNOWN_ALBUM = "Unknown Album";
        private const string UNKNOWN_YEAR = "Unknown Year";

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

        public static async Task Add(StorageFile File)
        {
            try
            {
                Debug.WriteLine(File.Name + " Music meta data start retreiving");


                var thumbnail = await File.GetThumbnailAsync(ThumbnailMode.MusicView, 100, ThumbnailOptions.ReturnOnlyIfCached);
                var properties = await File.Properties.GetMusicPropertiesAsync();

                var path = File.Path;
                var bytearray = await ConvertThumbnailToBytesAsync(thumbnail);
                var artistName = properties.Artist != "" ? properties.Artist : UNKNOWN_ARTIST;
                var albumArtistName = properties.AlbumArtist != "" ? properties.AlbumArtist : UNKNOWN_ARTIST;
                var albumTitle = properties.Album != "" ? properties.Album : UNKNOWN_ALBUM;
                var duration = properties.Duration.ToString(@"mm\:ss");
                var albumYear = properties.Year != 0 ? properties.Year.ToString() : UNKNOWN_YEAR;
                var songTitle = properties.Title != "" ? properties.Title : Path.GetFileNameWithoutExtension(File.Path);

                using (var _context = new MusicLibraryDbContext())
                {
                    var thumb = new Thumbnail
                    {
                        ImageBytes = bytearray,
                        Id = Guid.NewGuid().ToString()
                    };
                    _context.Thumbnails.Add(thumb);
                    _context.SaveChanges();
                    Debug.WriteLine("Thumbnail Done");

                    Music Music = new Music
                    {
                        Path = path,
                        Title = songTitle,
                        AlbumTitle = albumTitle,
                        Artist = artistName,
                        AlbumArtist = albumArtistName,
                        Year = albumYear,
                        ThumbnailId = thumb.Id,
                        Duration = duration,
                        
                        Id = Guid.NewGuid().ToString()
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

        private static async Task<byte[]> ConvertThumbnailToBytesAsync(StorageItemThumbnail thumbnail)
        {
            if(thumbnail == null)
            {
                return new byte[0];
            }
            byte[] result = new byte[thumbnail.Size];
            using(var reader = new DataReader(thumbnail))
            {
                await reader.LoadAsync((uint)thumbnail.Size);
                reader.ReadBytes(result);
                return result;
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
                var properties = await File.Properties.GetMusicPropertiesAsync();
                var fileStream = await File.OpenStreamForReadAsync();

                var tagFile = TagLib.File.Create(new StreamFileAbstraction(File.Name,
                                 fileStream, fileStream));

                var types = tagFile.TagTypes;

                if ((types & (TagTypes.Id3v1 | TagTypes.Id3v2)) == (TagTypes.Id3v1 | TagTypes.Id3v2))
                {
                    types = TagTypes.Id3v2;
                }

                var tags = tagFile.GetTag(types);

                var path = File.Path;
                var bytearray = tags.Pictures.Length == 0 ? new byte[] { } : tags.Pictures[0].Data.Data;
                var artistName = properties.Artist != "" ? properties.AlbumArtist : UNKNOWN_ARTIST;
                var albumArtistName = properties.AlbumArtist != null ? properties.AlbumArtist : UNKNOWN_ARTIST;
                var albumTitle = properties.Album != "" ? properties.Album : UNKNOWN_ALBUM;
                var duration = properties.Duration.ToString(@"mm\:ss");
                var albumYear = properties.Year != 0 ? properties.Year.ToString() : UNKNOWN_YEAR;
                var songTitle = properties.Title != "" ? properties.Title : Path.GetFileNameWithoutExtension(File.Path);

                Debug.WriteLine("Access into database");
                using (var _context = new MusicLibraryDbContext())
                {
                    Music music = _context.Musics.Where(m => m.Path == path).First();
                    Thumbnail thumbnail = _context.Thumbnails.Where(t => t.Id == music.ThumbnailId).First();

                    music.Artist = artistName;
                    music.AlbumArtist = albumArtistName;
                    music.AlbumTitle = albumTitle;
                    music.Title = songTitle;
                    music.Year = albumYear;
                    music.Duration = duration;

                    _context.Musics.Update(music);

                    if (thumbnail.ImageBytes != bytearray)
                    {
                        thumbnail.ImageBytes = bytearray;
                        _context.Thumbnails.Update(thumbnail);
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
            Library library = new Library();
            try
            {
                using (var _context = new MusicLibraryDbContext())
                {
                    library._musics = _context.Musics.Select(m => m).ToList();
                    library._mInP = _context.MusicInPlaylists.Select(m => m).ToList();
                    library._playlists = _context.Playlists.Select(p => p).ToList();
                    library._thumbnails = _context.Thumbnails.Select(t => t).ToList();
                }
                return library;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                return library;
            }
        }

        public static void CreatePlaylist(string playlistName)
        {
            Playlist playlist = new Playlist
            {
                Name = playlistName,
                Id = Guid.NewGuid().ToString()
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

        public static void DeletePlaylist(string playlistName)
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
                        MusicId = item.Id,
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

        public static bool PlaylistNameAvailable(string playlistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return (_context.Playlists.Where(p => p.Name == playlistName).FirstOrDefault() == null);
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
        public List<Music> _musics { get; set; }
        public List<Playlist> _playlists { get; set; }
        public List<Thumbnail> _thumbnails { get; set; }
        public List<MusicInPlaylist> _mInP { get; set; }

        public void RenderThumbnail()
        {
           foreach(var item in _thumbnails)
            {
                item.GetBitmapImage();
            }
        }

        public async Task GetFiles()
        {
            foreach(var song in _musics)
            {
                await GetFileAsync(song);
            }
        }

        private async Task GetFileAsync(Music song)
        {
            song.File = await StorageFile.GetFileFromPathAsync(song.Path);
        }
    }
}
