using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Movies;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Movies;

public class MoviesControllerService : IMoviesControllerService
{
    #region SERVICES

    private readonly DatabaseContextOld _database;
    
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public MoviesControllerService(DatabaseContextOld database, IUserService userService)
    {
        _database = database;
        
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main

    public async Task<RequestResult> GetAllMovies(MovieQueryParameters query)
    {
        IEnumerable<MediaMovie> rawData = await _database.MediaMovies.ToListAsync();
        IEnumerable<MovieResponse> data = rawData.Select(x => new MovieResponse(x));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetMovie(long id)
    {
        MediaMovie? item = await _database.MediaMovies.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        MovieResponse data = new MovieResponse(item);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostMovie(MovieRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        Media mediaItem = data.CreateMedia();
        await _database.Media.AddAsync(mediaItem);
        await _database.SaveChangesAsync();
        MediaMovie mediaMovieItem = data.CreateMediaMovie(mediaItem.Id);
        await _database.MediaMovies.AddAsync(mediaMovieItem);
        await _database.SaveChangesAsync();
        
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