using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.People.Person;

public abstract class BasePersonResponseQueryParameters<T> : QueryParameters<T> where T : PersonResponse
{
    #region CONSTANTS
    
    public override IDictionary<string, Func<T, IComparable?>>? OrderKeys => _orderKeys;
    protected static readonly IDictionary<string, Func<T, IComparable?>> _orderKeys = new Dictionary<string, Func<T, IComparable?>>
    {
        { "id", x => x.Id },
        { "name", x => x.Name },
        { "full_name", x => x.FullName },
        { "description", x => x.Description },
        { "birth_date", x => x.BirthDate },
        { "death_date", x => x.BirthDate },
        { "gender", x => x.Gender?.Name },
        { "rating.average", x => x.Rating.Average },
        { "rating.count", x => x.Rating.Count }
    };
    
    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    [FromQuery(Name = "full_name")]
    public string? FullName { get; set; }

    [FromQuery(Name = "description")]
    public string? Description { get; set; }

    [FromQuery(Name = "birth_date")]
    public DateOnly? BirthDate { get; set; }

    [FromQuery(Name = "birth_date_from")]
    public DateOnly? BirthDateFrom { get; set; }

    [FromQuery(Name = "birth_date_to")]
    public DateOnly? BirthDateTo { get; set; }

    [FromQuery(Name = "death_date")]
    public DateOnly? DeathDate { get; set; }

    [FromQuery(Name = "death_date_from")]
    public DateOnly? DeathDateFrom { get; set; }

    [FromQuery(Name = "death_date_to")]
    public DateOnly? DeathDateTo { get; set; }

    [FromQuery(Name = "gender_id")]
    public short? GenderId { get; set; }

    [FromQuery(Name = "rating_average")]
    public decimal? RatingAverage { get; set; }

    [FromQuery(Name = "rating_average_from")]
    public decimal? RatingAverageFrom { get; set; }

    [FromQuery(Name = "rating_average_to")]
    public decimal? RatingAverageTo { get; set; }

    [FromQuery(Name = "rating_count")]
    public long? RatingCount { get; set; }

    [FromQuery(Name = "rating_count_from")]
    public long? RatingCountFrom { get; set; }

    [FromQuery(Name = "rating_count_to")]
    public long? RatingCountTo { get; set; }
    
    #endregion
    
    
    
    #region PUBLIC METHODS

    protected override bool IsMeetingConditions(T item) =>
    (
        TestStringWithRegex(item.Name, Name)
        &&
        TestStringWithRegex(item.FullName, FullName)
        &&
        TestStringWithRegex(item.Description, Description)
        &&
        TestComparable(item.BirthDate, BirthDate, BirthDateFrom, BirthDateTo)
        &&
        TestComparable(item.DeathDate, DeathDate, DeathDateFrom, DeathDateTo)
        &&
        Test(item.Gender?.Id, GenderId)
        &&
        TestComparable(item.Rating.Average, RatingAverage, RatingAverageFrom, RatingAverageTo)
        &&
        TestComparable(item.Rating.Count, RatingCount, RatingCountFrom, RatingCountTo)
    );
    
    #endregion
}