
using System;
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
                DataBaseService.Initialize();
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
                await DataBaseService.Add(file);
                var result = DataBaseService.FetchAll();
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
        public async Task DataBaseUpdate()
        {
            try
            {
                StorageFile file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Test.mp3"));
                await DataBaseService.Add(file);
                await DataBaseService.Update(file);
            }
            catch(Exception e)
            {
                Assert.Fail();
            }
        }
    }
}
