namespace WatchIt.Database.Model.Roles;

public class RoleCreator : Role
{
    #region PROPERTIES
    
    public short CreatorTypeId { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION
    
    // Creator type
    public virtual RoleCreatorType CreatorType { get; set; } = default!;
    
    #endregion
}