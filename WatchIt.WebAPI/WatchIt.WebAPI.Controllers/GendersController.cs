using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Model.Genders.Gender;
using WatchIt.WebAPI.Services.Controllers.Genders;

namespace WatchIt.WebAPI.Controllers;

[ApiController]
[Route("genders")]
public class GendersController : ControllerBase
{
    #region SERVICES

    private readonly IGendersControllerService _gendersControllerService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public GendersController(IGendersControllerService gendersControllerService)
    {
        _gendersControllerService = gendersControllerService;
    }
    
    #endregion
    
    
    
    #region METHODS
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<GenderResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAllGenders(GenderResponseQueryParameters query) => await _gendersControllerService.GetGenders(query);
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(GenderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetGender([FromRoute]short id) => await _gendersControllerService.GetGender(id);
    
    [HttpPost]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(GenderResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> PostGender([FromBody]GenderRequest body) => await _gendersControllerService.PostGender(body);
    
    [HttpDelete("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> DeleteGender([FromRoute]short id) => await _gendersControllerService.DeleteGender(id);
    
    #endregion
}