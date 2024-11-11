using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database.Configuration.Roles;

public class RoleActorTypeConfiguration : IEntityTypeConfiguration<RoleActorType>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<RoleActorType> builder)
    {
        builder.ToTable("RoleActorTypes", "roles");
        
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