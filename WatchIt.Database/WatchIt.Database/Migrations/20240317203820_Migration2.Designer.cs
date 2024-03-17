﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WatchIt.Database;

#nullable disable

namespace WatchIt.Database.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20240317203820_Migration2")]
    partial class Migration2
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WatchIt.Database.Model.Account.Account", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<Guid?>("BackgroundPictureId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("character varying(320)");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastActive")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("LeftSalt")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<byte[]>("Password")
                        .IsRequired()
                        .HasMaxLength(1000)
                        .HasColumnType("bytea");

                    b.Property<Guid?>("ProfilePictureId")
                        .HasColumnType("uuid");

                    b.Property<string>("RightSalt")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.HasKey("Id");

                    b.HasIndex("BackgroundPictureId");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("ProfilePictureId")
                        .IsUnique();

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Account.AccountProfilePicture", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasMaxLength(-1)
                        .HasColumnType("bytea");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("UploadDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("AccountProfilePictures");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Genre.Genre", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<short>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            Name = "Comedy"
                        },
                        new
                        {
                            Id = (short)2,
                            Name = "Thriller"
                        },
                        new
                        {
                            Id = (short)3,
                            Name = "Horror"
                        });
                });

            modelBuilder.Entity("WatchIt.Database.Model.Genre.GenreMedia", b =>
                {
                    b.Property<short>("GenreId")
                        .HasColumnType("smallint");

                    b.Property<long>("MediaId")
                        .HasColumnType("bigint");

                    b.HasKey("GenreId", "MediaId");

                    b.HasIndex("MediaId");

                    b.ToTable("GenresMedia");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Media.Media", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<TimeSpan?>("Length")
                        .HasColumnType("interval");

                    b.Property<Guid?>("MediaPosterImageId")
                        .HasColumnType("uuid");

                    b.Property<string>("OriginalTitle")
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.Property<DateTime?>("ReleaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("character varying(250)");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("MediaPosterImageId")
                        .IsUnique();

                    b.ToTable("Media");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Media.MediaPhotoImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasMaxLength(-1)
                        .HasColumnType("bytea");

                    b.Property<bool>("IsMediaBackground")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsUniversalBackground")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<long>("MediaId")
                        .HasColumnType("bigint");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("UploadDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.HasIndex("MediaId");

                    b.ToTable("MediaPhotoImages");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Media.MediaPosterImage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<byte[]>("Image")
                        .IsRequired()
                        .HasMaxLength(-1)
                        .HasColumnType("bytea");

                    b.Property<string>("MimeType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<DateTime>("UploadDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id");

                    b.HasIndex("Id")
                        .IsUnique();

                    b.ToTable("MediaPosterImages");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Account.Account", b =>
                {
                    b.HasOne("WatchIt.Database.Model.Media.MediaPhotoImage", "BackgroundPicture")
                        .WithMany()
                        .HasForeignKey("BackgroundPictureId");

                    b.HasOne("WatchIt.Database.Model.Account.AccountProfilePicture", "ProfilePicture")
                        .WithOne("Account")
                        .HasForeignKey("WatchIt.Database.Model.Account.Account", "ProfilePictureId");

                    b.Navigation("BackgroundPicture");

                    b.Navigation("ProfilePicture");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Genre.GenreMedia", b =>
                {
                    b.HasOne("WatchIt.Database.Model.Genre.Genre", "Genre")
                        .WithMany("GenreMedia")
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WatchIt.Database.Model.Media.Media", "Media")
                        .WithMany("GenreMedia")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("Media");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Media.Media", b =>
                {
                    b.HasOne("WatchIt.Database.Model.Media.MediaPosterImage", "MediaPosterImage")
                        .WithOne("Media")
                        .HasForeignKey("WatchIt.Database.Model.Media.Media", "MediaPosterImageId");

                    b.Navigation("MediaPosterImage");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Media.MediaPhotoImage", b =>
                {
                    b.HasOne("WatchIt.Database.Model.Media.Media", "Media")
                        .WithMany("MediaPhotoImages")
                        .HasForeignKey("MediaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Media");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Account.AccountProfilePicture", b =>
                {
                    b.Navigation("Account")
                        .IsRequired();
                });

            modelBuilder.Entity("WatchIt.Database.Model.Genre.Genre", b =>
                {
                    b.Navigation("GenreMedia");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Media.Media", b =>
                {
                    b.Navigation("GenreMedia");

                    b.Navigation("MediaPhotoImages");
                });

            modelBuilder.Entity("WatchIt.Database.Model.Media.MediaPosterImage", b =>
                {
                    b.Navigation("Media")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
