using System;
using GalaSoft.MvvmLight;
using MusicLibraryService;

namespace Reborn_Zune.ViewModel
{
    public class LibraryViewModel : ViewModelBase
    {
        public LibraryViewModel()
        {
            GetFiles();
            
        }


        private void GetFiles()
        {
            _library = MusicLibrary.Fetch();
        }

        private Library _library;

        public Library Library
        {
            get { return _library; }
            set { _library = value; }
        }





    }
}