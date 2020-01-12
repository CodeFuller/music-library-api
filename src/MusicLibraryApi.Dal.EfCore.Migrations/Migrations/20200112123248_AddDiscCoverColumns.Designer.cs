﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MusicLibraryApi.Dal.EfCore;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace MusicLibraryApi.Dal.EfCore.Migrations.Migrations
{
    [DbContext(typeof(MusicLibraryDbContext))]
    [Migration("20200112123248_AddDiscCoverColumns")]
    partial class AddDiscCoverColumns
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Artist", b =>
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

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Disc", b =>
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

                    b.Property<long?>("CoverChecksum")
                        .HasColumnType("bigint");

                    b.Property<string>("CoverFileName")
                        .HasColumnType("text");

                    b.Property<long?>("CoverSize")
                        .HasColumnType("bigint");

                    b.Property<string>("DeleteComment")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("DeleteDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("FolderId")
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

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:IdentitySequenceOptions", "'2', '1', '', '', 'False', '1'")
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

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "<ROOT>"
                        });
                });

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Genre", b =>
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

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Playback", b =>
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

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Song", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int?>("ArtistId")
                        .HasColumnType("integer");

                    b.Property<int?>("BitRate")
                        .HasColumnType("integer");

                    b.Property<long?>("Checksum")
                        .HasColumnType("bigint");

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

                    b.Property<long?>("Size")
                        .HasColumnType("bigint");

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

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Disc", b =>
                {
                    b.HasOne("MusicLibraryApi.Abstractions.Models.Folder", "Folder")
                        .WithMany("Discs")
                        .HasForeignKey("FolderId")
                        .HasConstraintName("FK_Discs_Folders_FolderId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Folder", b =>
                {
                    b.HasOne("MusicLibraryApi.Abstractions.Models.Folder", "ParentFolder")
                        .WithMany("Subfolders")
                        .HasForeignKey("ParentFolderId")
                        .HasConstraintName("FK_Folders_Folders_ParentFolderId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Playback", b =>
                {
                    b.HasOne("MusicLibraryApi.Abstractions.Models.Song", "Song")
                        .WithMany("Playbacks")
                        .HasForeignKey("SongId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();
                });

            modelBuilder.Entity("MusicLibraryApi.Abstractions.Models.Song", b =>
                {
                    b.HasOne("MusicLibraryApi.Abstractions.Models.Artist", "Artist")
                        .WithMany("Songs")
                        .HasForeignKey("ArtistId")
                        .HasConstraintName("FK_Songs_Artists_ArtistId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("MusicLibraryApi.Abstractions.Models.Disc", "Disc")
                        .WithMany("Songs")
                        .HasForeignKey("DiscId")
                        .HasConstraintName("FK_Songs_Discs_DiscId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("MusicLibraryApi.Abstractions.Models.Genre", "Genre")
                        .WithMany("Songs")
                        .HasForeignKey("GenreId")
                        .HasConstraintName("FK_Songs_Genres_GenreId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}
