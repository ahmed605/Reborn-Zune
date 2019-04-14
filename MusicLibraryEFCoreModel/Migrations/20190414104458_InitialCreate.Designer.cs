﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicLibraryEFCoreModel;

namespace MusicLibraryEFCoreModel.Migrations
{
    [DbContext(typeof(MusicLibraryDbContext))]
    [Migration("20190414104458_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062");

            modelBuilder.Entity("MusicLibraryEFCoreModel.Album", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.Property<string>("ArtistId")
                        .IsRequired()
                        .HasColumnName("ArtistId")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("Title")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.ToTable("Album");
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.Artist", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Artist");
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.Music", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.Property<string>("AlbumId")
                        .HasColumnName("AlbumId")
                        .IsUnicode(true);

                    b.Property<string>("ArtistId")
                        .HasColumnName("ArtistId")
                        .IsUnicode(true);

                    b.Property<string>("Path")
                        .IsRequired()
                        .HasColumnName("Path")
                        .IsUnicode(true);

                    b.Property<string>("ThumbnailId")
                        .HasColumnName("ThumbnailId")
                        .IsUnicode(true);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("Title")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.HasIndex("AlbumId");

                    b.HasIndex("ArtistId");

                    b.HasIndex("Id")
                        .IsUnique()
                        .HasName("UQ__Music__AUQKR5RFNOUO6UFR");

                    b.HasIndex("ThumbnailId")
                        .IsUnique();

                    b.ToTable("Music");
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.MusicInPlaylist", b =>
                {
                    b.Property<string>("MusicId")
                        .HasColumnName("MusicId")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.Property<string>("PlaylistId")
                        .HasColumnName("PlaylistId")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.HasKey("MusicId", "PlaylistId");

                    b.HasIndex("PlaylistId");

                    b.ToTable("MusicInPlaylist");
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.Playlist", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnName("Name")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Playlst");
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.Thumbnail", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("Id")
                        .HasMaxLength(450)
                        .IsUnicode(true);

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasColumnName("Image")
                        .HasColumnType("BLOB")
                        .IsUnicode(true);

                    b.HasKey("Id");

                    b.ToTable("Thumbnail");
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.Album", b =>
                {
                    b.HasOne("MusicLibraryEFCoreModel.Artist", "Artist")
                        .WithMany("Albums")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.Music", b =>
                {
                    b.HasOne("MusicLibraryEFCoreModel.Album", "Album")
                        .WithMany("Musics")
                        .HasForeignKey("AlbumId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicLibraryEFCoreModel.Artist", "Artist")
                        .WithMany("Musics")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicLibraryEFCoreModel.Thumbnail", "Thumbnail")
                        .WithOne("Music")
                        .HasForeignKey("MusicLibraryEFCoreModel.Music", "ThumbnailId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("MusicLibraryEFCoreModel.MusicInPlaylist", b =>
                {
                    b.HasOne("MusicLibraryEFCoreModel.Music", "Music")
                        .WithMany("MusicInPlaylists")
                        .HasForeignKey("MusicId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MusicLibraryEFCoreModel.Playlist", "Playlist")
                        .WithMany("MusicInPlaylists")
                        .HasForeignKey("PlaylistId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
