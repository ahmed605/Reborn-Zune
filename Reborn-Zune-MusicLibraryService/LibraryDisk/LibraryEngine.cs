using Reborn_Zune_MusicLibraryService.Utility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;

namespace Reborn_Zune_MusicLibraryService.LibraryDisk
{
    static class LibraryEngine
    {
        public static async Task<LibraryReturnContainer> Initialize(bool IsFirstUse)
        {
            return await LoadLibrary(IsFirstUse);
        }

        private static async Task<LibraryReturnContainer> LoadLibrary(bool isFirstUse)
        {
            bool isChanged = false;
            List<KeyValuePair<StorageLibraryChangeType, object>> changes = new List<KeyValuePair<StorageLibraryChangeType, object>>();
            StorageLibrary musicsLib = await StorageLibrary.GetLibraryAsync(KnownLibraryId.Music);
            musicsLib.ChangeTracker.Enable();

            if (!isFirstUse)
            {
                StorageLibraryChangeReader musicChangeReader = musicsLib.ChangeTracker.GetChangeReader();
                isChanged = (await musicChangeReader.ReadBatchAsync()).Count > 0;
                await musicChangeReader.AcceptChangesAsync();
                Debug.WriteLine("Library Changed Detected, Reload all library stuff");
            }

            try
            {
                if(isFirstUse || isChanged)
                {
                    QueryOptions queryOption = new QueryOptions
                        (CommonFileQuery.OrderByTitle, new string[] { ".mp3", ".m4a", ".mp4" });

                    queryOption.FolderDepth = FolderDepth.Deep;

                    Queue<IStorageFolder> folders = new Queue<IStorageFolder>();

                    IReadOnlyList<StorageFile> files = await KnownFolders.MusicLibrary.CreateFileQueryWithOptions
                      (queryOption).GetFilesAsync();
                    return new LibraryReturnContainer
                    {
                        files = files,
                        isChanged = isChanged
                    };
                }
                Debug.WriteLine("Fetch Library succeed");
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.ToString());
                throw new Exception(e.ToString());
            }
        }
    }
}
