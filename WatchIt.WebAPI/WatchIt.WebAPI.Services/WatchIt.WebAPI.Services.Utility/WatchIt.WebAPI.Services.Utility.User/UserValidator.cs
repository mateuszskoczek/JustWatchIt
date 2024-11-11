using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using WatchIt.Database;

namespace WatchIt.WebAPI.Services.Utility.User;

public class UserValidator
{
    #region FIELDS

    protected readonly DatabaseContextOld _database;
    protected readonly ClaimsPrincipal _claimsPrincipal;
    protected readonly List<string> _validationErrors;

    #endregion



    #region PROPERTIES

    public bool IsValid { get; protected set; }
    public IEnumerable<string> ValidationErrors => _validationErrors;

    #endregion



    #region CONSTRUCTORS

    internal UserValidator(DatabaseContextOld database, ClaimsPrincipal claimsPrincipal)
    {
        _database = database;
        _claimsPrincipal = claimsPrincipal;
        _validationErrors = new List<string>();

        IsValid = true;
    }

    #endregion
    
    
    
    #region PUBLIC METHODS

    public UserValidator MustBeAdmin()
    {
        Claim adminClaim = _claimsPrincipal.FindFirst(x => x.Type == "admin")!;
        if (adminClaim.Value == bool.FalseString)
        {
            IsValid = false;
            _validationErrors.Add("User is not admin");
        }

        return this;
    }

    public UserValidator MustHaveId(long id)
    {
        Claim adminClaim = _claimsPrincipal.FindFirst(x => x.Type == JwtRegisteredClaimNames.Sub)!;
        if (adminClaim.Value == id.ToString())
        {
            IsValid = false;
            _validationErrors.Add("User have wrong id");
        }

        return this;
    }

    #endregion
}