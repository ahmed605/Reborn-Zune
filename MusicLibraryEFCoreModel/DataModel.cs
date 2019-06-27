using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace MusicLibraryEFCoreModel
{
    public class Music
    {
        public Music()
        {
           
        }

        public string Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string AlbumTitle { get; set; }
        public string AlbumArtist { get; set; }
        public string Artist { get; set; }
        public string Year { get; set; }
        public string ThumbnailId { get; set; }
        public Thumbnail Thumbnail { get; set; }
        [NotMapped]
        public StorageFile File { get; set; }
        
        public ICollection<MusicInPlaylist> MusicInPlaylists { get; set; }

    }
    public class Playlist
    {
        public Playlist()
        {
            
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<MusicInPlaylist> MusicInPlaylists { get; set; }

    }
    public class MusicInPlaylist
    {
        public string MusicId { get; set; }
        public string PlaylistId { get; set; }
        public Music Music { get; set; }
        public Playlist Playlist { get; set; }
    }
    public class Thumbnail
    {
        public Thumbnail()
        {
        }

        public string Id { get; set; }
        public byte[] ImageBytes { get; set; }

        [NotMapped]
        public BitmapImage Image { get; set; }
        public ICollection<Music> Musics { get; set; }

        public void GetBitmapImage()
        {
            if(ImageBytes.Length == 0)
            {
                Image = new BitmapImage(new Uri("ms-appx:///Assets/Vap-logo-placeholder.jpg"));
            }
            else
            {
                InMemoryRandomAccessStream randomAccessStream = new InMemoryRandomAccessStream();
                DataWriter writer = new DataWriter(randomAccessStream.GetOutputStreamAt(0));
                writer.WriteBytes(ImageBytes);
                writer.StoreAsync();
                Image = new BitmapImage();
                Image.SetSource(randomAccessStream);
            } 
        }
    }

    public class Album
    {

    }

    public class Artist
    {

    }
}
