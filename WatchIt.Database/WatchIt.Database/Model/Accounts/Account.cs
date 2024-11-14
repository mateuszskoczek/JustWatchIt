using WatchIt.Database.Model.Genders;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.Roles;

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
    
    // Background picture
    public virtual AccountBackgroundPicture? BackgroundPicture { get; set; }
    
    // Refresh tokens
    public virtual IEnumerable<AccountRefreshToken> RefreshTokens { get; set; } = [];
    
    // Follows
    public virtual IEnumerable<AccountFollow> FollowsRelationshipObjects { get; set; } = [];
    public virtual IEnumerable<Account> Follows { get; set; } = [];
    
    // Followers
    public virtual IEnumerable<AccountFollow> FollowersRelationshipObjects { get; set; } = [];
    public virtual IEnumerable<Account> Followers { get; set; } = [];
    
    // Gender
    public virtual Gender? Gender { get; set; }
    
    // Media ratings
    public virtual IEnumerable<MediumRating> MediaRatings { get; set; } = [];
    public virtual IEnumerable<Medium> MediaRated { get; set; } = [];
    
    // Roles ratings
    public virtual IEnumerable<RoleRating> RolesRatings { get; set; } = [];
    public virtual IEnumerable<Role> RolesRated { get; set; } = [];
    
    #endregion
}