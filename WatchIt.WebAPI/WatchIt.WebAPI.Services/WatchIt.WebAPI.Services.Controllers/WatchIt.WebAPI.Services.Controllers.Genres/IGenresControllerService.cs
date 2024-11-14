using WatchIt.Common.Model.Genres.Genre;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.WebAPI.Services.Controllers.Common;

namespace WatchIt.WebAPI.Services.Controllers.Genres;

public interface IGenresControllerService
{
    #region PUBLIC METHODS
    
    #region Main
    
    Task<RequestResult> GetGenres(GenreResponseQueryParameters query);
    Task<RequestResult> GetGenre(short id);
    Task<RequestResult> PostGenre(GenreRequest data);
    Task<RequestResult> DeleteGenre(short id);
    
    #endregion
    
    #region Media
    
    Task<RequestResult> GetGenreMedia(short id, MediumResponseQueryParameters query);
    
    #endregion
    
    #endregion
}