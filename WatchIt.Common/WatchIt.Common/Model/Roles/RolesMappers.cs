using WatchIt.Common.Model.Roles.Role;

namespace WatchIt.Common.Model.Roles;

public static class RolesMappers
{
    #region PUBLIC METHODS

    #region Roles
    
    

    public static RoleActorResponse ToResponse(this Database.Model.Roles.RoleActor entity)
    { 
        RoleActorResponse response = new RoleActorResponse();
        response.SetRoleResponseProperties(entity);
        response.Name = entity.Name;
        response.TypeId = entity.ActorTypeId;
        return response;
    }
    
    public static RoleCreatorResponse ToResponse(this Database.Model.Roles.RoleCreator entity)
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