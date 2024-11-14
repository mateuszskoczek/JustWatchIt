namespace WatchIt.Common.Model.Roles.Role;

public abstract class RoleResponse
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public long MediumId { get; set; }
    public long PersonId { get; set; }
    
    #endregion
}