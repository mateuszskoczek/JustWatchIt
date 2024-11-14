using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Configuration.Accounts;

public class AccountBackgroundPictureConfiguration : IEntityTypeConfiguration<AccountBackgroundPicture>
{
    #region PUBLIC METHODS
    
    public void Configure(EntityTypeBuilder<AccountBackgroundPicture> builder)
    {
        builder.ToTable("AccountBackgroundPictures", "accounts");
        
        // Account
        builder.HasKey(x => x.AccountId);
        builder.HasIndex(x => x.AccountId)
               .IsUnique();
        builder.HasOne(x => x.Account)
               .WithOne(x => x.BackgroundPicture)
               .HasForeignKey<AccountBackgroundPicture>(x => x.AccountId)
               .IsRequired();
        builder.Property(x => x.AccountId)
               .IsRequired();
        
        // Background
        builder.HasOne(x => x.Background)
               .WithMany(x => x.BackgroundUsages)
               .HasForeignKey(x => x.BackgroundId)
               .IsRequired();
        builder.Property(x => x.BackgroundId)
               .IsRequired();
    }
    
    #endregion
}