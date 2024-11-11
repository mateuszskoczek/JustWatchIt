using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumMovieConfiguration : IEntityTypeConfiguration<MediumMovie>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<MediumMovie> builder)
    {
        // Budget
        builder.Property(x => x.Budget)
               .HasColumnType("money");
    }
    
    #endregion
}