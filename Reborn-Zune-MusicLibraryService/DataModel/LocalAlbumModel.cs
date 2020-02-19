using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class LocalAlbumModel
    {
        public LocalAlbumModel()
        {
            Musics = new ObservableCollection<LocalMusicModel>();
        }
        public string Title { get; set; }
        public string Year { get; set; }
        public string AlbumArtist { get; set; }
        public BitmapImage Image { get; set; }

        public ObservableCollection<LocalMusicModel> Musics { get; set; }
    }
}