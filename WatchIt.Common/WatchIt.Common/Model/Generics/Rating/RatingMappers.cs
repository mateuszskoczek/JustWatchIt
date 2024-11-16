using WatchIt.Database.Model;
using WatchIt.Database.Model.People;
using WatchIt.Database.Model.Media;

namespace WatchIt.Common.Model.Generics.Rating;

public static class RatingMappers
{
    #region PUBLIC METHODS
    
    public static void UpdateWithRequest(this IRatingEntity entity, RatingRequest ratingRequest)
    {
        entity.Rating = ratingRequest.Rating;
    }
    
    public static RatingOverallResponse ToOverallResponse(this IEnumerable<IRatingEntity> entities)
    {
        long sum = entities.Sum(x => x.Rating);
        long count = entities.Count();
        return new RatingOverallResponse
        {
            Average = count > 0 ? (decimal)sum / count : null,
            Count = count
        };
    }

    public static RatingUserResponse ToUserResponse(this IRatingEntity entity) => new RatingUserResponse
    {
        Date = entity.Date,
        Rating = entity.Rating,
    };

    public static RatingUserOverallResponse ToUserOverallResponse(this IEnumerable<IRatingEntity> entities)
    {
        long sum = entities.Sum(x => x.Rating);
        long count = entities.Count();
        DateTimeOffset lastDate = entities.Max(x => x.Date);
        return new RatingUserOverallResponse
        {
            Average = count > 0 ? (decimal)sum / count : null,
            LastRatingDate = lastDate,
            Count = count
        };
    }
    
    #endregion
}