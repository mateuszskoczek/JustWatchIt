using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Model.Genders;

public class Gender
{
    #region PROPERTIES

    public short Id { get; set; }
    public string Name { get; set; } = default!;

    #endregion
    
    
    
    #region NAVIGATION
    
    // Accounts
    public virtual IEnumerable<Accounts.Account> Accounts { get; set; } = [];
    
    // People
    public virtual IEnumerable<People.Person> People { get; set; } = [];
    
    #endregion
}