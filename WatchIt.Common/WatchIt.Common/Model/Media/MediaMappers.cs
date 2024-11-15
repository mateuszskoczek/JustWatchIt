using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Generics.Rating;
using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Database.Model.Media;
using MediumType = WatchIt.Database.Model.Media.MediumType;

namespace WatchIt.Common.Model.Media;

public static class MediaMappers
{
    #region PUBLIC METHODS
    
    #region Medium

    public static MediumMovie ToEntity(this MediumMovieRequest request)
    {
        MediumMovie medium = new MediumMovie();
        medium.UpdateWithRequest(request);
        return medium;
    }

    public static void UpdateWithRequest(this MediumMovie entity, MediumMovieRequest request)
    {
        entity.SetMediumEntityProperties(request);
        entity.Budget = request.Budget;
    }
    
    public static MediumSeries ToEntity(this MediumSeriesRequest request)
    {
        MediumSeries medium = new MediumSeries();
        medium.UpdateWithRequest(request);
        return medium;
    }
    
    public static void UpdateWithRequest(this MediumSeries entity, MediumSeriesRequest request)
    {
        entity.SetMediumEntityProperties(request);
        entity.HasEnded = request.HasEnded;
    }

    public static MediumResponse ToResponse(this Database.Model.Media.Medium entity)
    { 
        MediumResponse response = new MediumResponse();
        response.SetMediumResponseProperties(entity);
        response.Type = entity.Type == MediumType.Movie ? Medium.MediumType.Movie : Medium.MediumType.Series;
        return response;
    }

    public static MediumMovieResponse ToResponse(this MediumMovie entity)
    { 
        MediumMovieResponse response = new MediumMovieResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumMovieResponseProperties(entity);
        return response;
    }
    
    public static MediumSeriesResponse ToResponse(this MediumSeries entity)
    { 
        MediumSeriesResponse response = new MediumSeriesResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumSeriesResponseProperties(entity);
        return response;
    }
    
    public static MediumUserRatedResponse ToResponse(this Database.Model.Media.Medium entity, long accountId)
    { 
        MediumUserRatedResponse response = new MediumUserRatedResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumUserRatedResponseProperties(entity, accountId);
        return response;
    }

    public static MediumMovieUserRatedResponse ToResponse(this MediumMovie entity, long accountId)
    { 
        MediumMovieUserRatedResponse response = new MediumMovieUserRatedResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumMovieResponseProperties(entity);
        response.SetMediumUserRatedResponseProperties(entity, accountId);
        return response;
    }
    
    public static MediumSeriesUserRatedResponse ToResponse(this MediumSeries entity, long accountId)
    { 
        MediumSeriesUserRatedResponse response = new MediumSeriesUserRatedResponse();
        response.SetMediumResponseProperties(entity);
        response.SetMediumSeriesResponseProperties(entity);
        response.SetMediumUserRatedResponseProperties(entity, accountId);
        return response;
    }
    
    #endregion
    
    #region MediumPicture

    public static MediumPicture ToEntity(this ImageRequest request, long mediumId) => new Database.Model.Media.MediumPicture
    {
        MediumId = mediumId,
        Image = request.Image,
        MimeType = request.MimeType,
    };

    #endregion
    
    #region MediumGenre

    public static MediumGenre CreateMediumGenre(long id, short genreId) => new MediumGenre
    {
        MediumId = id,
        GenreId = genreId,
    };
    
    #endregion
    
    #endregion
    
    
    
    #region PRIVATE METHODS

    private static void SetMediumEntityProperties(this Database.Model.Media.Medium entity, MediumRequest request)
    {
        entity.Title = request.Title;
        entity.OriginalTitle = request.OriginalTitle;
        entity.Description = request.Description;
        entity.Duration = request.Duration;
        entity.ReleaseDate = request.ReleaseDate;
    }

    private static void SetMediumResponseProperties(this BaseMediumResponse response, Database.Model.Media.Medium entity)
    {
        response.Id = entity.Id;
        response.Title = entity.Title;
        response.OriginalTitle = entity.OriginalTitle;
        response.Description = entity.Description;
        response.ReleaseDate = entity.ReleaseDate;
        response.Duration = entity.Duration;
        response.Genres = entity.Genres.Select(x => x.ToResponse());
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