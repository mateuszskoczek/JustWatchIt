using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Model.Series;
using WatchIt.WebAPI.Services.Controllers.Series;

namespace WatchIt.WebAPI.Controllers;

[ApiController]
[Route("series")]
public class SeriesController : ControllerBase
{
    #region SERVICES

    private readonly ISeriesControllerService _seriesControllerService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public SeriesController(ISeriesControllerService seriesControllerService)
    {
        _seriesControllerService = seriesControllerService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS

    #region Main

    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<SeriesResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllSeries(SeriesQueryParameters query) => await _seriesControllerService.GetAllSeries(query);
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(SeriesResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetSeries([FromRoute] long id) => await _seriesControllerService.GetSeries(id);
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(SeriesResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> PostSeries([FromBody] SeriesRequest body) => await _seriesControllerService.PostSeries(body);
    
    [HttpPut("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(void), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> PutSeries([FromRoute] long id, [FromBody]SeriesRequest body) => await _seriesControllerService.PutSeries(id, body);

    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteSeries([FromRoute] long id) => await _seriesControllerService.DeleteSeries(id);

    #endregion
    
    #region View count
    
    [HttpGet("view")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<SeriesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetSeriesViewRank([FromQuery] int first = 5, [FromQuery] int days = 7) => await _seriesControllerService.GetSeriesViewRank(first, days);
    
    #endregion
    
    #endregion
}