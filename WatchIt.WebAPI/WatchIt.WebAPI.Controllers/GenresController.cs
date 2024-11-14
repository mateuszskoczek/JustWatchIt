using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Model.Genres.Genre;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.WebAPI.Services.Controllers.Genres;
using GenreResponse = WatchIt.Common.Model.Genres.Genre.GenreResponse;

namespace WatchIt.WebAPI.Controllers;

[ApiController]
[Route("genres")]
public class GenresController(IGenresControllerService genresControllerService) : ControllerBase
{
    #region METHODS
    
    #region Main
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<GenreResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetGenres(GenreResponseQueryParameters query) => await genresControllerService.GetGenres(query);
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GenreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetGenre([FromRoute]short id) => await genresControllerService.GetGenre(id);
    
    [HttpPost]
    [Authorize]
    [ProducesResponseType(typeof(GenreResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> PostGenre([FromBody]GenreRequest body) => await genresControllerService.PostGenre(body);
    
    [HttpDelete("{id}")]
    [Authorize]
    [ProducesResponseType(typeof(GenreResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> DeleteGenre([FromRoute]short id) => await genresControllerService.DeleteGenre(id);
    
    #endregion

    #region Media

    [HttpGet("{id}/media")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MediumResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetGenreMedia([FromRoute]short id, MediumResponseQueryParameters query) => await genresControllerService.GetGenreMedia(id, query);

    #endregion
    
    #endregion
}