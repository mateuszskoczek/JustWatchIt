using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database.Configuration.Roles;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles", "roles");
        builder.HasDiscriminator<RoleType>("Type")
               .HasValue<RoleActor>(RoleType.Actor)
               .HasValue<RoleCreator>(RoleType.Creator);
        
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Type
        builder.Property(x => x.Type)
               .IsRequired();
        
        // Medium
        builder.HasOne(x => x.Medium)
               .WithMany(x => x.Roles)
               .HasForeignKey(x => x.MediumId)
               .IsRequired();
        builder.Property(x => x.MediumId)
               .IsRequired();
        
        // Person
        builder.HasOne(x => x.Person)
               .WithMany(x => x.Roles)
               .HasForeignKey(x => x.PersonId)
               .IsRequired();
        builder.Property(x => x.PersonId)
               .IsRequired();


        #region Navigation
        
        // RoleRating
        builder.HasMany(x => x.RatedBy)
               .WithMany(x => x.RolesRated)
               .UsingEntity<RoleRating>(
                   x => x.HasOne(y => y.Account)
                         .WithMany(y => y.RolesRatings)
                         .HasForeignKey(y => y.AccountId)
                         .IsRequired(),
                   x => x.HasOne(y => y.Role)
                         .WithMany(y => y.Ratings)
                         .HasForeignKey(y => y.RoleId)
                         .IsRequired()
               );

        #endregion
    }
    
    #endregion
}