using WatchIt.Database.Model.Genders;

namespace WatchIt.Database.Model.Accounts;

public class Account
{
    #region PROPERTIES

    public long Id { get; set; }
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public byte[] Password { get; set; } = default!;
    public string LeftSalt { get; set; } = default!;
    public string RightSalt { get; set; } = default!;
    public bool IsAdmin { get; set; } = false;
    public DateTimeOffset JoinDate { get; set; }
    public DateTimeOffset ActiveDate { get; set; }
    public string? Description { get; set; }
    public short? GenderId { get; set; }

    #endregion
    
    
    
    #region NAVIGATION
    
    // Profile picture
    public virtual AccountProfilePicture? ProfilePicture { get; set; }
    
    // Refresh tokens
    public virtual IEnumerable<AccountRefreshToken> RefreshTokens { get; set; } = default!;
    
    // Follows
    public virtual IEnumerable<AccountFollow> FollowsRelationObjects { get; set; } = default!;
    public virtual IEnumerable<Account> Follows { get; set; } = default!;
    
    // Followers
    public virtual IEnumerable<AccountFollow> FollowersRelationObjects { get; set; } = default!;
    public virtual IEnumerable<Account> Followers { get; set; } = default!;
    
    // Gender
    public virtual Gender? Gender { get; set; }
    
    #endregion
}