using WatchIt.Common.Model.Generics.Rating;

namespace WatchIt.Common.Model.People.Person;

public class PersonUserRatedResponse : PersonResponse
{
    #region PROPERTIES
    
    public RatingUserOverallResponse RatingUser { get; set; }
    
    #endregion
}