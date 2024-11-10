using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Model.Genders;

public class Gender
{
    #region PROPERTIES

    public short Id { get; set; }
    public string Name { get; set; } = default!;

    #endregion
    
    
    
    #region NAVIGATION
    
    // Account
    public virtual IEnumerable<Accounts.Account> Accounts { get; set; } = default!;
    
    #endregion
}