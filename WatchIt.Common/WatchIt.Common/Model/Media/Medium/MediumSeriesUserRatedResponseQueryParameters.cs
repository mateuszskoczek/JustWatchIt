using Microsoft.AspNetCore.Mvc;

namespace WatchIt.Common.Model.Media.Medium;

public class MediumSeriesUserRatedResponseQueryParameters : BaseMediumSeriesResponseQueryParameters<MediumSeriesUserRatedResponse>
{
    #region CONSTANTS

    public override IDictionary<string, Func<MediumSeriesUserRatedResponse, IComparable?>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<MediumSeriesUserRatedResponse, IComparable?>> _partialOrderKeys = new Dictionary<string, Func<MediumSeriesUserRatedResponse, IComparable?>>
    {
        { "rating_user.rating", x => x.RatingUser.Rating },
        { "rating_user.last_date", x => x.RatingUser.Date }
    };
    protected new static readonly IDictionary<string, Func<MediumSeriesUserRatedResponse, IComparable?>> _orderKeys = BaseMediumSeriesResponseQueryParameters<MediumSeriesUserRatedResponse>._orderKeys.Concat(_partialOrderKeys).ToDictionary(x => x.Key, x => x.Value);
    
    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "rating_user_rating")]
    public byte? RatingUserRating { get; set; }

    [FromQuery(Name = "rating_user_rating_from")]
    public byte? RatingUserRatingFrom { get; set; }

    [FromQuery(Name = "rating_user_rating_to")]
    public byte? RatingUserRatingTo { get; set; }
    
    [FromQuery(Name = "rating_user_date")]
    public DateOnly? RatingUserDate { get; set; }

    [FromQuery(Name = "rating_user_date_from")]
    public DateOnly? RatingUserDateFrom { get; set; }

    [FromQuery(Name = "rating_user_date_to")]
    public DateOnly? RatingUserDateTo { get; set; }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    protected override bool IsMeetingConditions(MediumSeriesUserRatedResponse item) =>
    (
        base.IsMeetingConditions(item)
        &&
        TestComparable(item.RatingUser.Rating, RatingUserRating, RatingUserRatingFrom, RatingUserRatingTo)
        &&
        TestComparable(item.RatingUser.Date, RatingUserDate, RatingUserDateFrom, RatingUserDateTo)
    );
    
    #endregion
}