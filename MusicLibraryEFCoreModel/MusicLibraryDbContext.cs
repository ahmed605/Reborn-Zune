using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace MusicLibraryEFCoreModel
{
    public class MusicLibraryDbContext : DbContext
    {
        public DbSet<Music> Musics { get; set; }
        public DbSet<Thumbnail> Thumbnails { get; set; }
        public DbSet<Album> Albums { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Playlist> Playlists { get; set; }
        public DbSet<MusicInPlaylist> MusicInPlaylists { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=musiclibray.db");
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Music>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<string>(e => e.Path)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Path");

                entity.Property<string>(e => e.Title)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Title");

                entity.Property<string>(e => e.ThumbnailId)
                    .IsUnicode()
                    .HasColumnName("ThumbnailId");

                entity.Property<string>(e => e.AlbumId)
                    .IsUnicode()
                    .HasColumnName("AlbumId");

                entity.Property<string>(e => e.ArtistId)
                    .IsUnicode()
                    .HasColumnName("ArtistId");

                entity.HasKey(a => a.Id);

                entity.HasIndex("Id")
                    .IsUnique()
                    .HasName("UQ__Music__AUQKR5RFNOUO6UFR");

                entity.ToTable("Music");

                entity.HasOne(m => m.Album)
                    .WithMany(a => a.Musics)
                    .HasForeignKey(a => a.AlbumId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Artist)
                    .WithMany(a => a.Musics)
                    .HasForeignKey(a => a.ArtistId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Album>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<string>(e => e.Title)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Title");

                entity.Property<string>(e => e.ArtistId)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("ArtistId");

                entity.HasKey("Id");

                entity.ToTable("Album");

                entity.HasOne(a => a.Artist)
                    .WithMany(a => a.Albums)
                    .HasForeignKey(a => a.ArtistId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Artist>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<string>(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Name");

                entity.HasKey("Id");

                entity.ToTable("Artist");
            });

            builder.Entity<Playlist>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<string>(e => e.Name)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnName("Name");

                entity.HasKey("Id");

                entity.ToTable("Playlst");
            });

            builder.Entity<MusicInPlaylist>(entity =>
            {
            entity.Property<string>(e => e.MusicId)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(450)
                .HasColumnName("MusicId");

            entity.Property<string>(e => e.PlaylistId)
                .IsRequired()
                .IsUnicode()
                .HasMaxLength(450)
                .HasColumnName("PlaylistId");

            entity.HasKey(new string[] { "MusicId", "PlaylistId"});

                entity.ToTable("MusicInPlaylist");

                entity.HasOne(mp => mp.Music)
                    .WithMany(m => m.MusicInPlaylists)
                    .HasForeignKey(mp => mp.MusicId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(mp => mp.Playlist)
                    .WithMany(p => p.MusicInPlaylists)
                    .HasForeignKey(mp => mp.PlaylistId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Thumbnail>(entity =>
            {
                entity.Property<string>(e => e.Id)
                    .IsRequired()
                    .IsUnicode()
                    .HasMaxLength(450)
                    .HasColumnName("Id");

                entity.Property<byte[]>(e => e.Image)
                    .IsRequired()
                    .IsUnicode()
                    .HasColumnType("BLOB")
                    .HasColumnName("Image");

                entity.HasKey("Id");

                entity.ToTable("Thumbnail");

                entity.HasOne<Music>(t => t.Music)
                    .WithOne(m => m.Thumbnail)
                    .HasForeignKey<Music>(m => m.ThumbnailId)
                    .OnDelete(DeleteBehavior.Cascade);

            });
        }
    }
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
    }

    

   

    
}
