using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_Common.Interface
{
    public interface IPlaybackItem
    {
        string GetTitle();
        BitmapImage GetImage();
        string GetMediaItemIdKey();
        string GetId();
        Task<MediaPlaybackItem> ToPlaybackItem();
        MediaPlaybackItem GetExsistedPlaybackItem();
    }
}
