using MusicLibraryEFCoreModel;
using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune_MusicLibraryService.MusicLibraryDataModel
{
    public class MLMusicInPlaylistModel
    {
        public MLMusicInPlaylistModel(MusicInPlaylist mInP)
        {
            MusicInPlaylist = mInP;
        }
        public MusicInPlaylist MusicInPlaylist { get; set; }
    }
}
