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
        
        // Account
        builder.HasKey(x => x.AccountId);
        builder.HasIndex(x => x.AccountId)
               .IsUnique();
        builder.HasOne(x => x.Account)
               .WithOne(x => x.ProfilePicture)
               .HasForeignKey<AccountProfilePicture>(x => x.AccountId)
               .IsRequired();
        builder.Property(x => x.AccountId)
               .IsRequired();
        
        // Generic properties
        base.Configure(builder);
    }
    
    #endregion
}