using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Configuration.Accounts;

public class AccountProfilePictureConfiguration : ImageEntityConfiguration<AccountProfilePicture>
{
    #region PUBLIC METHODS
    
    public override void Configure(EntityTypeBuilder<AccountProfilePicture> builder)
    {
        builder.ToTable("AccountProfilePicture", "accounts");
        
        base.Configure(builder);
    }
    
    #endregion
}