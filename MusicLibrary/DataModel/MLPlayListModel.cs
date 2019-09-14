using Reborn_Zune_MusicLibraryEFCoreModel;

namespace Reborn_Zune_MusicLibraryService.DataModel
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
