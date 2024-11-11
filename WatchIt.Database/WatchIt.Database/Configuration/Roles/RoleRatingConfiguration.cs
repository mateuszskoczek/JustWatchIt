using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database.Configuration.Roles;

public class RoleRatingConfiguration : RatingEntityConfiguration<RoleRating>
{
    #region PUBLIC METHODS
    
    public override void Configure(EntityTypeBuilder<RoleRating> builder)
    {
        builder.ToTable("RoleRatings", "roles");
        
        // Role
        // FK configured in RoleConfiguration
        builder.Property(x => x.RoleId)
               .IsRequired();
        
        // Generic properties
        base.Configure(builder);
    }
    
    #endregion
}