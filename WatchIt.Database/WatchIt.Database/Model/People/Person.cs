using WatchIt.Database.Model.Genders;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database.Model.People;

public class Person
{
    #region PROPERTIES

    public long Id { get; set; }
    public string Name { get; set; } = default!;
    public string? FullName { get; set; }
    public string? Description { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public short? GenderId { get; set; }

    #endregion
    
    
    
    #region NAVIGATION
    
    // Gender
    public virtual Gender? Gender { get; set; }
    
    // Picture
    public virtual PersonPicture? Picture { get; set; }
    
    // View counts
    public virtual IEnumerable<PersonViewCount> ViewCounts { get; set; } = [];
    
    // Roles
    public virtual IEnumerable<Role> Roles { get; set; } = [];
    
    #endregion
}