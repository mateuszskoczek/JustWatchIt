using WatchIt.Common.Model.Media.Medium;
using WatchIt.WebAPI.Services.Controllers.Common;

namespace WatchIt.WebAPI.Services.Controllers.Movies;

public interface IMoviesControllerService
{
    Task<RequestResult> GetMovies(MediumMovieResponseQueryParameters query);
    Task<RequestResult> GetMovie(long id);
    Task<RequestResult> PostMovie(MediumMovieRequest data);
    Task<RequestResult> PutMovie(long id, MediumMovieRequest data);
    Task<RequestResult> DeleteMovie(long id);
    Task<RequestResult> GetMoviesViewRank(int first, int days);
}