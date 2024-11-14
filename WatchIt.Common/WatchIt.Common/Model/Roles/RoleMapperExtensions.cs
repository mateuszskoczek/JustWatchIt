using WatchIt.Common.Model.Roles.Role;

namespace WatchIt.Common.Model.Roles;

public static class RoleMapperExtensions
{
    #region PUBLIC METHODS

    #region Roles

    public static RoleActorResponse ToRoleActorResponse(this Database.Model.Roles.RoleActor entity)
    { 
        RoleActorResponse response = new RoleActorResponse();
        response.SetRoleResponseProperties(entity);
        response.Name = entity.Name;
        response.TypeId = entity.ActorTypeId;
        return response;
    }
    
    public static RoleCreatorResponse ToRoleCreatorResponse(this Database.Model.Roles.RoleCreator entity)
    { 
        RoleCreatorResponse response = new RoleCreatorResponse();
        response.SetRoleResponseProperties(entity);
        response.TypeId = entity.CreatorTypeId;
        return response;
    }

    #endregion
    
    #endregion
    
    
    
    #region PRIVATE METHODS
    
    private static void SetRoleResponseProperties(this RoleResponse response, Database.Model.Roles.Role entity)
    {
        response.Id = entity.Id;
        response.PersonId = entity.PersonId;
        response.MediumId = entity.MediumId;
    }
    
    #endregion
}