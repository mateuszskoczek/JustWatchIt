using WatchIt.Common.Model.Generics.Rating;

namespace WatchIt.Common.Model.Media.Medium;

public class MediumMovieUserRatedResponse : MediumMovieResponse, IMediumUserRatedResponse
{
    #region PROPERTIES
    
    public RatingUserResponse? RatingUser { get; set; }
    
    #endregion
}