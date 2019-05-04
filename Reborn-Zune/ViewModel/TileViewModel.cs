using GalaSoft.MvvmLight;
using Reborn_Zune.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
#pragma warning disable 0169
namespace Reborn_Zune.ViewModel
{
    public class TileViewModel :ViewModelBase
    {

        #region Fields
        private CoreDispatcher dispatcher;
        private PlayerViewModel _playerViewModel;
        private ObservableCollection<BitmapImage> _bitmapList;
        private ObservableCollection<UIElement> _tiles;
        private ObservableCollection<BitmapImage> thumbnails;
        #endregion

        #region Constructor
        public TileViewModel()
        {
            BitmapList = new ObservableCollection<BitmapImage>();
            Tiles = new ObservableCollection<UIElement>();
        }

        public TileViewModel(ObservableCollection<BitmapImage> thumbnails)
        {
            BitmapList = thumbnails;
            Tiles = new ObservableCollection<UIElement>();
            CreateTile();
        }
        #endregion

        #region Properties
        public ObservableCollection<BitmapImage> BitmapList
        {
            get
            {
                return _bitmapList;
            }
            set
            {
                if (_bitmapList != value)
                {
                    _bitmapList = value;
                    RaisePropertyChanged(() => BitmapList);
                }
            }
        }

        public ObservableCollection<UIElement> Tiles
        {
            get
            {
                return _tiles;
            }
            set
            {
                if (_tiles != value)
                {
                    _tiles = value;
                    RaisePropertyChanged(() => Tiles);
                }
            }
        }
        #endregion


        #region Helpers
        private int Spans(int i)
        {
            switch (i)
            {
                case 1:
                    return 4;
                case 3:
                    return 3;
                case 9:
                    return 3;
                case 23:
                    return 3;
                case 27:
                    return 3;
                case 32:
                    return 3;
                case 39:
                    return 3;
                case 42:
                    return 3;
                case 50:
                    return 4;
                case 52:
                    return 3;
                case 59:
                    return 3;
                case 65:
                    return 3;
                case 70:
                    return 3;
                case 82:
                    return 3;
                case 90:
                    return 3;
                case 98:
                    return 3;
                case 100:
                    return 3;
                case 103:
                    return 4;
                case 110:
                    return 3;
                case 118:
                    return 3;
                case 120:
                    return 3;
                case 121:
                    return 3;
                case 130:
                    return 3;
                case 140:
                    return 3;
                case 147:
                    return 4;
                case 155:
                    return 3;
                case 160:
                    return 3;
                case 166:
                    return 3;
                case 170:
                    return 3;
                case 175:
                    return 3;
                case 185:
                    return 4;
                case 200:
                    return 4;
                case 210:
                    return 3;
                case 217:
                    return 3;
                case 220:
                    return 4;
                case 252:
                    return 3;
                default:
                    return 1;

            }
        }

        private void CreateTile()
        {
            var a = new ObservableCollection<UIElement>();
            Random rnd = new Random();
            for (int i = 0; i < 300; i++)
            {
                int factor = Spans(i);
                int id = rnd.Next(BitmapList.Count);
                Tile tile = new Tile()
                {
                    Width = factor * 70,
                    Height = factor * 70,
                    Thumbnail = BitmapList[id],
                    Index = i
                };
                a.Add(tile);
            }
            Tiles = a;
        }

        public void ClearTiles()
        {
            var a = new ObservableCollection<UIElement>();
            Tiles = a;
            GC.Collect();
        }
        #endregion
    }
}
