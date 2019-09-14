using Reborn_Zune_MusicLibraryEFCoreModel;

namespace Reborn_Zune_MusicLibraryService.DataModel
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
