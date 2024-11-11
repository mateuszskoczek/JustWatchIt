using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database.Configuration.Roles;

public class RoleCreatorConfiguration : IEntityTypeConfiguration<RoleCreator>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<RoleCreator> builder)
    {
        // Creator type
        builder.HasOne(x => x.CreatorType)
               .WithMany(x => x.Roles)
               .HasForeignKey(x => x.CreatorTypeId)
               .IsRequired();
    }
    
    #endregion
}