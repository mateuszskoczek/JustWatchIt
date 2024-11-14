using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Roles.Role;

public abstract class BaseRoleResponseQueryParameters<T> : QueryParameters<T> where T : RoleResponse
{
    #region CONSTANTS

    public override IDictionary<string, Func<T, IComparable?>>? OrderKeys => _orderKeys;
    protected static readonly IDictionary<string, Func<T, IComparable?>> _orderKeys = new Dictionary<string, Func<T, IComparable?>>
    {
        { "person_id", item => item.PersonId },
        { "medium_id", item => item.MediumId },
    };

    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "person_id")]
    public long? PersonId { get; set; }
    
    [FromQuery(Name = "medium_id")]
    public long? MediumId { get; set; }
    
    #endregion
    
    
    
    #region PRIVATE METHODS
    
    protected override bool IsMeetingConditions(T item) =>
    (
        Test(item.PersonId, PersonId)
        &&
        Test(item.MediumId, MediumId)
    );

    #endregion
}