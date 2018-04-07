using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune.Control
{
    public sealed class CustomMediaTransportControl : MediaTransportControls
    {

        Image ThumbnailImage;
        TextBlock MusicTitle;
        public CustomMediaTransportControl()
        {
            DefaultStyleKey = typeof(CustomMediaTransportControl);
        }

        protected override void OnApplyTemplate()
        {
            // This is where you would get your custom button and create an event handler for its click method.
            ThumbnailImage = GetTemplateChild("MusicThumbnail") as Image;
            MusicTitle = GetTemplateChild("MusicTitle") as TextBlock;
            base.OnApplyTemplate();
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
