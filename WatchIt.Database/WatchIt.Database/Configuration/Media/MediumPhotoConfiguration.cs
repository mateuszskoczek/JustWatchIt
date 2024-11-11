using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumPhotoConfiguration : ImageEntityConfiguration<MediumPhoto>
{
    #region PUBLIC METHODS

    public override void Configure(EntityTypeBuilder<MediumPhoto> builder)
    {
        builder.ToTable("MediumPhotos", "media");
        
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Medium
        builder.HasOne(x => x.Medium)
               .WithMany(x => x.Photos)
               .HasForeignKey(x => x.MediumId)
               .IsRequired();
        builder.Property(x => x.MediumId)
               .IsRequired();
        
        // Generic properties
        base.Configure(builder);
    }

    #endregion
}