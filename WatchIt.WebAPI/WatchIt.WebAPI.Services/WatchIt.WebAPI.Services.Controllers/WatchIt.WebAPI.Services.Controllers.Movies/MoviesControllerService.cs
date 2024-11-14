using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;
using Media = WatchIt.Database.Model.Media.Media;

namespace WatchIt.WebAPI.Services.Controllers.Movies;

public class MoviesControllerService : IMoviesControllerService
{
    #region SERVICES

    private readonly DatabaseContext _database;
    
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public MoviesControllerService(DatabaseContext database, IUserService userService)
    {
        _database = database;
        
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main

    public async Task<RequestResult> GetMovies(MediumMovieResponseQueryParameters query)
    {
        IEnumerable<MediumMovie> rawData = await _database.Media
                                                          .OfType<MediumMovie>()
                                                          .ToListAsync();
        IEnumerable<MediumMovieResponse> data = rawData.Select(x => x.ToMediumMovieResponse())
                                                       .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetMovie(long id)
    {
        MediumMovie? item = await _database.Media.OfType<MediumMovie>().SingleOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(item.ToMediumMovieResponse());
    }
    
    public async Task<RequestResult> PostMovie(MediumMovieRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        
        
        return RequestResult.Created($"movies/{mediaItem.Id}", new MovieResponse(mediaMovieItem));
    }
    
    public async Task<RequestResult> PutMovie(long id, MovieRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediaMovie? item = await _database.MediaMovies.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        data.UpdateMediaMovie(item);
        data.UpdateMedia(item.Media);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteMovie(long id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediaMovie? item = await _database.MediaMovies.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        _database.MediaMovies.Attach(item);
        _database.MediaMovies.Remove(item);
        _database.MediaPosterImages.Attach(item.Media.MediaPosterImage!);
        _database.MediaPosterImages.Remove(item.Media.MediaPosterImage!);
        _database.MediaPhotoImages.AttachRange(item.Media.MediaPhotoImages);
        _database.MediaPhotoImages.RemoveRange(item.Media.MediaPhotoImages);
        _database.MediaGenres.AttachRange(item.Media.MediaGenres);
        _database.MediaGenres.RemoveRange(item.Media.MediaGenres);
        _database.MediaProductionCountries.AttachRange(item.Media.MediaProductionCountries);
        _database.MediaProductionCountries.RemoveRange(item.Media.MediaProductionCountries);
        _database.PersonActorRoles.AttachRange(item.Media.PersonActorRoles);
        _database.PersonActorRoles.RemoveRange(item.Media.PersonActorRoles);
        _database.PersonCreatorRoles.AttachRange(item.Media.PersonCreatorRoles);
        _database.PersonCreatorRoles.RemoveRange(item.Media.PersonCreatorRoles);
        _database.RatingsMedia.AttachRange(item.Media.RatingMedia);
        _database.RatingsMedia.RemoveRange(item.Media.RatingMedia);
        _database.ViewCountsMedia.AttachRange(item.Media.ViewCountsMedia);
        _database.ViewCountsMedia.RemoveRange(item.Media.ViewCountsMedia);
        _database.Media.Attach(item.Media);
        _database.Media.Remove(item.Media);
        await _database.SaveChangesAsync();
        
        return RequestResult.NoContent();
    }
    
    #endregion

    #region View count

    public async Task<RequestResult> GetMoviesViewRank(int first, int days)
    {
        if (first < 1 || days < 1)
        {
            return RequestResult.BadRequest();
        }
        
        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-days);
        IEnumerable<MediaMovie> rawData = await _database.MediaMovies.OrderByDescending(x => x.Media.ViewCountsMedia.Where(y => y.Date >= startDate)
                                                                                                                    .Sum(y => y.ViewCount))
                                                                     .ThenBy(x => x.Id)
                                                                     .Take(first)
                                                                     .ToListAsync();
        
        IEnumerable<MovieResponse> data = rawData.Select(x => new MovieResponse(x));
        
        return RequestResult.Ok(data);
    }

    #endregion

    #endregion
}