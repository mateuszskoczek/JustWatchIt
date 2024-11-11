using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database.Configuration.Roles;

public class RoleActorConfiguration : IEntityTypeConfiguration<RoleActor>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<RoleActor> builder)
    {
        // Actor type
        builder.HasOne(x => x.ActorType)
               .WithMany(x => x.Roles)
               .HasForeignKey(x => x.ActorTypeId)
               .IsRequired();
        
        // Name
        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();
    }
    
    #endregion
}