using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using Reborn_Zune.Utilities;
using System;
using System.Threading.Tasks;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalMusicModel : ObservableObject
    {
        public LocalMusicModel(Music music)
        {
            Music = music;
        }

        public const String MediaItemIdKey = "mediaItemId";

        private Music _music;

        public Music Music
        {
            get
            {
                return _music;
            }
            set
            {
                Set<Music>(() => this.Music, ref _music, value);
            }
        }

        public String GetMediaItemIdKey
        {
            get
            {
                return MediaItemIdKey;
            }
        }

        public MediaPlaybackItem ToPlaybackItem()
        {
            var source = MediaSource.CreateFromStorageFile(Music.File);

            var playbackItem = new MediaPlaybackItem(source);

            var displayProperties = playbackItem.GetDisplayProperties();

            playbackItem.ApplyDisplayProperties(displayProperties);

            source.CustomProperties[GetMediaItemIdKey] = Music.Id;

            return playbackItem;
        }

    }
}
