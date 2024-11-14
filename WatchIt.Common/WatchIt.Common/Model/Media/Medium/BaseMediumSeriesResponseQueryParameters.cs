using Microsoft.AspNetCore.Mvc;

namespace WatchIt.Common.Model.Media.Medium;

public abstract class BaseMediumSeriesResponseQueryParameters<T> : BaseMediumResponseQueryParameters<T> where T : MediumSeriesResponse
{
    #region PROPERTIES
    
    [FromQuery(Name = "has_ended")]
    public bool? HasEnded { get; set; }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    protected override bool IsMeetingConditions(T item) =>
    (
        base.IsMeetingConditions(item)
        &&
        Test(item.HasEnded, HasEnded)
    );
    
    #endregion
}