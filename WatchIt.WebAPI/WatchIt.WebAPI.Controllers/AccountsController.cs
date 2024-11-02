﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Model.Accounts;
using WatchIt.Common.Model.Movies;
using WatchIt.Common.Model.Persons;
using WatchIt.Common.Model.Series;
using WatchIt.WebAPI.Services.Controllers.Accounts;

namespace WatchIt.WebAPI.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController(IAccountsControllerService accountsControllerService) : ControllerBase
{
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody]RegisterRequest body) => await accountsControllerService.Register(body);
    
    [HttpPost("authenticate")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Authenticate([FromBody]AuthenticateRequest body) => await accountsControllerService.Authenticate(body);
    
    [HttpPost("authenticate-refresh")]
    [Authorize(AuthenticationSchemes = "refresh")]
    [ProducesResponseType(typeof(AuthenticateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(void), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult> AuthenticateRefresh() => await accountsControllerService.AuthenticateRefresh();

    [HttpDelete("logout")]
    [Authorize(AuthenticationSchemes = "refresh")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Logout() => await accountsControllerService.Logout();

    [HttpGet("{id}/profile-picture")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AccountProfilePictureResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountProfilePicture([FromRoute(Name = "id")]long id) => await accountsControllerService.GetAccountProfilePicture(id);
    
    [HttpGet("{id}/info")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountInfo([FromRoute]long id) => await accountsControllerService.GetAccountInfo(id);
    
    [HttpPut("info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutAccountInfo([FromBody]AccountRequest data) => await accountsControllerService.PutAccountInfo(data);
    
    [HttpGet("{id}/movies")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MovieRatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountRatedMovies([FromRoute]long id, MovieRatedQueryParameters query) => await accountsControllerService.GetAccountRatedMovies(id, query);
    
    [HttpGet("{id}/series")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<SeriesRatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountRatedSeries([FromRoute]long id, SeriesRatedQueryParameters query) => await accountsControllerService.GetAccountRatedSeries(id, query);
    
    [HttpGet("{id}/persons")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PersonRatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountRatedPersons([FromRoute]long id, PersonRatedQueryParameters query) => await accountsControllerService.GetAccountRatedPersons(id, query);
}