using Microsoft.Toolkit.Uwp.Helpers;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.Storage;

namespace MusicLibraryService
{
    public static class MusicLibrary
    {
        public static async void Initialize(bool IsFirstUse)
        {
            DataBaseService.Initialize();
            var result = await LibraryService.Initialize(IsFirstUse);
            foreach(var i in result)
            {
                
                if(i.Value.GetType().Name == "StorageFile") //Add/Update DataBase
                {
                    Debug.WriteLine("StorageFile");
                    if (i.Key == StorageLibraryChangeType.ContentsChanged)
                    {
                        Debug.WriteLine("ContentChanged");
                        await DataBaseService.Update(i.Value as StorageFile);
                    }
                    else if(i.Key == StorageLibraryChangeType.MovedIntoLibrary)
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
        }
    }
}
