using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Converters;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumPhotoBackgroundSettingsConfiguration : IEntityTypeConfiguration<MediumPhotoBackgroundSettings>
{
    #region PUBLIC METHODS
    
    public void Configure(EntityTypeBuilder<MediumPhotoBackgroundSettings> builder)
    {
        builder.ToTable("MediumPhotoBackgroundSettings", "media");
        
        // Id
        builder.HasKey(x => x.PhotoId);
        builder.HasOne(x => x.Photo)
               .WithOne(x => x.BackgroundSettings)
               .HasForeignKey<MediumPhotoBackgroundSettings>(x => x.PhotoId)
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