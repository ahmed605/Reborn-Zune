using System;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Reborn_Zune
{
    public sealed partial class Tile : UserControl
    {
        private WriteableBitmap _bitmapImage;

        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Index.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index", typeof(int), typeof(Tile), new PropertyMetadata(0));

        public WriteableBitmap Thumbnail
        {
            get { return (WriteableBitmap)GetValue(ThumbnailProperty); }
            set { SetValue(ThumbnailProperty, value);
                _bitmapImage = value;
            }
        }

        // Using a DependencyProperty as the backing store for Thumbnail.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbnailProperty =
            DependencyProperty.Register("Thumbnail", typeof(WriteableBitmap), typeof(Tile), new PropertyMetadata(null));


        public Tile()
        {
            this.InitializeComponent();
        }

    }
}
