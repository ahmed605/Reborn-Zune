
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryEFCoreModel;
using MusicLibraryService;
using Windows.Storage;

namespace MusicLibraryServiceTest
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void DatabaseInitialize()
        {
            try
            {
                MusicLibrary.Initialize(true);
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
            
        }

        [TestMethod]
        public async Task DatabseInsertAndFetch()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Test.mp3"));
                MusicLibrary.AddSong(file);
                var result = MusicLibrary.FetchAll();
                Assert.IsTrue(result.musics.Count >= 1);
                Assert.IsTrue(result.albums.Count >= 1);
                Assert.IsTrue(result.artists.Count >= 1);
                Assert.IsTrue(result.thumbnails.Count >= 1);
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
        }

        [TestMethod]
        public void DatabaseDeleteSong()
        {
            try
            {
                MusicLibrary.DeleteSong("C:\\Users\\yinju\\source\\repos\\Reborn-Zune\\MusicLibraryServiceTest\\bin\\x86\\Debug\\AppX\\Assets\\Test.mp3");
                var result = MusicLibrary.FetchAll();
                Assert.IsTrue(result.musics.Count == 0);
            }
            catch(Exception e)
            {
                Assert.Fail();
            }

        }

        [TestMethod]
        public void DatabaseCreatePlaylist()
        {
            MusicLibrary.CreatePlaylist("playlist1");
            var result = MusicLibrary.FetchAll();
            Assert.IsTrue(result.playlists.Count == 1);
        }

        [TestMethod]
        public void DatabaseAddSongsToPlaylist()
        {
            List<Music> musics = new List<Music>
            {
                new Music
                {
                    Title = "A",
                    Path = "C:\\Users\\yinju\\source\\repos\\Reborn-Zune\\MusicLibraryServiceTest\\bin\\x86\\Debug\\AppX\\Assets\\Test.mp3"
                }
            };
            MusicLibrary.AddSongsToPlaylist("playlist1", musics);

            var result = MusicLibrary.FetchAll();
            Assert.IsTrue(result.playlists[0].MusicInPlaylists.Count == 1);
        }
    }
}
