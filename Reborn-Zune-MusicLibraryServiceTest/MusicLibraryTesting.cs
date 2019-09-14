using Microsoft.VisualStudio.TestTools.UnitTesting;
using Reborn_Zune_MusicLibraryService;
using System.Linq;

namespace Reborn_Zune_MusicLibraryServiceTest
{
    [TestClass]
    public class MusicLibraryTesting
    {
        [TestMethod]
        public void TestLibraryInstanceCreatedAndLoadedAndDeletedSuccessfully()
        {
            MusicLibraryService service = new MusicLibraryService();
            service.Run();
            Assert.IsNotNull(service.Library);
            service.Clean();
        }

        [TestMethod]
        public void TestMusicImageFileLoaded()
        {
            MusicLibraryService service = new MusicLibraryService();
            service.Run();
            service.Library.Thumbnails.ForEach(t => Assert.IsNotNull(t.Image));
            service.Library.Musics.ForEach(t => Assert.IsNotNull(t.File));
            service.Clean();
        }

        [TestMethod]
        public void TestCreatePlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            service.Run();
            service.CreatePlaylist("a");
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            service.Clean();
        }

        [TestMethod]
        public void TestEditPlaylistName()
        {
            MusicLibraryService service = new MusicLibraryService();
            service.Run();
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            service.EditPlaylistName("a", "b");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 0);
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "b") == 1);
            service.Clean();
        }

        [TestMethod]
        public void TestAddSongsIntoPlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            service.Run();
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            var songs = service.Library.Musics;
            service.AddSongsToPlaylist("a", songs);
            Assert.IsTrue(service.Library.MInP.Count == songs.Count);
            service.Clean();
        }

        [TestMethod]
        public void TestRemoveSongsIntoPlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            service.Run();
            service.CreatePlaylist("a");
            Assert.IsTrue(service.Library.Playlists.Count(p => p.Name == "a") == 1);
            var songs = service.Library.Musics;
            service.AddSongsToPlaylist("a", songs);
            Assert.IsTrue(service.Library.MInP.Count == songs.Count);
            service.RemoveSongsFromPlaylist("a", songs);
            Assert.IsTrue(service.Library.MInP.Count == 0);
            service.Clean();
        }

        [TestMethod]
        public void TestSameSongOnMuliplePlaylist()
        {
            MusicLibraryService service = new MusicLibraryService();
            service.Run();

            service.CreatePlaylist("a");
            service.CreatePlaylist("b");
            Assert.IsTrue(service.Library.Playlists.Count == 2);
            var songs = service.Library.Musics;
            service.AddSongsToPlaylist("a", songs);
            service.AddSongsToPlaylist("b", songs);

            Assert.IsTrue(service.Library.MInP.Count == songs.Count * 2);

            service.Clean();
        }
    }
}
