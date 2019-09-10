using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class Library
    {
        public List<MLMusicModel> Musics { get; set; }
        public List<MLPlayListModel> Playlists { get; set; }
        public List<MLThumbnailModel> Thumbnails { get; set; }
        public List<MLMusicInPlaylistModel> MInP { get; set; }

        public void RenderThumbnail()
        {
            foreach (var item in Thumbnails)
            {
                item.GetBitmapImage();
            }
        }

        public async Task GetFiles()
        {
            foreach (var song in Musics)
            {
                await GetFileAsync(song);
            }
        }

        private async Task GetFileAsync(MLMusicModel song)
        {
            song.File = await StorageFile.GetFileFromPathAsync(song.Music.Path);
        }
    }
}
