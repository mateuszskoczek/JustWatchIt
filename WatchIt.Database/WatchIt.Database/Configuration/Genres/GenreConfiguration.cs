using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Genres;

namespace WatchIt.Database.Configuration.Genres;

public class GenreConfiguration : IEntityTypeConfiguration<Genre>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<Genre> builder)
    {
        builder.ToTable("Genres", "genres");
        
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Name
        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();
    }

    #endregion
}