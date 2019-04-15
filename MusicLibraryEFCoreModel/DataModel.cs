using System;
using System.Collections.Generic;
using System.Text;

namespace MusicLibraryEFCoreModel
{
    public class Music
    {
        public Music()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Path { get; set; }
        public string Title { get; set; }
        public string ThumbnailId { get; set; }
        public string AlbumId { get; set; }
        public string ArtistId { get; set; }
        public Album Album { get; set; }
        public Artist Artist { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public ICollection<MusicInPlaylist> MusicInPlaylists { get; set; }
    }

    public class Album
    {
        public Album()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Title { get; set; }
        public string ArtistId { get; set; }
        public string ThumbnailId { get; set; }
        public Thumbnail Thumbnail { get; set; }
        public Artist Artist { get; set; }
        public ICollection<Music> Musics { get; set; }
    }

    public class Artist
    {
        public Artist()
        {
            Id = Guid.NewGuid().ToString();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<Music> Musics { get; set; }
        public ICollection<Album> Albums { get; set; }
    }

    public class Playlist
    {
        public Playlist()
        {
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public ICollection<MusicInPlaylist> MusicInPlaylists { get; set; }

        public ICollection<Music> Musics { get; set; }
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
            Id = Guid.NewGuid().ToString();
        }

        public string Id { get; set; }
        public byte[] Image { get; set; }
        public Music Music { get; set; }
        public Album Album { get; set; }
    }
}
