using GalaSoft.MvvmLight;
using MusicLibraryEFCoreModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reborn_Zune.Model
{
    public class LocalArtistModel : ObservableObject
    {
        #region Constructor
        public LocalArtistModel(Artist artist)
        {
            Artist = artist;
        }
        #endregion

        private Artist _artist;
        public Artist Artist
        {
            get
            {
                return _artist;
            }
            set
            {
                Set<Artist>(() => this.Artist, ref _artist, value);
            }
        }
    }
}
