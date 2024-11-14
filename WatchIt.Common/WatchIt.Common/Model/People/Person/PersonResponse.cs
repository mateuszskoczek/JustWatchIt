using WatchIt.Common.Model.Genders.Gender;
using WatchIt.Common.Model.Generics.Rating;

namespace WatchIt.Common.Model.People.Person;

public class PersonResponse
{
    #region PROPERTIES
    
    public long Id { get; set; }
    public string Name { get; set; }
    public string? FullName { get; set; }
    public string? Description { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public GenderResponse? Gender { get; set; }
    public RatingOverallResponse Rating { get; set; }
    
    #endregion
}