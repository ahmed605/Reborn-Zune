using Microsoft.Graphics.Canvas;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.Helpers;
using Reborn_Zune.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Numerics;
using Windows.Graphics.Display;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Composition.Effects;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Reborn_Zune
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //private ObservableCollection<UIElement> _items;
        //private const float lightDepth = 300f;
        //private const int animationDelay = 600;
        //private const int animationDuration = 70;
        //private CompositionEffectFactory _effectFactory;
        //private Random _random = new Random();
        private Compositor _compositor;
        private PointLight _pointLight;
        //private PointLight _secondPointLight;
        private Visual _root;
        private MainViewModel viewModel;

        public MainPage()
        {
            this.InitializeComponent();
            //ObservableCollection<String> _Artists = new ObservableCollection<string>
            //{
            //    "GALA",
            //    "Rich Brian",
            //    "タニザワトモフミ",
            //    "卫兰",
            //    "宋冬野",
            //    "张学友",
            //    "朴树",
            //    "林俊杰",
            //    "程璧",
            //    "逃跑计划",
            //    "陈奕迅",
            //    "陈粒",
            //    "陈长林",
            //    "马頔",
            //    "骆集益",
            //    "黑豹乐队"
            //};
            //ObservableCollection<BitmapImage> _Alumbs = new ObservableCollection<BitmapImage>
            //{
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //    new BitmapImage(new Uri("ms-appx://Reborn-Zune/Assets/aaa.jpg")),
            //};
            //ObservableCollection<String> _Songs = new ObservableCollection<string>
            //{
            //    "追梦赤子心",
            //    "Is This Love",
            //    "きみにとどけ",
            //    "一万次悲伤",
            //    "冲绳民谣",
            //    "夏日倾情",
            //    "哪里是你的拥抱",
            //    "囍帖街(Live) - live",
            //    "夜空中最亮的星",
            //    "奇妙能力歌",
            //    "她说",
            //    "安和桥",
            //    "平凡之路",
            //    "味道 - live",
            //    "慢慢",
            //    "我真的受伤了",
            //    "李香兰",
            //    "祝福",
            //    "约定 - live",
            //    "蓝月亮 - live",
            //    "恋恋风尘",
            //    "我想和你虚度时光",
            //    "斑马，斑马",
            //    "无地自容",
            //    "时代之梦",
            //    "春分的夜",
            //    "最佳损友",
            //    "结婚",
            //    "给少年的歌",
            //    "绝对占有 相对自由",
            //    "莉莉安",
            //    "董小姐",
            //    "走马",
            //    "再见,再见",
            //    "海咪咪小姐",
            //    "回梦游仙",
            //    "鸽子"
            //};

            //ArtistList.ItemsSource = _Artists;
            //AlbumList.ItemsSource = _Alumbs;
            //SongList.ItemsSource = _Songs;

            viewModel = new MainViewModel();

            var titleBar = ApplicationView.GetForCurrentView().TitleBar;
            titleBar.InactiveBackgroundColor = Windows.UI.Colors.Transparent;
            titleBar.InactiveForegroundColor = Colors.White;
            titleBar.ButtonBackgroundColor = "#00000000".ToColor();
            titleBar.ButtonForegroundColor = Colors.DarkGray;
            titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
            titleBar.ButtonInactiveForegroundColor = Colors.DarkGray;
            titleBar.ButtonHoverBackgroundColor = Colors.Transparent;
            titleBar.ButtonHoverForegroundColor = Colors.Black;
            titleBar.ButtonPressedBackgroundColor = "#f5f5f5".ToColor();
            titleBar.ButtonPressedForegroundColor = Colors.Black;

        }

        
    }
}
