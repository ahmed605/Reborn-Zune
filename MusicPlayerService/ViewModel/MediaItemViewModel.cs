using GalaSoft.MvvmLight;
using Reborn_Zune_MusicLibraryService.DataModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Xaml.Media.Imaging;

namespace Reborn_Zune_MusicPlayerService.ViewModel
{
    public class MediaItemViewModel : ViewModelBase
    {
        MediaListViewModel listViewModel;
        MediaPlaybackItem playbackItem;

        BitmapImage previewImage;

#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword

        public LocalMusicModel MediaItem { get; private set; }

        public string Title => MediaItem.Title;

        public BitmapImage PreviewImage
        {
            get { return previewImage; }

            private set
            {
                if (previewImage != value)
                {
                    previewImage = value;
                    RaisePropertyChanged("PreviewImage");
                }
            }
        }

        public MediaPlaybackItem PlaybackItem
        {
            get
            {
                // Already have one then return it
                if (playbackItem != null)
                    return playbackItem;

                // Don't have one, try to rebind to one in the list
                playbackItem = listViewModel.PlaybackList.Items.SingleOrDefault(pi =>
                    (string)pi.Source.CustomProperties[MediaItem.GetMediaItemIdKey] == MediaItem.Id);

                if (playbackItem != null)
                    return playbackItem;

                // Not in the list, make a new one
                playbackItem = MediaItem.ToPlaybackItem();
                RaisePropertyChanged("PlaybackItem");
                return playbackItem;
            }
        }

        public MediaItemViewModel(MediaListViewModel listViewModel, LocalMusicModel mediaItem)
        {
            this.listViewModel = listViewModel;
            MediaItem = mediaItem;

            RaisePropertyChanged("Title");

            // This app caches all images by loading the WriteableBitmap
            // when the item is created, but production apps would
            // use a more resource friendly paging mechanism or
            // just use PreviewImageUri directly.
            //
            // The reason we cache here is to avoid audio gaps 
            // between tracks on transitions when changing artwork.

            PreviewImage = mediaItem.Image;
        }

    }
}
