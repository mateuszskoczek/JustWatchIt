using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumSeriesConfiguration : IEntityTypeConfiguration<MediumSeries>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<MediumSeries> builder)
    {
        // Has ended
        builder.Property(x => x.HasEnded)
               .IsRequired()
               .HasDefaultValue(false);
    }
    
    #endregion
}