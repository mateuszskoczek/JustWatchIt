using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Media.Medium;

public abstract class BaseMediumResponseQueryParameters<T> : QueryParameters<T> where T : BaseMediumResponse
{
    #region CONSTANTS
    
    public override IDictionary<string, Func<T, IComparable?>>? OrderKeys => _orderKeys;
    protected static readonly IDictionary<string, Func<T, IComparable?>> _orderKeys = new Dictionary<string, Func<T, IComparable?>>
    {
        { "id", x => x.Id },
        { "title", x => x.Title },
        { "original_title", x => x.OriginalTitle },
        { "description", x => x.Description },
        { "release_date", x => x.ReleaseDate },
        { "rating.average", x => x.Rating.Average },
        { "rating.count", x => x.Rating.Count }
    };
    
    #endregion
    
    
    
    #region PROPERTIES

    [FromQuery(Name = "title")]
    public string? Title { get; set; }

    [FromQuery(Name = "original_title")]
    public string? OriginalTitle { get; set; }

    [FromQuery(Name = "description")]
    public string? Description { get; set; }

    [FromQuery(Name = "release_date")]
    public DateOnly? ReleaseDate { get; set; }

    [FromQuery(Name = "release_date_from")]
    public DateOnly? ReleaseDateFrom { get; set; }

    [FromQuery(Name = "release_date_to")]
    public DateOnly? ReleaseDateTo { get; set; }

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
    
    [FromQuery(Name = "genre")]
    public IEnumerable<short>? Genres { get; set; }

    #endregion
    
    
    
    #region PRIVATE METHODS

    protected override bool IsMeetingConditions(T item) =>
    (
        TestStringWithRegex(item.Title, Title)
        &&
        TestStringWithRegex(item.OriginalTitle, OriginalTitle)
        &&
        TestStringWithRegex(item.Description, Description)
        &&
        TestComparable(item.ReleaseDate, ReleaseDate, ReleaseDateFrom, ReleaseDateTo)
        &&
        TestComparable(item.Rating.Average, RatingAverage, RatingAverageFrom, RatingAverageTo)
        &&
        TestComparable(item.Rating.Count, RatingCount, RatingCountFrom, RatingCountTo)
        &&
        TestContains(Genres, item.Genres.Select(x => x.Id))
    );

    #endregion
}