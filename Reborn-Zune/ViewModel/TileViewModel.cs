using GalaSoft.MvvmLight;
using Reborn_Zune.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.ViewModel
{
    public class TileViewModel :ViewModelBase
    {
        private CoreDispatcher dispatcher;
        private PlayerViewModel _playerViewModel;
        private ObservableCollection<WriteableBitmap> _bitmapList;
        private ObservableCollection<UIElement> _tiles;

        public TileViewModel()
        {

        }
        public TileViewModel(CoreDispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public PlayerViewModel PlayerViewModel
        {
            get
            {
                return _playerViewModel;
            }
            set
            {
                _playerViewModel = value;
                RaisePropertyChanged(() => PlayerViewModel);
            }
        }

        public ObservableCollection<WriteableBitmap> BitmapList
        {
            get
            {
                return _bitmapList;
            }
            set
            {
                if(_bitmapList != value)
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
                if(_tiles != value)
                {
                    _tiles = value;
                    RaisePropertyChanged(() => Tiles);
                }
            }
        }

        public void CreateTiles()
        {
            if (Tiles == null)
                Tiles = new ObservableCollection<UIElement>();
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
                Tiles.Add(tile);
            }
        }

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
    }
}
