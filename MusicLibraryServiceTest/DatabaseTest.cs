
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MusicLibraryEFCoreModel;
using Windows.Storage;

namespace MusicLibraryServiceTest
{
    [TestClass]
    public class DatabaseTest
    {
        [TestMethod]
        public void DatabaseInitialize()
        {
            using(var _context = new MusicLibraryDbContext())
            {
                _context.Database.Migrate();
                Assert.IsTrue(_context.Database.CanConnect());
            }
        }

        [TestMethod]
        public void DatabaseInsert()
        {
            using (var _context = new MusicLibraryDbContext())
            {
                Assert.IsTrue(_context.Database.CanConnect());
            }
        }
    }
}
