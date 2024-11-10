using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Configuration.Accounts;

public class AccountProfilePictureConfiguration : ImageEntityConfiguration<AccountProfilePicture>
{
    #region PUBLIC METHODS
    
    public override void Configure(EntityTypeBuilder<AccountProfilePicture> builder)
    {
        builder.ToTable($"AccountProfilePictures", "accounts");
        
        // Generic properties
        base.Configure(builder);
        
        // Account
        builder.HasOne(x => x.Account)
               .WithOne(x => x.ProfilePicture)
               .HasForeignKey<AccountProfilePicture>(x => x.AccountId)
               .IsRequired();
        builder.HasIndex(x => x.AccountId)
               .IsUnique();
        builder.Property(x => x.AccountId)
               .IsRequired();
    }
    
    #endregion
}