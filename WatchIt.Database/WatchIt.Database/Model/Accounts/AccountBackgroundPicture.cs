using WatchIt.Database.Model.Photos;

namespace WatchIt.Database.Model.Accounts;

public class AccountBackgroundPicture
{
    #region PROPERTIES

    public long AccountId { get; set; }
    public Guid BackgroundId { get; set; }

    #endregion
    
    
    
    #region NAVIGATION
    
    // Account
    public virtual Account Account { get; set; } = default!;
    
    // Background
    public virtual PhotoBackground Background { get; set; } = default!;
    
    #endregion
}