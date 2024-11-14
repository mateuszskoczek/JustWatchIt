using Microsoft.AspNetCore.Mvc;

namespace WatchIt.Common.Model.Media.Medium;

public class MediumResponseQueryParameters : BaseMediumResponseQueryParameters<MediumResponse>
{
    #region PROPERTIES
    
    [FromQuery(Name = "type")]
    public MediumType? Type { get; set; }
    
    #endregion
    
    
    
    #region PUBLIC METHODS

    protected override bool IsMeetingConditions(MediumResponse item) =>
    (
        base.IsMeetingConditions(item)
        &&
        Test(item.Type, Type)
    );
    
    #endregion
}