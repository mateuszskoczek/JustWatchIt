namespace WatchIt.Database.Model.Accounts;

public class AccountRefreshToken
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public long AccountId { get; set; }
    public DateTimeOffset ExpirationDate { get; set; }
    public bool IsExtendable { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION

    // Account
    public virtual Account Account { get; set; } = default!;

    #endregion
}