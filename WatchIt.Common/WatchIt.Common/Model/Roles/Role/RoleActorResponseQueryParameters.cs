using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Roles.Role;

public class RoleActorResponseQueryParameters : BaseRoleResponseQueryParameters<RoleActorResponse>
{
    #region CONSTANTS

    public override IDictionary<string, Func<RoleActorResponse, IComparable?>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<RoleActorResponse, IComparable?>> _partialOrderKeys = new Dictionary<string, Func<RoleActorResponse, IComparable?>>
    {
        { "type_id", x => x.TypeId },
        { "name", x => x.Name }
    };
    protected new static readonly IDictionary<string, Func<RoleActorResponse, IComparable?>> _orderKeys = BaseRoleResponseQueryParameters<RoleActorResponse>._orderKeys.Concat(_partialOrderKeys).ToDictionary(x => x.Key, x => x.Value);


    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "type_id")]
    public short? TypeId { get; set; }
    
    [FromQuery(Name = "name")]
    public string? Name { get; set; }
    
    #endregion
    
    
    
    #region PRIVATE METHODS
    
    protected override bool IsMeetingConditions(RoleActorResponse item) =>
    (
        base.IsMeetingConditions(item)
        &&
        Test(item.TypeId, TypeId)
        &&
        TestStringWithRegex(item.Name, Name)
    );

    #endregion
}