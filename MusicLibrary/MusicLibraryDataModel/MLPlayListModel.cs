using MusicLibraryEFCoreModel;
using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune_MusicLibraryService.MusicLibraryDataModel
{
    public class MLPlayListModel
    {
        public MLPlayListModel(Playlist playlist)
        {
            Playlist = playlist;
        }
        public Playlist Playlist { get; set; }
    }
}
