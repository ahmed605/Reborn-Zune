using GalaSoft.MvvmLight;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class LocalArtistModel
    {
        #region Constructor
        public LocalArtistModel()
        {
            Albums = new ObservableCollection<LocalAlbumModel>();
            Musics = new ObservableCollection<LocalMusicModel>();
        }
        #endregion
        public string Name { get; set; }
        public ObservableCollection<LocalMusicModel> Musics { get; set; }
        public ObservableCollection<LocalAlbumModel> Albums { get; set; }
    }
}
