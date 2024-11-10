using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Configuration.Accounts;

public class AccountRefreshTokenConfiguration : IEntityTypeConfiguration<AccountRefreshToken>
{
    #region PUBLIC METHODS

    public void Configure(EntityTypeBuilder<AccountRefreshToken> builder)
    {
        builder.ToTable("AccountRefreshTokens", "accounts");
        
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Account
        builder.HasOne(x => x.Account)
               .WithMany(x => x.RefreshTokens)
               .HasForeignKey(x => x.AccountId)
               .IsRequired();
        builder.Property(x => x.AccountId)
               .IsRequired();
        
        // Expiration date
        builder.Property(x => x.ExpirationDate)
               .IsRequired();
        
        // Is extendable
        builder.Property(x => x.IsExtendable)
               .IsRequired();
    }

    #endregion
}