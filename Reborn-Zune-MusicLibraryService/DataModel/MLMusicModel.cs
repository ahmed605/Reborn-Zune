using Reborn_Zune_MusicLibraryEFCoreModel;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class MLMusicModel : IMLDataModel
    {
        public MLMusicModel(Music music)
        {
            UnwrapDataFields(music);
            GetFile(music);
        }

        public string Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public string Artist { get; set; }
        public string Year { get; set; }
        public string ThumbnailId { get; set; }

        public StorageFile File { get; set; }

        public void UnwrapDataFields(IEFCoreModel model)
        {
            var music = model as Music;
            this.Id = music.Id;
            this.Path = music.Path;
            this.Title = music.Title;
            this.Duration = music.Duration;
            this.AlbumTitle = music.AlbumTitle;
            this.AlbumArtist = music.AlbumArtist;
            this.Artist = music.Artist;
            this.Year = music.Year;
            this.ThumbnailId = music.ThumbnailId;
        }

        private async void GetFile(Music music)
        {
            await GetFileAsync(music);
        }

        private async Task GetFileAsync(Music music)
        {
            File = await StorageFile.GetFileFromPathAsync(Path);
        }
    }
}
