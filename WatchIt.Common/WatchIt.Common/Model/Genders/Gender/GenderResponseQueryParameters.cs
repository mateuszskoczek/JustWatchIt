using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;
using WatchIt.Common.Model.Genders;

namespace WatchIt.Common.Model.Genders.Gender;

public class GenderResponseQueryParameters : QueryParameters<GenderResponse>
{
    #region CONSTANTS

    public override IDictionary<string, Func<GenderResponse, IComparable>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<GenderResponse, IComparable>> _orderKeys = new Dictionary<string, Func<GenderResponse, IComparable>>
    {
        { "id", x => x.Id },
        { "name", item => item.Name }
    };

    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "name")]
    public string? Name { get; set; }
    
    #endregion
    
    
    
    #region PRIVATE METHODS

    protected override bool IsMeetingConditions(GenderResponse item) => 
    (
        TestStringWithRegex(item.Name, Name)
    );

    #endregion
}