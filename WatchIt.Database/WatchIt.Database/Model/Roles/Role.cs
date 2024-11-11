using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Model.Roles;

public abstract class Role
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public RoleType Type { get; set; }
    public long MediumId { get; set; }
    public long PersonId { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION
    
    // Medium
    public virtual Medium Medium { get; set; } = default!;
    
    // Person
    public virtual People.Person Person { get; set; } = default!;
    
    // Ratings
    public virtual IEnumerable<RoleRating> Ratings { get; set; } = default!;
    public virtual IEnumerable<Accounts.Account> RatedBy { get; set; } = default!;
    
    #endregion
}