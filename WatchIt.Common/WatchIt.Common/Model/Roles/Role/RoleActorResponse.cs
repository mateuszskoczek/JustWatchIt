using WatchIt.Database.Model.Roles;

namespace WatchIt.Common.Model.Roles.Role;

public class RoleActorResponse : RoleResponse
{
    #region PROPERTIES

    public string Name { get; set; }
    public short TypeId { get; set; }

    #endregion
}