using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryEFCoreModel;
using Reborn_Zune_MusicLibraryService.DataBase;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicLibraryService.LibraryDisk;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService
{
    public class MusicLibraryService : IService
    {
        private bool IsFirstUse;
        public static event EventHandler InitializeFinished;
        public static event EventHandler FetchSucceed;
        public static Library Library { get; set; }

        public MusicLibraryService() { }

        public MusicLibraryService(bool IsFirstUse)
        {
            this.IsFirstUse = IsFirstUse;
        }

        public void Run()
        {
            InitializeDBMS();
            LoadLibraryDisk();
            FetchDBMS();
        }

        #region DBMS
        public void InitializeDBMS()
        {
            try
            {
                DataBaseEngine.Initialize();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        public void FetchDBMS()
        {
            try
            {
                Library = DataBaseEngine.FetchAll();
                //library.RenderThumbnail(); //TODO: Move to DataBaseEngine, should be done when the engine fetch the library
                //await library.GetFiles(); //TODO: Move to DataBaseEngine
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
            }

        }
        #endregion

        #region LibraryDisk
        public async void LoadLibraryDisk()
        {
            try
            {
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
        public static void AddSongsToPlaylist(string v, List<Music> musics)
        {
            DataBaseEngine.AddSongsToPlaylist(v, musics);
        }

        public static bool CreatePlaylist(string playlistName)
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

        public static Library FetchPlaylist()
        {
            var library = DataBaseEngine.FetchAll();
            return library;
        }
        #endregion

    }
}
