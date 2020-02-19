using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicLibraryService.DataModel
{
    public class LocalThumbnailModel
    {
        public string Id { get; set; }
        public byte[] ImageBytes { get; set; }
        public BitmapImage Image { get; set; }

        
    }
}
