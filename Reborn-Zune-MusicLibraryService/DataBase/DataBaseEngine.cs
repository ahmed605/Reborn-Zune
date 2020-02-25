using Microsoft.EntityFrameworkCore;
using Reborn_Zune_MusicLibraryEFCoreModel;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicLibraryService.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicLibraryService.DataBase
{
    static class DataBaseEngine
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

        #region Normal Database Operation
        public static async Task Sync(IReadOnlyList<StorageFile> files)
        {
            try
            {
                using (var _context = new MusicLibraryDbContext())
                {
                    _context.Database.ExecuteSqlCommand("delete from Thumbnail");

                    foreach (var File in files)
                    {
                        Debug.WriteLine("Start processing " + File.Name + "'s music meta data");
                        var thumbnail = await File.GetThumbnailAsync(ThumbnailMode.MusicView, 100, ThumbnailOptions.UseCurrentScale);
                        var properties = await File.Properties.GetMusicPropertiesAsync();

                        var path = File.Path;
                        var bytearray = await ConvertThumbnailToBytesAsync(thumbnail);
                        var artistName = properties.Artist != "" ? properties.Artist : UNKNOWN_ARTIST;
                        var albumArtistName = properties.AlbumArtist != "" ? properties.AlbumArtist : UNKNOWN_ARTIST;
                        var albumTitle = properties.Album != "" ? properties.Album : UNKNOWN_ALBUM;
                        var duration = properties.Duration.ToString(@"mm\:ss");
                        var albumYear = properties.Year != 0 ? properties.Year.ToString() : UNKNOWN_YEAR;
                        var songTitle = properties.Title != "" ? properties.Title : Path.GetFileNameWithoutExtension(File.Path);

                        var song = _context.Musics.SingleOrDefault(s => s.Title == songTitle);
                        if (song != null)
                        {
                            var thumb = new Thumbnail
                            {
                                ImageBytes = bytearray,
                                Id = Guid.NewGuid().ToString()
                            };
                            _context.Thumbnails.Add(thumb);
                            _context.SaveChanges();
                            song.Synced = true;
                            song.ThumbnailId = thumb.Id;
                            _context.Musics.Update(song);
                            _context.SaveChanges();
                        }
                        else
                        {
                            var thumb = new Thumbnail
                            {
                                ImageBytes = bytearray,
                                Id = Guid.NewGuid().ToString()
                            };
                            _context.Thumbnails.Add(thumb);
                            _context.SaveChanges();
                            var music = new Music
                            {
                                Path = path,
                                Title = songTitle,
                                AlbumTitle = albumTitle,
                                Artist = artistName,
                                AlbumArtist = albumArtistName,
                                Year = albumYear,
                                ThumbnailId = thumb.Id,
                                Duration = duration,
                                Synced = true,
                                Id = Guid.NewGuid().ToString()
                            };
                            _context.Musics.Add(music);
                            _context.SaveChanges();
                        }
                    }
                    _context.Database.ExecuteSqlCommand("DELETE FROM Music WHERE Synced == 0;");
                    _context.Database.ExecuteSqlCommand("UPDATE Music SET Synced = 0;");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
            
        }
        public static LocalAlbumModel FetchAlbum(string AlbumId)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return CreateAlbum(_context.Musics.Include(m => m.Thumbnail).Include(m => m.MusicInPlaylists).Where(m => m.Id == AlbumId).ToList());
            }
        }
        public static List<LocalAlbumModel> FetchAlbums()
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return _context.Musics.Include(m=>m.Thumbnail).Include(m=>m.MusicInPlaylists).GroupBy(m => m.AlbumTitle).Select(g => CreateAlbum(g.ToList())).ToList();
            }
        }
        public static List<LocalThumbnailModel> FetchThumbnails()
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return _context.Thumbnails.Select(m => new LocalThumbnailModel
                {
                    Id = m.Id,
                    ImageBytes = m.ImageBytes,
                    Image = Utilities.GetBitmapImage(m.ImageBytes)
                }).ToList();
            }
        }
        public static List<LocalPlaylistModel> FetchPlaylists()
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return _context.Playlists.Include(p=>p.MusicInPlaylists).Select(p => new LocalPlaylistModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Musics = new ObservableCollection<LocalMusicModel>(p.MusicInPlaylists.Select(m => new LocalMusicModel
                    {
                        Id = m.Music.Id,
                        Path = m.Music.Path,
                        Title = m.Music.Title,
                        Duration = m.Music.Duration,
                        AlbumTitle = m.Music.AlbumTitle,
                        AlbumArtist = m.Music.AlbumArtist,
                        Artist = m.Music.Artist,
                        Year = m.Music.Year,
                        ThumbnailId = m.Music.Thumbnail.Id,
                        ImageBytes = m.Music.Thumbnail.ImageBytes,
                        Image = Utilities.GetBitmapImage(m.Music.Thumbnail.ImageBytes)
                    }).ToList())
                }).ToList();
            }
        }
        public static LocalPlaylistModel FetchPlaylist(string PlaylistId)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var qwe = _context.Playlists.Include(p => p.MusicInPlaylists).FirstOrDefault(p => p.Id == PlaylistId);
                return new LocalPlaylistModel
                {
                    Id = qwe.Id,
                    Name = qwe.Name,
                    Musics = new ObservableCollection<LocalMusicModel>(qwe.MusicInPlaylists.Select(m => new LocalMusicModel
                    {
                        Id = m.Music.Id,
                        Path = m.Music.Path,
                        Title = m.Music.Title,
                        Duration = m.Music.Duration,
                        AlbumTitle = m.Music.AlbumTitle,
                        AlbumArtist = m.Music.AlbumArtist,
                        Artist = m.Music.Artist,
                        Year = m.Music.Year,
                        ThumbnailId = m.Music.Thumbnail.Id,
                        ImageBytes = m.Music.Thumbnail.ImageBytes,
                        Image = Utilities.GetBitmapImage(m.Music.Thumbnail.ImageBytes)
                    }).ToList())
                };
                    
            }
        }
        public static Playlist CreatePlaylist(string playlistName)
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

            return playlist;
        }
        public static void EditPlaylistName(string playlistId, string newPlaylistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Id == playlistId).FirstOrDefault();
                playlist.Name = newPlaylistName;
                _context.Playlists.Update(playlist);
                _context.SaveChanges();
            }
        }
        public static void DeletePlaylist(string playlistId)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Id == playlistId).FirstOrDefault();
                _context.Playlists.Remove(playlist);
                _context.SaveChanges();
            }
        }
        public static void AddSongsToPlaylist(string playlistId, List<LocalMusicModel> musics)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Id == playlistId).FirstOrDefault();
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
        public static void RemoveSongsFromPlaylist(string playlistId, List<LocalMusicModel> musics)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                var playlist = _context.Playlists.Where(p => p.Id == playlistId).First();
                foreach (var item in musics)
                {
                    var mInP = _context.MusicInPlaylists.Where(m => m.PlaylistId == playlist.Id && m.MusicId == item.Id).First();
                    _context.MusicInPlaylists.Remove(mInP);
                }
                _context.SaveChanges();
            }
        }
        public static void Reset()
        {
            try
            {
                using (var _context = new MusicLibraryDbContext())
                {
                    _context.Database.EnsureDeleted();
                }
            }
            catch(Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }
        #endregion

        #region Helper
        private static LocalAlbumModel CreateAlbum(List<Music> musics)
        {
            var localMusics = musics.Select(m => new LocalMusicModel
            {
                Id = m.Id,
                Path = m.Path,
                Title = m.Title,
                Duration = m.Duration,
                AlbumTitle = m.AlbumTitle,
                AlbumArtist = m.AlbumArtist,
                Artist = m.Artist,
                Year = m.Year,
                ThumbnailId = m.Thumbnail.Id,
                ImageBytes = m.Thumbnail.ImageBytes,
                Image =  Utilities.GetBitmapImage(m.Thumbnail.ImageBytes)
            });
            return new LocalAlbumModel
            {
                Title = localMusics.First().AlbumTitle,
                Year = localMusics.First().Year,
                AlbumArtist = localMusics.First().AlbumArtist,
                Image = localMusics.First().Image,
                Musics = new ObservableCollection<LocalMusicModel>(localMusics)
            };
        }
        public static bool PlaylistNameAvailable(string playlistName)
        {
            using (var _context = new MusicLibraryDbContext())
            {
                return (_context.Playlists.Where(p => p.Name == playlistName).FirstOrDefault() == null);
            }
        }
        private static async Task<byte[]> ConvertThumbnailToBytesAsync(StorageItemThumbnail thumbnail)
        {
            byte[] result = new byte[thumbnail.Size];
            using (var reader = new DataReader(thumbnail))
            {
                await reader.LoadAsync((uint)thumbnail.Size);
                reader.ReadBytes(result);
                string base64ImageString = Convert.ToBase64String(result);
                return base64ImageString == Utilities.NULL_THUMBNAIL_BASE64_STRING ? new byte[0] : result;
            }
        }
        #endregion
    }
}
