using WatchIt.Common.Model.Generics.Rating;

namespace WatchIt.Common.Model.Media.Medium;

public class MediumSeriesUserRatedResponse : MediumSeriesResponse, IMediumUserRatedResponse
{
    #region PROPERTIES

    public RatingUserResponse? RatingUser { get; set; }
    
    #endregion
}