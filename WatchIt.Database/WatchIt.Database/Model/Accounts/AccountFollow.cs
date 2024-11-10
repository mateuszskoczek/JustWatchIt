namespace WatchIt.Database.Model.Accounts;

public class AccountFollow
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public long FollowerId { get; set; }
    public long FollowedId { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION
    
    // Account follower
    public virtual Account Follower { get; set; } = default!;
    
    // Account followed
    public virtual Account Followed { get; set; } = default!;
    
    #endregion
}