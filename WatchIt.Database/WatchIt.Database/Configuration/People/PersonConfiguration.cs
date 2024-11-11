using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.People;

namespace WatchIt.Database.Configuration.People;

public class PersonConfiguration : IEntityTypeConfiguration<Person>
{
    #region PUBLIC METHODS
    
    public void Configure(EntityTypeBuilder<Person> builder)
    {
        builder.ToTable("People", "people");
        
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
        
        // Full name
        builder.Property(x => x.FullName)
               .HasMaxLength(200);
        
        // Description
        builder.Property(x => x.Description)
               .HasMaxLength(1000);
        
        // Birth date
        builder.Property(x => x.BirthDate);

        // Death date
        builder.Property(x => x.DeathDate);
        
        // Gender
        builder.HasOne(x => x.Gender)
               .WithMany(x => x.People)
               .HasForeignKey(x => x.GenderId);
        builder.Property(x => x.GenderId);
    }
    
    #endregion
}