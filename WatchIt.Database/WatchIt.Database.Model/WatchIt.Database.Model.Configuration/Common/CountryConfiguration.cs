using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Common;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Model.Configuration.Common;

public class CountryConfiguration : IEntityTypeConfiguration<Country>
{
    public void Configure(EntityTypeBuilder<Country> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.IsHistorical)
               .HasDefaultValue(false)
               .IsRequired();

        // Navigation
        builder.HasMany(x => x.MediaProduction)
               .WithMany(x => x.ProductionCountries)
               .UsingEntity<MediaProductionCountry>();
    }
}