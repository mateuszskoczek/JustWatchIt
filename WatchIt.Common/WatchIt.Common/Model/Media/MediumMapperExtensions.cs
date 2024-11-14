using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Generics.Rating;
using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Database.Model.Media;
using MediumType = WatchIt.Database.Model.Media.MediumType;

namespace WatchIt.Common.Model.Media;

public static class MediumMapperExtensions
{
    #region PUBLIC METHODS
    
    #region Medium

    public static Database.Model.Media.Medium ToMediumEntity(this MediumResponse response)
    {
        
    }

    public static MediumResponse ToMediumResponse(this Database.Model.Media.Medium entity)
    { 
        MediumResponse response = new MediumResponse();
        response.SetMediumResponseProperties(entity);
        response.Type = entity.Type == MediumType.Movie ? Medium.MediumType.Movie : Medium.MediumType.Series;
        return response;
    }

    public static MediumMovieResponse ToMediumMovieResponse(this MediumMovie entity)
    { 
        MediumMovieResponse response = new MediumMovieResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumMovieResponseProperties(entity);
        return response;
    }
    
    public static MediumSeriesResponse ToMediumSeriesResponse(this MediumSeries entity)
    { 
        MediumSeriesResponse response = new MediumSeriesResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumSeriesResponseProperties(entity);
        return response;
    }
    
    public static MediumUserRatedResponse ToMediumUserRatedResponse(this Database.Model.Media.Medium entity, long accountId)
    { 
        MediumUserRatedResponse response = new MediumUserRatedResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumUserRatedResponseProperties(entity, accountId);
        return response;
    }

    public static MediumMovieUserRatedResponse ToMediumMovieUserRatedResponse(this MediumMovie entity, long accountId)
    { 
        MediumMovieUserRatedResponse response = new MediumMovieUserRatedResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumMovieResponseProperties(entity);
        response.SetMediumUserRatedResponseProperties(entity, accountId);
        return response;
    }
    
    public static MediumSeriesUserRatedResponse ToMediumSeriesUserRatedResponse(this MediumSeries entity, long accountId)
    { 
        MediumSeriesUserRatedResponse response = new MediumSeriesUserRatedResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumSeriesResponseProperties(entity);
        response.SetMediumUserRatedResponseProperties(entity, accountId);
        return response;
    }
    
    #endregion
    
    #region MediumPicture

    public static Database.Model.Media.MediumPicture ToAccountProfilePictureEntity(this ImageRequest request, long mediumId) => new Database.Model.Media.MediumPicture
    {
        MediumId = mediumId,
        Image = request.Image,
        MimeType = request.MimeType,
    };

    #endregion
    
    #endregion
    
    
    
    #region PRIVATE METHODS

    private static void SetMediumResponseProperties(this BaseMediumResponse response, Database.Model.Media.Medium entity)
    {
        response.Id = entity.Id;
        response.Title = entity.Title;
        response.OriginalTitle = entity.OriginalTitle;
        response.Description = entity.Description;
        response.ReleaseDate = entity.ReleaseDate;
        response.Duration = entity.Duration;
        response.Genres = entity.Genres.Select(x => x.ToGenreResponse());
        response.Rating = entity.Ratings.GetRatingResponseFromRatingEntitiesCollection();
    }
    
    private static void SetMediumMovieResponseProperties(this MediumMovieResponse response, MediumMovie entity)
    {
        response.Budget = entity.Budget;
    }
    
    private static void SetMediumSeriesResponseProperties(this MediumSeriesResponse response, MediumSeries entity)
    {
        response.HasEnded = entity.HasEnded;
    }

    private static void SetMediumUserRatedResponseProperties(this IMediumUserRatedResponse response, Database.Model.Media.Medium entity, long accountId)
    {
        response.RatingUser = entity.Ratings.SingleOrDefault(x => x.AccountId == accountId)?.GetRatingResponseFromRatingEntity();
    }
    
    #endregion
}