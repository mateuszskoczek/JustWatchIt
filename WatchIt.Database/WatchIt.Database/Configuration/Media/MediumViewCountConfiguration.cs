using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumViewCountConfiguration : ViewCountEntityConfiguration<MediumViewCount>
{
    #region PUBLIC METHODS

    public override void Configure(EntityTypeBuilder<MediumViewCount> builder)
    {
        builder.ToTable("MediumViewCounts", "media");
        
        // Medium
        builder.HasOne(x => x.Medium)
               .WithMany(x => x.ViewCounts)
               .HasForeignKey(x => x.MediumId)
               .IsRequired();
        builder.Property(x => x.MediumId)
               .IsRequired();
        
        // Generic properties
        base.Configure(builder);
    }

    #endregion
}