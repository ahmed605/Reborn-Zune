using Microsoft.Toolkit.Uwp.Helpers;
using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryService.DataBase;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicLibraryService.LibraryDisk;
using Reborn_Zune_MusicLibraryService.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService
{
    public class MusicLibraryService : IService
    {
        private bool IsFirstUse => SystemInformation.IsFirstRun;
        private bool IsChanged { get; set; }

        public event EventHandler Completed;

        public MusicLibraryService()
        {
            Run();
        }

        public async void Run()
        {
            try
            {
                InitializeDBMS();
                var result = await LoadLibraryDiskAsync();
                await DatabaseSynchronize(result);
                Completed?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        public void Clean()
        {
            DataBaseEngine.Reset();
        }

        #region DBMS (Sealed)
        private void InitializeDBMS()
        {
            try
            {
                DataBaseEngine.Initialize();
                Debug.WriteLine("DBMS Initialize");
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new Exception(e.ToString());
            }

        }
        private async Task DatabaseSynchronize(IReadOnlyList<StorageFile> result)
        {
            await DataBaseEngine.Sync(result);
        }

        #endregion

        #region LibraryDisk (Sealed)
        private async Task<IReadOnlyList<StorageFile>> LoadLibraryDiskAsync()
        {
            try
            {
                Debug.WriteLine("Library Initialize");
                LibraryReturnContainer result = await LibraryEngine.Initialize(IsFirstUse);
                IsChanged = result.isChanged;
                return result.files;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw new Exception(e.ToString());
            }

        }
        #endregion

        #region ServiceOperation
        public LocalAlbumModel FetchAlbum(string AlbumId)
        {
            return DataBaseEngine.FetchAlbum(AlbumId);
        }
        public List<LocalAlbumModel> FetchAllAlbums()
        {
            return DataBaseEngine.FetchAlbums();
        }
        public List<LocalThumbnailModel> FetchThumbnails()
        {
            return DataBaseEngine.FetchThumbnails();
        }
        public List<LocalPlaylistModel> FetchPlaylists()
        {
            return DataBaseEngine.FetchPlaylists();
        }
        public LocalPlaylistModel FetchPlaylist(string PlaylistId)
        {
            return DataBaseEngine.FetchPlaylist(PlaylistId);
        }
        public void AddSongsToPlaylist(string playlistId, List<LocalMusicModel> musics)
        {
            DataBaseEngine.AddSongsToPlaylist(playlistId, musics);
        }
        public bool CreatePlaylist(string playlistName)
        {
            if (!DataBaseEngine.PlaylistNameAvailable(playlistName))
            {
                return false;
            }
            else
            {
                DataBaseEngine.CreatePlaylist(playlistName);
                return true;
            }
        }
        public void EditPlaylistName(string playlistId, string newName)
        {
            DataBaseEngine.EditPlaylistName(playlistId, newName);
        }
        public void DeletePlaylist(string playlistId)
        {
            DataBaseEngine.DeletePlaylist(playlistId);
        }
        public void RemoveSongsFromPlaylist(string playlistId, List<LocalMusicModel> musics)
        {
            DataBaseEngine.RemoveSongsFromPlaylist(playlistId, musics);
        }
        #endregion

    }
}
