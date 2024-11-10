using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Configuration.Accounts;

public class AccountFollowConfiguration : IEntityTypeConfiguration<AccountFollow>
{
    #region PUBLIC METHODS
    
    public void Configure(EntityTypeBuilder<AccountFollow> builder)
    {
        builder.ToTable("AccountFollow", "accounts");
        
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Account follower
        // FK configured in AccountConfiguration
        builder.Property(x => x.FollowerId)
               .IsRequired();
        
        // Account followed
        // FK configured in AccountConfiguration
        builder.Property(x => x.FollowedId)
               .IsRequired();
    }
    
    #endregion
}