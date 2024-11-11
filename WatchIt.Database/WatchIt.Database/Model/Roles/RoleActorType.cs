namespace WatchIt.Database.Model.Roles;

public class RoleActorType
{
    #region PROPERTIES
    
    public short Id { get; set; }
    public string Name { get; set; } = default!;
    
    #endregion
    
    
    
    #region NAVIGATION
    
    public virtual IEnumerable<RoleActor> Roles { get; set; } = [];
    
    #endregion
}