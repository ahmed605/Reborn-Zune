using MusicLibraryEFCoreModel;
using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class MLMusicModel
    {
        public MLMusicModel(Music music)
        {
            this.Music = music;
        }

        public Music Music { get; set; }

        public StorageFile File { get; set; }
    }
}
