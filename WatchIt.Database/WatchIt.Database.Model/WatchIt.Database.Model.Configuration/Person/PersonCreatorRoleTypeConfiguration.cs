using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Person;

namespace WatchIt.Database.Model.Configuration.Person;

public class PersonCreatorRoleTypeConfiguration : IEntityTypeConfiguration<PersonCreatorRoleType>
{
    public void Configure(EntityTypeBuilder<PersonCreatorRoleType> builder)
    {
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();

        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();
    }
}