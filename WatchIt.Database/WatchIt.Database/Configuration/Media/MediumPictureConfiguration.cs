using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumPictureConfiguration : ImageEntityConfiguration<MediumPicture>
{
    #region PUBLIC METHODS
    
    public override void Configure(EntityTypeBuilder<MediumPicture> builder)
    {
        builder.ToTable("MediumPictures", "media");
        
        // Medium
        builder.HasKey(x => x.MediumId);
        builder.HasIndex(x => x.MediumId)
               .IsUnique();
        builder.HasOne(x => x.Medium)
               .WithOne(x => x.Picture)
               .HasForeignKey<MediumPicture>(x => x.MediumId)
               .IsRequired();
        builder.Property(x => x.MediumId)
               .IsRequired();
        
        // Generic properties
        base.Configure(builder);
    }
    
    #endregion
}