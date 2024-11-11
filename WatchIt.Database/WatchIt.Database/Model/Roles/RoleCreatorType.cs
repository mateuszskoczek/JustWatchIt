namespace WatchIt.Database.Model.Roles;

public class RoleCreatorType
{
    #region PROPERTIES
    
    public short Id { get; set; }
    public string Name { get; set; } = default!;
    
    #endregion
    
    
    
    #region NAVIGATION
    
    public virtual IEnumerable<RoleCreator> Roles { get; set; } = [];
    
    #endregion
}