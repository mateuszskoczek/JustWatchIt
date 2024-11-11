namespace WatchIt.Database.Model.Accounts;

public class AccountFollow
{
    #region PROPERTIES
    
    public long FollowerId { get; set; }
    public long FollowedId { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION
    
    // Follower
    public virtual Account Follower { get; set; } = default!;
    
    // Followed
    public virtual Account Followed { get; set; } = default!;
    
    #endregion
}