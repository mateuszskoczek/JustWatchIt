using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.People;

namespace WatchIt.Database.Configuration.People;

public class PersonPictureConfiguration : ImageEntityConfiguration<PersonPicture>
{
    #region PUBLIC METHODS
    
    public override void Configure(EntityTypeBuilder<PersonPicture> builder)
    {
        builder.ToTable("PersonPictures", "people");
        
        // Person
        builder.HasKey(x => x.PersonId);
        builder.HasIndex(x => x.PersonId)
               .IsUnique();
        builder.HasOne(x => x.Person)
               .WithOne(x => x.Picture)
               .HasForeignKey<PersonPicture>(x => x.PersonId)
               .IsRequired();
        builder.Property(x => x.PersonId)
               .IsRequired();
        
        // Generic properties
        base.Configure(builder);
    }
    
    #endregion
}