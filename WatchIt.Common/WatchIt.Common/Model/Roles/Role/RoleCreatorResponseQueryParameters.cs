using Microsoft.AspNetCore.Mvc;

namespace WatchIt.Common.Model.Roles.Role;

public class RoleCreatorResponseQueryParameters : BaseRoleResponseQueryParameters<RoleCreatorResponse>
{
    #region CONSTANTS

    public override IDictionary<string, Func<RoleCreatorResponse, IComparable?>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<RoleCreatorResponse, IComparable?>> _partialOrderKeys = new Dictionary<string, Func<RoleCreatorResponse, IComparable?>>
    {
        { "type_id", x => x.TypeId },
    };
    protected new static readonly IDictionary<string, Func<RoleCreatorResponse, IComparable?>> _orderKeys = BaseRoleResponseQueryParameters<RoleCreatorResponse>._orderKeys.Concat(_partialOrderKeys).ToDictionary(x => x.Key, x => x.Value);


    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "type_id")]
    public short? TypeId { get; set; }
    
    #endregion
    
    
    
    #region PRIVATE METHODS
    
    protected override bool IsMeetingConditions(RoleCreatorResponse item) =>
    (
        base.IsMeetingConditions(item)
        &&
        Test(item.TypeId, TypeId)
    );

    #endregion
}