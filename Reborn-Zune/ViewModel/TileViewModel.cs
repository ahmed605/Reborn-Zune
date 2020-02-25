﻿using GalaSoft.MvvmLight;
using Reborn_Zune.Control;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Display;
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
        private Random rnd = new Random();
        private ObservableCollection<BitmapImage> _bitmapList;
        private ObservableCollection<UIElement> _tiles;
        private ObservableCollection<BitmapImage> thumbnails;
        private double _actualWidth;
        private double _actualHeight;
        private int _maxTileNumber;
        #endregion

        #region Constructor
        public TileViewModel()
        {
            BitmapList = new ObservableCollection<BitmapImage>();
            Tiles = new ObservableCollection<UIElement>();
            var displayInformation = DisplayInformation.GetForCurrentView();
            
        }

        public TileViewModel(ObservableCollection<BitmapImage> thumbnails)
        {
            BitmapList = thumbnails;
            Tiles = new ObservableCollection<UIElement>();
            CalculateScreeInfo();
            CreateTile();
            
        }

        private void CalculateScreeInfo()
        {
            var displayInformation = DisplayInformation.GetForCurrentView();
            ActualHeight = displayInformation.ScreenHeightInRawPixels + 70 * 2;
            ActualWidth = displayInformation.ScreenWidthInRawPixels + 70 * 1;
            var RawPVP = displayInformation.RawPixelsPerViewPixel;
            var maxViewPixel = (ActualWidth / RawPVP) * (ActualWidth / RawPVP);
            var tileViewPixel = 4900 / RawPVP;
            var percentage = 1 + (RawPVP+4) / 10;
            MaxTileNumer = (int)((maxViewPixel / tileViewPixel) * (0.22 * percentage));
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

        public double ActualWidth
        {
            get
            {
                return _actualWidth;
            }
            set
            {
                if(_actualWidth != value)
                {
                    _actualWidth = value;
                    RaisePropertyChanged(() => ActualWidth);
                }
            }
        }

        public double ActualHeight
        {
            get
            {
                return _actualHeight;
            }
            set
            {
                if(_actualHeight != value)
                {
                    _actualHeight = value;
                    RaisePropertyChanged(() => ActualHeight);
                }
            }
        }

        public int MaxTileNumer
        {
            get
            {
                return _maxTileNumber;
            }
            set
            {
                if(_maxTileNumber != value)
                {
                    _maxTileNumber = value;
                    RaisePropertyChanged(() => MaxTileNumer);
                }
            }
        }
        #endregion


        #region Helpers
        private int Spans(int i)
        {
            int val = rnd.Next(0,200);
            if(val < 5)
            {
                return 4;
            }
            else if(val < 25)
            {
                return 3;
            }
            else
            {
                return 1;
            }
        }

        private void CreateTile()
        {
            var a = new ObservableCollection<UIElement>();
            for (int i = 0; i < MaxTileNumer; i++)
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
