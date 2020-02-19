using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class LocalPlaylistModel
    {
        public LocalPlaylistModel()
        {
            Musics = new ObservableCollection<LocalMusicModel>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ObservableCollection<LocalMusicModel> Musics
        {
            get;set;
        }

        public ImageSource GetImage()
        {
            return new BitmapImage(new Uri("ms-appx:///Assets/LargeTile.scale-400.png"));
        }

        public string GetArtist()
        {
            return "Various Artists";
        }
    }
}
