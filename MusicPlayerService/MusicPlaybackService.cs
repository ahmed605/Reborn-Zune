using Reborn_Zune_Common.Interface;
using Reborn_Zune_Common.Services;
using Reborn_Zune_MusicLibraryService.DataModel;
using Reborn_Zune_MusicLibraryService.Interface;
using Reborn_Zune_MusicPlayerService.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace Reborn_Zune_MusicPlayerService
{
    public class MusicPlaybackService<T> : IService, IDisposable
        where T: IPlaybackItem
    {
        bool disposed;
        CoreDispatcher dispatcher;
        MediaPlaybackList subscribedPlaybackList;
        MediaListViewModel<T> mediaList;

        bool canSkipNext;
        bool canSkipPrevious;

        public PlaybackSessionViewModel PlaybackSession { get; private set; }
        
        public MediaPlayer Player { get; private set; }

        public MusicPlaybackService(CoreDispatcher dispatcher)
        {
            Player = new MediaPlayer();
            Player.AutoPlay = true;
            this.dispatcher = dispatcher;
            PlaybackSession = new PlaybackSessionViewModel(Player.PlaybackSession, dispatcher);
        }

        public bool CanSkipNext
        {
            get { return canSkipNext; }
            set
            {
                if (canSkipNext != value)
                {
                    canSkipNext = value;
                    //RaisePropertyChanged("CanSkipNext");
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
                    //RaisePropertyChanged("CanSkipPrevious");
                }
            }
        }

        public MediaPlaybackList PlaybackList
        {
            get { return Player.Source as MediaPlaybackList; }
            set { Player.Source = value; }
        }

        public MediaListViewModel<T> MediaList
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
                        if (Player.Source != mediaList.PlaybackList)
                            Player.Source = mediaList.PlaybackList;

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

                    //RaisePropertyChanged("MediaList");
                }
            }
        }

        private void HandlePlaybackListChanges(IObservableVector<MediaPlaybackItem> vector)
        {
            if (vector.Count > 0)
            {
                CanSkipNext = true;
                CanSkipPrevious = true;
            }
            else
            {
                CanSkipNext = false;
                CanSkipPrevious = false;
            }
        }

        private async void Items_VectorChanged(IObservableVector<MediaPlaybackItem> sender, IVectorChangedEventArgs @event)
        {
            if (disposed) return;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (disposed) return;
                HandlePlaybackListChanges(sender);
            });
        }

        private async void SubscribedPlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            if (disposed) return;
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                if (disposed) return;
                HandlePlaybackListChanges(sender.Items);
            });
        }

        public void Dispose()
        {
            if (disposed)
                return;

            if (MediaList != null)
            {
                MediaList.Dispose();
                MediaList = null; // Setter triggers vector unsubscribe logic
            }

            PlaybackSession.Dispose();

            disposed = true;
        }

        public void SetCurrentItem(int selectedIndex)
        {
            MediaList.CurrentItemIndex = selectedIndex;
        }

        public void Run()
        {
            //DO NOT ADD LINE OF CODE
        }

        public async Task<bool> AddToPlaybackQueue(List<T> mediaPlaybackList)
        {
            if (PlayBackListConsistencyDetect(mediaPlaybackList))
                PlaybackList = await ToPlayBackList(mediaPlaybackList);
            MediaList = new MediaListViewModel<T>(mediaPlaybackList, PlaybackList, dispatcher);
            return true;
        }

        private bool PlayBackListConsistencyDetect(List<T> currentList)
        {
            if (PlaybackList == null)
                return true;

            // Verify consistency of the lists that were passed in
            var mediaListIds = currentList.Select(i => i.GetId());
            var playbackListIds = PlaybackList.Items.Select(
                i => (string)i.Source.CustomProperties.SingleOrDefault(
                    p => p.Key == LocalMusicModel.MediaItemIdKey).Value);

            if (!mediaListIds.SequenceEqual(playbackListIds))
                return true;

            return false;

        }

        private async Task<MediaPlaybackList> ToPlayBackList(List<T> musics)
        {
            var playbackList = new MediaPlaybackList();

            // Add playback items to the list
            foreach (var mediaItem in musics)
            {
                playbackList.Items.Add(await mediaItem.ToPlaybackItem());
            }

            return playbackList;
        }
    }
}
