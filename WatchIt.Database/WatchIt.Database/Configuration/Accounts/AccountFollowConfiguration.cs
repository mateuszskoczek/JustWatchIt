using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Configuration.Accounts;

public class AccountFollowConfiguration : IEntityTypeConfiguration<AccountFollow>
{
    #region PUBLIC METHODS
    
    public void Configure(EntityTypeBuilder<AccountFollow> builder)
    {
        builder.ToTable("AccountFollows", "accounts");
        builder.HasKey(x => new { x.FollowerId, x.FollowedId });
        
        // Follower
        // FK configured in AccountConfiguration
        builder.Property(x => x.FollowerId)
               .IsRequired();
        
        // Followed
        // FK configured in AccountConfiguration
        builder.Property(x => x.FollowedId)
               .IsRequired();
    }
    
    #endregion
}