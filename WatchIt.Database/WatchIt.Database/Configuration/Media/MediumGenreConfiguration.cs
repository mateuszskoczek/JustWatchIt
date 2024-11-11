using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumGenreConfiguration : IEntityTypeConfiguration<MediumGenre>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<MediumGenre> builder)
    {
        builder.ToTable("MediumGenres", "media");
        builder.HasKey(x => new { x.GenreId, x.MediumId });
        
        // Medium
        // FK configured in MediumConfiguration
        builder.Property(x => x.MediumId)
               .IsRequired();
        
        // Genre
        // FK configured in MediumConfiguration
        builder.Property(x => x.GenreId)
               .IsRequired();
    }

    #endregion
}