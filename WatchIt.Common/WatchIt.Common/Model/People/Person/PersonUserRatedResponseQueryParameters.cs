using Microsoft.AspNetCore.Mvc;

namespace WatchIt.Common.Model.People.Person;

public class PersonUserRatedResponseQueryParameters : BasePersonResponseQueryParameters<PersonUserRatedResponse>
{
    #region CONSTANTS

    public override IDictionary<string, Func<PersonUserRatedResponse, IComparable?>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<PersonUserRatedResponse, IComparable?>> _partialOrderKeys = new Dictionary<string, Func<PersonUserRatedResponse, IComparable?>>
    {
        { "rating_user.average", x => x.RatingUser.Average },
        { "rating_user.count", x => x.RatingUser.Count },
        { "rating_user.last_date", x => x.RatingUser.LastRatingDate }
    };
    protected new static readonly IDictionary<string, Func<PersonUserRatedResponse, IComparable?>> _orderKeys = BasePersonResponseQueryParameters<PersonUserRatedResponse>._orderKeys.Concat(_partialOrderKeys).ToDictionary(x => x.Key, x => x.Value);
    
    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "rating_user_average")]
    public decimal? RatingUserAverage { get; set; }

    [FromQuery(Name = "rating_user_average_from")]
    public decimal? RatingUserAverageFrom { get; set; }

    [FromQuery(Name = "rating_user_average_to")]
    public decimal? RatingUserAverageTo { get; set; }

    [FromQuery(Name = "rating_user_count")]
    public long? RatingUserCount { get; set; }

    [FromQuery(Name = "rating_user_count_from")]
    public long? RatingUserCountFrom { get; set; }

    [FromQuery(Name = "rating_user_count_to")]
    public long? RatingUserCountTo { get; set; }
    
    [FromQuery(Name = "rating_user_last_rating_date")]
    public DateOnly? RatingUserLastRatingDate { get; set; }

    [FromQuery(Name = "rating_user_last_rating_date_from")]
    public DateOnly? RatingUserLastRatingDateFrom { get; set; }

    [FromQuery(Name = "rating_user_last_rating_date_to")]
    public DateOnly? RatingUserLastRatingDateTo { get; set; }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    protected override bool IsMeetingConditions(PersonUserRatedResponse item) =>
    (
        base.IsMeetingConditions(item)
        &&
        TestComparable(item.RatingUser.Average, RatingUserAverage, RatingUserAverageFrom, RatingUserAverageTo)
        &&
        TestComparable(item.RatingUser.Count, RatingUserCount, RatingUserCountFrom, RatingUserCountTo)
        &&
        TestComparable(item.RatingUser.LastRatingDate, RatingUserLastRatingDate, RatingUserLastRatingDateFrom, RatingUserLastRatingDateTo)
    );
    
    #endregion
}