using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Genres.Genre;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.Genres;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;
using GenreResponse = WatchIt.Common.Model.Genres.Genre.GenreResponse;

namespace WatchIt.WebAPI.Services.Controllers.Genres;

public class GenresControllerService : IGenresControllerService
{
    #region SERVICES

    private readonly DatabaseContext _database;
    private readonly IUserService _userService;

    #endregion
    

    
    #region CONSTRUCTORS
    
    public GenresControllerService(DatabaseContext database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion

    
    
    #region PUBLIC METHODS

    #region Main
    
    public async Task<RequestResult> GetGenres(GenreResponseQueryParameters query)
    {
        IEnumerable<Genre> rawData = await _database.Genres
                                                    .ToListAsync();
        IEnumerable<GenreResponse> data = rawData.Select(x => x.ToResponse())
                                                 .PrepareData(query);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> GetGenre(short id)
    {
        Genre? item = await _database.Genres.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(item.ToResponse());
    }

    public async Task<RequestResult> PostGenre(GenreRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Genre entity = data.ToEntity();
        await _database.Genres.AddAsync(entity);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok(entity.ToResponse());
    }

    public async Task<RequestResult> DeleteGenre(short id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Genre? item = await _database.Genres.FindAsync(id);
        if (item is not null)
        {
            _database.Genres.Attach(item);
            _database.Genres.Remove(item);
            await _database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }

    #endregion

    #region Media

    public async Task<RequestResult> GetGenreMedia(short id, MediumResponseQueryParameters query)
    {
        Genre? genre = await _database.Genres.FindAsync(id);
        if (genre is null)
        {
            return RequestResult.NotFound();
        }
        
        IEnumerable<MediumResponse> data = genre.Media
                                                .Select(x => x.ToResponse())
                                                .PrepareData(query);
        return RequestResult.Ok(data);
    }

    #endregion
    
    #endregion 
}