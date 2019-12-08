﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    [DbContext(typeof(MusicLibraryDbContext))]
    partial class MusicLibraryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.ArtistEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Artists");
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.DiscEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("AlbumId")
                        .HasColumnType("text");

                    b.Property<int?>("AlbumOrder")
                        .HasColumnType("integer");

                    b.Property<string>("AlbumTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DeleteComment")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DeleteDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("FolderId")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("TreeTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("FolderId");

                    b.ToTable("Discs");
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.FolderEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("ParentFolderId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ParentFolderId", "Name")
                        .IsUnique();

                    b.ToTable("Folders");
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.GenreEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.PlaybackEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("PlaybackTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("SongId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SongId");

                    b.ToTable("Playbacks");
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.SongEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ArtistId")
                        .HasColumnType("integer");

                    b.Property<int?>("BitRate")
                        .HasColumnType("integer");

                    b.Property<string>("DeleteComment")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DeleteDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("DiscId")
                        .HasColumnType("integer");

                    b.Property<TimeSpan>("Duration")
                        .HasColumnType("interval");

                    b.Property<int?>("GenreId")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset?>("LastPlaybackTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("PlaybacksCount")
                        .HasColumnType("integer");

                    b.Property<int?>("Rating")
                        .HasColumnType("integer");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<short?>("TrackNumber")
                        .HasColumnType("smallint");

                    b.Property<string>("TreeTitle")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ArtistId");

                    b.HasIndex("DiscId");

                    b.HasIndex("GenreId");

                    b.ToTable("Songs");
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.DiscEntity", b =>
                {
                    b.HasOne("MusicLibraryApi.Dal.EfCore.Entities.FolderEntity", "Folder")
                        .WithMany("Discs")
                        .HasForeignKey("FolderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.FolderEntity", b =>
                {
                    b.HasOne("MusicLibraryApi.Dal.EfCore.Entities.FolderEntity", "ParentFolder")
                        .WithMany("Subfolders")
                        .HasForeignKey("ParentFolderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.PlaybackEntity", b =>
                {
                    b.HasOne("MusicLibraryApi.Dal.EfCore.Entities.SongEntity", "Song")
                        .WithMany("Playbacks")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicLibraryApi.Dal.EfCore.Entities.SongEntity", b =>
                {
                    b.HasOne("MusicLibraryApi.Dal.EfCore.Entities.ArtistEntity", "Artist")
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MusicLibraryApi.Dal.EfCore.Entities.DiscEntity", "Disc")
                        .WithMany("Songs")
                        .HasForeignKey("DiscId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MusicLibraryApi.Dal.EfCore.Entities.GenreEntity", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId");
                });
#pragma warning restore 612, 618
        }
    }
}
