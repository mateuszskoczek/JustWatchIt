namespace WatchIt.Database.Model.Roles;

public class RoleActor : Role
{
    #region PROPERTIES
    
    public short ActorTypeId { get; set; }
    public string Name { get; set; } = default!;
    
    #endregion
    
    
    
    #region NAVIGATION
    
    // Actor type
    public virtual RoleActorType ActorType { get; set; } = default!;
    
    #endregion
}