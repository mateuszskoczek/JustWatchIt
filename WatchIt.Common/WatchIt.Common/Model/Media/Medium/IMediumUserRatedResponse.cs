using WatchIt.Common.Model.Generics.Rating;

namespace WatchIt.Common.Model.Media.Medium;

public interface IMediumUserRatedResponse
{
    #region PROPERTIES
    
    RatingUserResponse? RatingUser { get; set; }
    
    #endregion
}