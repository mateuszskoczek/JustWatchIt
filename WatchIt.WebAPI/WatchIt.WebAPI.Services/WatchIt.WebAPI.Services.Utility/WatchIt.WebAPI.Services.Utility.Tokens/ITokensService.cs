using WatchIt.Database.Model.Accounts;

namespace WatchIt.WebAPI.Services.Utility.Tokens;

public interface ITokensService
{
    Task<string> CreateRefreshTokenAsync(Account account, bool extendable);
    Task<string> ExtendRefreshTokenAsync(Account account, Guid id);
    Task<string> CreateAccessTokenAsync(Account account);
    string CreateAccessToken(Account account);
}