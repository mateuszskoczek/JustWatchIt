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
        IEnumerable<MediumMovieResponse> data = rawData.Select(x => x.ToResponse())
                                                       .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetMovie(long id)
    {
        MediumMovie? item = await _database.Media
                                           .OfType<MediumMovie>()
                                           .SingleOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(item.ToResponse());
    }
    
    public async Task<RequestResult> PostMovie(MediumMovieRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        MediumMovie entity = data.ToEntity();
        await _database.Media.AddAsync(entity);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"movies/{entity.Id}", entity.ToResponse());
    }
    
    public async Task<RequestResult> PutMovie(long id, MediumMovieRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediumMovie? item = await _database.Media
                                           .OfType<MediumMovie>()
                                           .SingleOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        item.UpdateWithRequest(data);
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
        
        Medium? item = await _database.Media.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        _database.Media.Attach(item);
        _database.Media.Remove(item);
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
        IEnumerable<MediumMovie> rawData = await _database.Media
                                                          .OfType<MediumMovie>()
                                                          .OrderByDescending(x => x.ViewCounts
                                                                                   .Where(y => y.Date >= startDate)
                                                                                   .Sum(y => y.ViewCount))
                                                          .ThenBy(x => x.Id)
                                                          .Take(first)
                                                          .ToListAsync();
        IEnumerable<MediumMovieResponse> data = rawData.Select(x => x.ToResponse());
        return RequestResult.Ok(data);
    }

    #endregion

    #endregion
}