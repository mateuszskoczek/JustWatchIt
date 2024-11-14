using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Model.Accounts.Account;
using WatchIt.Common.Model.Accounts.AccountAuthenticate;
using WatchIt.Common.Model.Accounts.AccountBackgroundPicture;
using WatchIt.Common.Model.Accounts.AccountEmail;
using WatchIt.Common.Model.Accounts.AccountPassword;
using WatchIt.Common.Model.Accounts.AccountProfileInfo;
using WatchIt.Common.Model.Accounts.AccountRegister;
using WatchIt.Common.Model.Accounts.AccountUsername;
using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Common.Model.People.Person;
using WatchIt.WebAPI.Services.Controllers.Accounts;

namespace WatchIt.WebAPI.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController(IAccountsControllerService accountsControllerService) : ControllerBase
{
    #region Basic
    
    [HttpPost("register")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AccountRegisterResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> Register([FromBody]AccountRegisterRequest body) => await accountsControllerService.Register(body);
    
    [HttpPost("authenticate")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AccountAuthenticateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Authenticate([FromBody]AccountAuthenticateRequest body) => await accountsControllerService.Authenticate(body);
    
    [HttpPost("authenticate_refresh")]
    [Authorize(AuthenticationSchemes = "refresh")]
    [ProducesResponseType(typeof(AccountAuthenticateResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> AuthenticateRefresh() => await accountsControllerService.AuthenticateRefresh();

    [HttpDelete("logout")]
    [Authorize(AuthenticationSchemes = "refresh")]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> Logout() => await accountsControllerService.Logout();

    #endregion
    
    #region Profile picture
    
    [HttpGet("{id}/profile_picture")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountProfilePicture([FromRoute(Name = "id")]long id) => await accountsControllerService.GetAccountProfilePicture(id);
    
    [HttpPut("profile_picture")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> PutAccountProfilePicture([FromBody]ImageRequest body) => await accountsControllerService.PutAccountProfilePicture(body);
    
    [HttpDelete("profile_picture")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteAccountProfilePicture() => await accountsControllerService.DeleteAccountProfilePicture();
    
    #endregion
    
    #region Profile background
    
    [HttpGet("{id}/profile_background")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(ImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountProfileBackground([FromRoute(Name = "id")]long id) => await accountsControllerService.GetAccountProfileBackground(id);
    
    [HttpPut("profile_background")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(ImageResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> PutAccountProfileBackground([FromBody]AccountBackgroundPictureRequest body) => await accountsControllerService.PutAccountProfileBackground(body);
    
    [HttpDelete("profile_background")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(typeof(void), StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteAccountProfileBackground() => await accountsControllerService.DeleteAccountProfileBackground();
    
    #endregion
    
    #region Info
    
    [HttpGet]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult> GetAccounts(AccountResponseQueryParameters query) => await accountsControllerService.GetAccounts(query);
    
    [HttpGet("{id}")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(AccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccount([FromRoute]long id) => await accountsControllerService.GetAccount(id);
    
    [HttpPut("profile_info")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> PutAccountProfileInfo([FromBody]AccountProfileInfoRequest data) => await accountsControllerService.PutAccountProfileInfo(data);
    
    [HttpPatch("username")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> PatchAccountUsername([FromBody]AccountUsernameRequest data) => await accountsControllerService.PatchAccountUsername(data);
    
    [HttpPatch("email")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> PatchAccountEmail([FromBody]AccountEmailRequest data) => await accountsControllerService.PatchAccountEmail(data);
    
    [HttpPatch("password")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> PatchAccountPassword([FromBody]AccountPasswordRequest data) => await accountsControllerService.PatchAccountPassword(data);
    
    #endregion
    
    #region Media
    
    [HttpGet("{id}/movies")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MediumMovieUserRatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountRatedMovies([FromRoute]long id, MediumMovieUserRatedResponseQueryParameters query) => await accountsControllerService.GetAccountRatedMovies(id, query);
    
    [HttpGet("{id}/series")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<MediumSeriesUserRatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountRatedSeries([FromRoute]long id, MediumSeriesUserRatedResponseQueryParameters query) => await accountsControllerService.GetAccountRatedSeries(id, query);
    
    [HttpGet("{id}/persons")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<PersonUserRatedResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountRatedPersons([FromRoute]long id, PersonUserRatedResponseQueryParameters query) => await accountsControllerService.GetAccountRatedPersons(id, query);
    
    #endregion

    #region Follows

    [HttpGet("{id}/follows")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountFollows([FromRoute]long id, AccountResponseQueryParameters query) => await accountsControllerService.GetAccountFollows(id, query);
    
    [HttpGet("{id}/followers")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(IEnumerable<AccountResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetAccountFollowers([FromRoute]long id, AccountResponseQueryParameters query) => await accountsControllerService.GetAccountFollowers(id, query);
    
    [HttpPost("follows/{user_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> PostAccountFollow([FromRoute(Name = "user_id")]long userId) => await accountsControllerService.PostAccountFollow(userId);
    
    [HttpDelete("follows/{user_id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(void), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult> DeleteAccountFollow([FromRoute(Name = "user_id")]long userId) => await accountsControllerService.DeleteAccountFollow(userId);

    #endregion
}