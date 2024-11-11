using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database.Configuration.Roles;

public class RoleCreatorTypeConfiguration : IEntityTypeConfiguration<RoleCreatorType>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<RoleCreatorType> builder)
    {
        builder.ToTable("RoleCreatorTypes", "roles");
        
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Name
        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();
    }

    #endregion
}