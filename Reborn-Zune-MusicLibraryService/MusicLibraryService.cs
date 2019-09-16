using Microsoft.Toolkit.Uwp.Helpers;
using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryService.DataBase;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicLibraryService.LibraryDisk;
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

        public event EventHandler Completed;

        public MusicLibraryService()
        {
            Run();
        }

        public Library Library { get; set; }

        public async void Run()
        {
            InitializeDBMS();
            await LoadLibraryDiskAsync();
            await CreateLibraryInstanceAsync();
            Completed?.Invoke(this, EventArgs.Empty);
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
                Debug.WriteLine("DBMS Initialize");
                DataBaseEngine.Initialize();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        private async Task CreateLibraryInstanceAsync()
        {
            try
            {
                Library = await DataBaseEngine.FetchAllAsync();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
        #endregion

        #region LibraryDisk (Sealed)
        private async Task LoadLibraryDiskAsync()
        {
            try
            {
                Debug.WriteLine("Library Initialize");
                var result = await LibraryEngine.Initialize(IsFirstUse);
                foreach (var i in result)
                {
                    if (i.Value.GetType().Name == "StorageFile") //Add/Update DataBase
                    {
                        //Debug.WriteLine("StorageFile");
                        if (i.Key == StorageLibraryChangeType.ContentsChanged)
                        {
                            //Debug.WriteLine("ContentChanged");
                            await DataBaseEngine.Update(i.Value as StorageFile);
                        }
                        else if (i.Key == StorageLibraryChangeType.MovedIntoLibrary)
                        {
                            //Debug.WriteLine("MovedIntoLibrary");
                            await DataBaseEngine.Add((StorageFile)i.Value);
                        }

                    }
                    else if (i.Value.GetType().Name == "String") //Moved Out
                    {
                        //Debug.WriteLine("Move out");
                        DataBaseEngine.Delete(i.Value.ToString());
                    }
                    else if (i.Value.GetType().Name == "KeyValuePair`2") //Moved or Renamed
                    {
                        //Debug.WriteLine("Moved or Renamed");
                        DataBaseEngine.Update((KeyValuePair<string, string>)i.Value);
                    }
                }
                //InitializeFinished?.Invoke(null, EventArgs.Empty);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }
        #endregion

        #region ServiceOperation
        public void AddSongsToPlaylist(string v, List<MLMusicModel> musics)
        {
            DataBaseEngine.AddSongsToPlaylist(v, musics);
            RefreshLibrary();
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
                RefreshLibrary();
                return true;
            }
        }

        public void EditPlaylistName(string oldName, string newName)
        {
            DataBaseEngine.EditPlaylistName(oldName, newName);
            RefreshLibrary();
        }

        public void DeletePlaylist(string name)
        {
            DataBaseEngine.DeletePlaylist(name);
            RefreshLibrary();
        }

        public void RemoveSongsFromPlaylist(string playlistName, List<MLMusicModel> musics)
        {
            DataBaseEngine.RemoveSongsFromPlaylist(playlistName, musics);
            RefreshLibrary();
        }

        private void RefreshLibrary()
        {
            Library.MInP = new ObservableCollection<MLMusicInPlaylistModel>(DataBaseEngine.FetchSongPlaylistRelationship().Select(m => new MLMusicInPlaylistModel(m)).ToList());
            Library.Playlists = new ObservableCollection<MLPlayListModel>(DataBaseEngine.FetchPlaylist().Select(p => new MLPlayListModel(p)).ToList());
        }

        #endregion

    }
}
