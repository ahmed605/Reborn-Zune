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
        private ObservableCollection<WriteableBitmap> _bitmapList;
        private ObservableCollection<UIElement> _tiles;
        #endregion

        #region Constructor
        public TileViewModel()
        {
            BitmapList = new ObservableCollection<WriteableBitmap>();
            Tiles = new ObservableCollection<UIElement>();
        }
        #endregion

        #region Properties
        public ObservableCollection<WriteableBitmap> BitmapList
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

        #region Events
        public void CurrentListDoubleTapped(object sender, DoubleTappedRoutedEventArgs e)
        {
            int selectedIndex = (sender as ListView).SelectedIndex;
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

        public async void CreateTiles(List<WriteableBitmap> getThumbnails)
        {
            foreach (var item in getThumbnails)
            {
                var grayBitmap = await GrayScaleBitmap(item);
                BitmapList.Add(grayBitmap);
            }

            CreateTile();
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

        private async Task<WriteableBitmap> GrayScaleBitmap(WriteableBitmap thumbnail)
        {
            byte[] srcPixels = new byte[4 * thumbnail.PixelWidth * thumbnail.PixelHeight];
            using (Stream pixelStream = thumbnail.PixelBuffer.AsStream())
            {
                await pixelStream.ReadAsync(srcPixels, 0, srcPixels.Length);
            }
            WriteableBitmap dstBitmap =
               new WriteableBitmap(thumbnail.PixelWidth, thumbnail.PixelHeight);
            byte[] dstPixels = new byte[4 * dstBitmap.PixelWidth * dstBitmap.PixelHeight];
            for (int i = 0; i < srcPixels.Length; i += 4)
            {
                double b = (double)srcPixels[i] / 255.0;
                double g = (double)srcPixels[i + 1] / 255.0;
                double r = (double)srcPixels[i + 2] / 255.0;

                byte a = srcPixels[i + 3];

                double e = (0.21 * r + 0.71 * g + 0.07 * b) * 255;
                byte f = Convert.ToByte(e);

                dstPixels[i] = f;
                dstPixels[i + 1] = f;
                dstPixels[i + 2] = f;
                dstPixels[i + 3] = a;
            }
            // Move the pixels into the destination bitmap
            using (Stream pixelStream = dstBitmap.PixelBuffer.AsStream())
            {
                await pixelStream.WriteAsync(dstPixels, 0, dstPixels.Length);
            }
            dstBitmap.Invalidate();
            return dstBitmap;
        }
        #endregion
    }
}
