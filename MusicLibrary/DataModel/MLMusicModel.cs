using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class MLMusicModel
    {
        public MLMusicModel(Music music)
        {
            this.Music = music;
            GetFileAsync(music);
        }

        public Music Music { get; set; }

        public StorageFile File { get; set; }

        private async void GetFileAsync(Music music)
        {
            File = await StorageFile.GetFileFromPathAsync(Music.Path);
        }
    }
}
