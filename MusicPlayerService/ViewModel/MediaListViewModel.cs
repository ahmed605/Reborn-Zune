﻿using Reborn_Zune_MusicLibraryService.DataModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Media.Playback;
using Windows.UI.Core;

namespace Reborn_Zune_MusicPlayerService.ViewModel
{
    public class MediaListViewModel : ObservableCollection<MediaItemViewModel>, IDisposable
    {
        CoreDispatcher dispatcher;
        int currentItemIndex = -1;
        bool disposed;
        bool initializing;

        public ObservableCollection<LocalMusicModel> MediaList { get; private set; }

        public MediaItemViewModel CurrentItem
        {
            get { return currentItemIndex == -1 ? null : this[currentItemIndex]; }
            set
            {
                if (value == null)
                {
                    CurrentItemIndex = -1;
                    return;
                }

                if (currentItemIndex == -1 || this[currentItemIndex] != value)
                {
                    CurrentItemIndex = IndexOf(value);
                }
            }
        }

        public int CurrentItemIndex
        {
            get
            {
                return currentItemIndex;
            }
            set
            {
                if (currentItemIndex != value)
                {
                    // Clamp invalid values
                    var min = -1;
                    var max = PlaybackList.Items.Count - 1;
                    currentItemIndex = (value < min) ? min : (value > max) ? max : value;

                    try
                    {
                        // This succeeds if the playlist has been bound to a player
                        if (currentItemIndex >= 0 && currentItemIndex != PlaybackList.CurrentItemIndex)
                            PlaybackList.MoveTo((uint)currentItemIndex);
                    }
                    catch
                    {
                        // Most likely the playlist had not been bound to a player so set start index
                        PlaybackList.StartingItem = CurrentItem.PlaybackItem;
                    }
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrentItemIndex"));
                    OnPropertyChanged(new PropertyChangedEventArgs("CurrentItem"));
                }
            }
        }

        public MediaPlaybackList PlaybackList
        {
            get;
            private set;
        }

        public MediaListViewModel(ObservableCollection<LocalMusicModel> mediaList, MediaPlaybackList playbackList, CoreDispatcher dispatcher)
        {
            MediaList = mediaList;
            PlaybackList = playbackList;
            this.dispatcher = dispatcher;

            // Verify consistency of the lists that were passed in
            var mediaListIds = mediaList.Select(i => i.Id);

            var playbackListIds = playbackList.Items.Select(
                i => (string)i.Source.CustomProperties.SingleOrDefault(
                    p => p.Key == LocalMusicModel.MediaItemIdKey).Value);
            var a = mediaListIds.ToList();
            var b = playbackListIds.ToList();

            // Initialize the view model items
            initializing = true;

            foreach (var mediaItem in mediaList)
                Add(new MediaItemViewModel(this, mediaItem));

            initializing = false;

            // The view model supports TwoWay binding so update when the playback list item changes
            PlaybackList.CurrentItemChanged += PlaybackList_CurrentItemChanged;

            // Start where the playback list is currently at
            CurrentItemIndex = (int)PlaybackList.CurrentItemIndex;
        }

        protected override void InsertItem(int index, MediaItemViewModel item)
        {
            base.InsertItem(index, item);

            // Don't add items during construction
            if (!initializing)
                PlaybackList.Items.Add(item.PlaybackItem);
        }

        protected override void ClearItems()
        {
            base.ClearItems();

            PlaybackList.Items.Clear();
        }

        protected override void RemoveItem(int index)
        {
            base.RemoveItem(index);

            PlaybackList.Items.RemoveAt(index);
        }

        protected override void SetItem(int index, MediaItemViewModel item)
        {
            base.SetItem(index, item);

            PlaybackList.Items[index] = item.PlaybackItem;
        }

        protected override void MoveItem(int oldIndex, int newIndex)
        {
            base.MoveItem(oldIndex, newIndex);

            var item = PlaybackList.Items[oldIndex];
            PlaybackList.Items.RemoveAt(oldIndex);
            PlaybackList.Items.Insert(newIndex, item);
        }

        /// <summary>
        /// This call comes in on a worker thread so requires a little extra care
        /// to avoid touching the UI if it has already been disposed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private async void PlaybackList_CurrentItemChanged(MediaPlaybackList sender, CurrentMediaPlaybackItemChangedEventArgs args)
        {
            // If the event was unsubscribed before 
            // this handler executed just return.
            if (disposed)
                return;

            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                // If the event was unsubscribed before 
                // this handler executed just return.
                if (disposed)
                    return;

                var playbackItem = args.NewItem;

                if (playbackItem == null)
                {
                    CurrentItem = null;
                }
                else
                {
                    // Find the single item in this list with a playback item
                    // matching the one we just received the event for.
                    CurrentItem = this.Single(mediaItemViewModel => mediaItemViewModel.PlaybackItem == playbackItem);
                }

            });
        }

        public void Dispose()
        {
            if (disposed)
                return;

            PlaybackList.CurrentItemChanged -= PlaybackList_CurrentItemChanged;

            disposed = true;
        }
    }
}
