using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using MusicLibraryService;

namespace Reborn_Zune.ViewModel
{
    public class LibraryViewModel : ViewModelBase
    {
        public LibraryViewModel()
        {
            MusicLibrary.InitializeFinished += MusicLibrary_InitializeFinished;
        }

        private void MusicLibrary_InitializeFinished(object sender, EventArgs e)
        {
            Library = MusicLibrary.FetchAll();
        }

        private Library _library;

        public Library Library
        {
            get { return _library; }
            set { _library = value; }
        }



        
        
    }
}