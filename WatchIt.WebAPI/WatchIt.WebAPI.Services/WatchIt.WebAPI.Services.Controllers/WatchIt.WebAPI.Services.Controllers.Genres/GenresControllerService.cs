using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Media;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;
using Genre = WatchIt.Database.Model.Common.Genre;

namespace WatchIt.WebAPI.Services.Controllers.Genres;

public class GenresControllerService(DatabaseContextOld database, IUserService userService) : IGenresControllerService
{
    #region PUBLIC METHODS

    #region Main
    
    public async Task<RequestResult> GetGenres(GenreQueryParameters query)
    {
        IEnumerable<GenreResponse> data = await database.Genres.Select(x => new GenreResponse(x)).ToListAsync();
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> GetGenre(short id)
    {
        Genre? item = await database.Genres.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        GenreResponse data = new GenreResponse(item);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> PostGenre(GenreRequest data)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        Genre item = data.CreateGenre();
        await database.Genres.AddAsync(item);
        await database.SaveChangesAsync();
        
        return RequestResult.Created($"genres/{item.Id}", new GenreResponse(item));
    }

    public async Task<RequestResult> DeleteGenre(short id)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Genre? item = await database.Genres.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        database.MediaGenres.AttachRange(item.MediaGenres);
        database.MediaGenres.RemoveRange(item.MediaGenres);
        database.Genres.Attach(item);
        database.Genres.Remove(item);
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    #endregion

    #region Media

    public async Task<RequestResult> GetGenreMedia(short id, MediaQueryParameters query)
    {
        Genre? genre = await database.Genres.FirstOrDefaultAsync(x => x.Id == id);
        if (genre is null)
        {
            return RequestResult.NotFound();
        }
        
        IEnumerable<MediaResponse> data = genre.Media.Select(x => new MediaResponse(x, database.MediaMovies.Any(y => y.Id == x.Id) ? MediaType.Movie : MediaType.Series));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }

    #endregion
    
    #endregion 
}