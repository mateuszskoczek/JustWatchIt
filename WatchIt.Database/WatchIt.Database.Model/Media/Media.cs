﻿using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchIt.Database.Model.Account;
using WatchIt.Database.Model.Genre;

namespace WatchIt.Database.Model.Media
{
    public class Media : IEntity<Media>
    {
        #region PROPERTIES

        public long Id { get; set; }
        public string Title { get; set; }
        public string? OriginalTitle { get; set; }
        public string? Description { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public TimeSpan? Length { get; set; }
        public Guid? MediaPosterImageId { get; set; }

        #endregion



        #region NAVIGATION

        public MediaPosterImage? MediaPosterImage { get; set; }
        public ICollection<MediaPhotoImage> MediaPhotoImages { get; set; }
        public ICollection<GenreMedia> GenreMedia { get; set; }
        public ICollection<Genre.Genre> Genres { get; set; }

        #endregion



        #region PUBLIC METHODS

        static void IEntity<Media>.Build(EntityTypeBuilder<Media> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasIndex(x => x.Id)
                   .IsUnique();
            builder.Property(x => x.Id)
                   .IsRequired();

            builder.Property(x => x.Title)
                   .HasMaxLength(250)
                   .IsRequired();

            builder.Property(x => x.OriginalTitle)
                   .HasMaxLength(250);

            builder.Property(x => x.Description)
                   .HasMaxLength(1000);

            builder.Property(x => x.ReleaseDate);

            builder.Property(x => x.Length);

            builder.HasOne(x => x.MediaPosterImage)
                   .WithOne(x => x.Media)
                   .HasForeignKey<Media>(x => x.MediaPosterImageId);
            builder.Property(x => x.MediaPosterImageId);

            // Navigation
            builder.HasMany(x => x.Genres)
                   .WithMany(x => x.Media)
                   .UsingEntity<GenreMedia>();
        }

        #endregion
    }
}
