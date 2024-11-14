using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using WatchIt.Database;
using WatchIt.Database.Model.Accounts;
using WatchIt.WebAPI.Services.Utility.Configuration;
using WatchIt.WebAPI.Services.Utility.Tokens.Exceptions;

namespace WatchIt.WebAPI.Services.Utility.Tokens;

public class TokensService : ITokensService
{
    #region SERVICES

    private readonly Configuration.Model.Tokens _tokensConfig;
    private readonly DatabaseContext _database;
    private readonly IConfigurationService _configurationService;

    #endregion
    
    
    
    #region CONSTRUCORS
    
    public TokensService(DatabaseContext database, IConfigurationService configurationService)
    {
        _database = database;
        _configurationService = configurationService;
        _tokensConfig = configurationService.Data.Authentication.Tokens;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS

    public async Task<string> CreateRefreshTokenAsync(Account account, bool extendable)
    {
        int expirationMinutes = extendable ? _tokensConfig.RefreshToken.ExtendedLifetime : _tokensConfig.RefreshToken.NormalLifetime;
        DateTime expirationDate = DateTime.UtcNow.AddMinutes(expirationMinutes);
        Guid id = Guid.NewGuid();
        
        _database.AccountRefreshTokens.Add(new AccountRefreshToken
        {
            Id = id,
            AccountId = account.Id,
            ExpirationDate = expirationDate,
            IsExtendable = extendable,
        });
        await _database.SaveChangesAsync();

        return GenerateRefreshJwt(account, id, expirationDate, extendable);
    }

    public async Task<string> ExtendRefreshTokenAsync(Account account, Guid id)
    {
        AccountRefreshToken? token = account.RefreshTokens.FirstOrDefault(x => x.Id == id);
        switch (token)
        {
            case null: throw new TokenNotFoundException();
            case { IsExtendable: true }: throw new TokenNotExtendableException();
        }
        
        DateTime expirationDate = DateTime.UtcNow.AddMinutes(_tokensConfig.RefreshToken.ExtendedLifetime);

        token.ExpirationDate = expirationDate;
        await _database.SaveChangesAsync();

        return GenerateRefreshJwt(account, id, expirationDate, true);
    }
    
    public async Task<string> CreateAccessTokenAsync(Account account) => await Task.Run(() => CreateAccessToken(account));

    public string CreateAccessToken(Account account)
    {
        DateTime lifetime = DateTime.Now.AddMinutes(_tokensConfig.AccessToken.NormalLifetime);
        Guid id = Guid.NewGuid();

        SecurityTokenDescriptor tokenDescriptor = CreateBaseSecurityTokenDescriptor(account, id, lifetime);
        tokenDescriptor.Audience = "access";

        return TokenToString(tokenDescriptor);
    }
    
    #endregion



    #region PRIVATE METHODS

    protected string GenerateRefreshJwt(Account account, Guid id, DateTime expirationDate, bool extendable)
    {
        SecurityTokenDescriptor tokenDescriptor = CreateBaseSecurityTokenDescriptor(account, id, expirationDate);
        tokenDescriptor.Audience = "refresh";
        tokenDescriptor.Subject.AddClaim(new Claim("extend", extendable.ToString()));

        return TokenToString(tokenDescriptor);
    }

    protected SecurityTokenDescriptor CreateBaseSecurityTokenDescriptor(Account account, Guid id, DateTime expirationTime)
    {
        return new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, id.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Username),
                new Claim(JwtRegisteredClaimNames.Exp, expirationTime.Ticks.ToString()),
                new Claim("admin", account.IsAdmin.ToString()),
            }),
            Expires = expirationTime,
            Issuer = _configurationService.Data.Authentication.Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationService.Data.Authentication.Key)), SecurityAlgorithms.HmacSha512)
        };
    }

    protected string TokenToString(SecurityTokenDescriptor tokenDescriptor)
    {
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        handler.InboundClaimTypeMap.Clear();

        SecurityToken token = handler.CreateToken(tokenDescriptor);

        return handler.WriteToken(token);
    }

    #endregion
}