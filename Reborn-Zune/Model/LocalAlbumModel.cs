using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using Reborn_Zune.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Model
{
    public class LocalAlbumModel : ObservableObject
    {
        #region Constructors
        public LocalAlbumModel(Album album)
        {
            Album = album;
            GetThumbanail();
        }

        private async void GetThumbanail()
        {
            Image = await Utility.ImageFromBytes(Album.Musics.First().Thumbnail.Image);
        }
        #endregion

        private Album _album;
        public Album Album
        {
            get
            {
                return _album;
            }
            set
            {
                Set<Album>(() => this.Album, ref _album, value);
            }
        }

        private BitmapImage _image;
        public BitmapImage Image
        {
            get
            {
                return _image;
            }
            set
            {
                Set<BitmapImage>(() => this.Image, ref _image, value);
            }
        }
    }
}
