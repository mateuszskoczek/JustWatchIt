using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Converters;
using WatchIt.Database.Model.Photos;

namespace WatchIt.Database.Configuration.Media;

public class PhotoBackgroundConfiguration : IEntityTypeConfiguration<PhotoBackground>
{
    #region PUBLIC METHODS
    
    public void Configure(EntityTypeBuilder<PhotoBackground> builder)
    {
        builder.ToTable("PhotoBackground", "photos");
        
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Photo
        builder.HasOne(x => x.Photo)
               .WithOne(x => x.BackgroundSettings)
               .HasForeignKey<PhotoBackground>(x => x.PhotoId)
               .IsRequired();
        builder.HasIndex(x => x.PhotoId)
               .IsUnique();
        builder.Property(x => x.PhotoId)
               .IsRequired();
        
        // Is universal
        builder.Property(x => x.IsUniversal)
               .IsRequired()
               .HasDefaultValue(false);
        
        // First gradient color
        builder.Property(x => x.FirstGradientColor)
               .HasConversion<ColorToByteArrayConverter>()
               .IsRequired();
        
        // Second gradient color
        builder.Property(x => x.SecondGradientColor)
               .HasConversion<ColorToByteArrayConverter>()
               .IsRequired();
    }
    
    #endregion
}