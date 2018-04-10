using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;

namespace Reborn_Zune.Control
{
    public sealed class CustomMediaTransportControl : MediaTransportControls
    {
        public event EventHandler<EventArgs> Clicked;

        Image ThumbnailImage;
        TextBlock MusicTitle;
        Grid VolumeSliderGrid;
        StackPanel VolumeValue;
        Slider VolumeSlider;
        DockPanel MediaTransportControlsTimelineGrid;
        Button TilePageButton;
        public CustomMediaTransportControl()
        {
            DefaultStyleKey = typeof(CustomMediaTransportControl);
        }

        protected override void OnApplyTemplate()
        {
            // This is where you would get your custom button and create an event handler for its click method.
            ThumbnailImage = GetTemplateChild("MusicThumbnail") as Image;
            MusicTitle = GetTemplateChild("MusicTitle") as TextBlock;
            VolumeSliderGrid = GetTemplateChild("VolumeSliderGrid") as Grid;
            VolumeValue = GetTemplateChild("VolumeValue") as StackPanel;
            VolumeSlider = GetTemplateChild("VolumeSlider") as Slider;
            MediaTransportControlsTimelineGrid = GetTemplateChild("MediaTransportControlsTimelineGrid") as DockPanel;
            TilePageButton = GetTemplateChild("TilePageButton") as Button;


            VolumeSliderGrid.PointerEntered += Grid_PointerEntered;
            VolumeSliderGrid.PointerExited += Grid_PointerExited;
            MediaTransportControlsTimelineGrid.Loaded += DockPanel_Loaded;
            TilePageButton.Click += Button_Clicked;

            base.OnApplyTemplate();
        }

        private void Button_Clicked(object sender, RoutedEventArgs e)
        {
            Clicked?.Invoke(this, EventArgs.Empty);
        }

        private async void DockPanel_Loaded(object sender, RoutedEventArgs e)
        {
            await MediaTransportControlsTimelineGrid.Fade(1f, 400)
                            .Offset(-20f,0f,300)
                            .StartAsync();
        }

        private void Grid_PointerExited(object sender, PointerRoutedEventArgs e)
        {
               VolumeSlider.Scale(centerX: 5f,
                            centerY: 5f,
                            scaleX: 0.5f,
                            scaleY: 0.5f,
                            duration: 300,
                            delay:3000)
                        .Fade(0, 300)
                        .StartAsync();

            VolumeValue.Scale(centerX: 0.8f,
                        centerY: 0.8f,
                        scaleX: 1f,
                        scaleY: 1f,
                        duration: 300, delay: 3)
                        .Fade(1, 300)
                        .StartAsync();
        }

        private void Grid_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            VolumeValue.Scale(centerX:0.8f, 
                            centerY:0.8f,
                            scaleX:0.5f,
                            scaleY: 0.5f,
                            duration: 300)
                        .Fade(0,300)
                        .StartAsync();

            VolumeSlider.Scale(centerX: 5f,
                        centerY: 5f,
                        scaleX: 1f,
                        scaleY: 1f,
                        duration: 300, delay: 3)
                        .Fade(1, 300)
                        .StartAsync();
        }

        public String Title
        {
            get { return (String)GetValue(TitleProperty); }
            set { SetValue(TitleProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Title.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register("Title", typeof(String), typeof(CustomMediaTransportControl), new PropertyMetadata(String.Empty, onTitleChanged));

        private static void onTitleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CustomMediaTransportControl)d).TitlePropertyChanged(e.NewValue as String);
        }

        private void TitlePropertyChanged(string v)
        {
            MusicTitle.Text = v;
        }

        public BitmapSource Thumbnail 
        {
            get { return (BitmapSource)GetValue(MyPropertyProperty); }
            set { SetValue(MyPropertyProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MyPropertyProperty =
            DependencyProperty.Register("Thumbnail", typeof(BitmapSource), typeof(CustomMediaTransportControl), new PropertyMetadata(new BitmapImage(), onThumbnailChanged));

        private static void onThumbnailChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            
            ((CustomMediaTransportControl)d).ThumbnailPropertyChanged(e.NewValue as BitmapSource);
        }

        private void ThumbnailPropertyChanged(BitmapSource bitmapSource)
        {
            ThumbnailImage.Source = bitmapSource;
        }
    }
}
