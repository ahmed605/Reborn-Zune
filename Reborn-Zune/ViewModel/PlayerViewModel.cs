using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace Reborn_Zune.ViewModel
{
    class PlayerViewModel : ViewModelBase, IDisposable
    {
        bool disposed;
        MediaPlayer player;
        CoreDispatcher dispatcher;
        MediaPlaybackList subscribedPlaybackList;
        MediaListViewModel mediaList;

        bool canSkipNext;
        bool canSkipPrevious;

        public PlaybackSessionViewModel PlaybackSession { get; private set; }

        public PlayerViewModel(MediaPlayer player, CoreDispatcher dispatcher)
        {
            this.player = player;
            this.dispatcher = dispatcher;
            this.player.AudioCategory = MediaPlayerAudioCategory.Media;
        }

        public bool CanSkipNext
        {
            get { return canSkipNext; }
            set
            {
                if (canSkipNext != value)
                {
                    canSkipNext = value;
                    RaisePropertyChanged("CanSkipNext");
                }
            }
        }

        public bool CanSkipPrevious
        {
            get { return canSkipPrevious; }
            set
            {
                if (canSkipPrevious != value)
                {
                    canSkipPrevious = value;
                    RaisePropertyChanged("CanSkipPrevious");
                }
            }
        }

        public MediaListViewModel MediaList
        {
            get { return mediaList; }
            set
            {
                if (mediaList != value)
                {
                    if (subscribedPlaybackList != null)
                    {
                        subscribedPlaybackList.CurrentItemChanged -= SubscribedPlaybackList_CurrentItemChanged;
                        subscribedPlaybackList.Items.VectorChanged -= Items_VectorChanged;
                        subscribedPlaybackList = null;
                    }

                    mediaList = value;

                    if (mediaList != null)
                    {
                        if (player.Source != mediaList.PlaybackList)
                            player.Source = mediaList.PlaybackList;

                        subscribedPlaybackList = mediaList.PlaybackList;
                        subscribedPlaybackList.CurrentItemChanged += SubscribedPlaybackList_CurrentItemChanged;
                        subscribedPlaybackList.Items.VectorChanged += Items_VectorChanged;
                        HandlePlaybackListChanges(subscribedPlaybackList.Items);
                    }
                    else
                    {
                        CanSkipNext = false;
                        CanSkipPrevious = false;
                    }

                    RaisePropertyChanged("MediaList");
                }
            }
        }

        private void HandlePlaybackListChanges(IObservableVector<MediaPlaybackItem> items)
        {
            throw new NotImplementedException();
        }

        private void Items_VectorChanged(IObservableVector<MediaPlaybackItem> sender, IVectorChangedEventArgs @event)
        {
            throw new NotImplementedException();
        }

        private void SubscribedPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
