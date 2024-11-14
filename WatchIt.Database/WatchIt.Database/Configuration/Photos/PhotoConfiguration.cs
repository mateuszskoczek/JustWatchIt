using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Photos;

namespace WatchIt.Database.Configuration.Media;

public class PhotoConfiguration : ImageEntityConfiguration<Photo>
{
    #region PUBLIC METHODS

    public override void Configure(EntityTypeBuilder<Photo> builder)
    {
        builder.ToTable("Photos", "photos");
        
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