namespace WatchIt.Common.Model.Generics.Rating;

public class RatingUserOverallResponse
{
    #region PROPERTIES
    
    public decimal? Average { get; set; }
    public long Count { get; set; }
    public DateTimeOffset? LastRatingDate { get; set; }
    
    #endregion
}