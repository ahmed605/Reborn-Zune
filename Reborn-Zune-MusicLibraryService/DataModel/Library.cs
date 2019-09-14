using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class Library
    {
        public List<MLMusicModel> Musics { get; set; }
        public List<MLPlayListModel> Playlists { get; set; }
        public List<MLThumbnailModel> Thumbnails { get; set; }
        public List<MLMusicInPlaylistModel> MInP { get; set; }
    }
}
