namespace WatchIt.Database.Model.Roles;

public class RoleRating : IRatingEntity
{
    #region PROPERTIES

    public Guid Id { get; set; }
    public Guid RoleId { get; set; }
    public long AccountId { get; set; }
    public byte Rating { get; set; }
    public DateTime Date { get; set; }

    #endregion
    
    
    
    #region NAVIGATION
    
    // Role
    public virtual Role Role { get; set; } = default!;
    
    // Account
    public virtual Accounts.Account Account { get; set; } = default!;
    
    #endregion
}