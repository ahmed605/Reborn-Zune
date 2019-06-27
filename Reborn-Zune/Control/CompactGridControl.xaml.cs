using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Reborn_Zune.Control
{
    public sealed partial class CompactGridControl : UserControl
    {



        public ObservableCollection<BitmapImage> ThumbanilSource
        {
            get { return (ObservableCollection<BitmapImage>)GetValue(ThumbanilSourceProperty); }
            set { SetValue(ThumbanilSourceProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ThumbanilSource.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ThumbanilSourceProperty =
            DependencyProperty.Register("ThumbanilSource", typeof(ObservableCollection<BitmapImage>), typeof(CompactGridControl), new PropertyMetadata(new ObservableCollection<BitmapImage>(), onThumbnailSourceChanged));

        private static void onThumbnailSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((CompactGridControl)d).onThumbnailSourceChanged(e.NewValue as ObservableCollection<BitmapImage>);
        }

        private void onThumbnailSourceChanged(ObservableCollection<BitmapImage> source)
        {
            gridView.ItemsSource = source;
        }

        public CompactGridControl()
        {
            this.InitializeComponent();
            gridView.ItemsSource = new List<string>
            {
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
                "a",
            };
        }
    }
}
