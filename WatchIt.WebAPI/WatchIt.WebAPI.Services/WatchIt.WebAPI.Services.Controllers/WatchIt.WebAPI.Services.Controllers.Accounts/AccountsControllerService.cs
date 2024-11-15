using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleToolkit.Extensions;
using WatchIt.Common.Model.Accounts;
using WatchIt.Common.Model.Accounts.Account;
using WatchIt.Common.Model.Accounts.AccountAuthenticate;
using WatchIt.Common.Model.Accounts.AccountBackgroundPicture;
using WatchIt.Common.Model.Accounts.AccountEmail;
using WatchIt.Common.Model.Accounts.AccountPassword;
using WatchIt.Common.Model.Accounts.AccountProfileInfo;
using WatchIt.Common.Model.Accounts.AccountRegister;
using WatchIt.Common.Model.Accounts.AccountUsername;
using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Common.Model.People;
using WatchIt.Common.Model.People.Person;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.Accounts;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.Tokens;
using WatchIt.WebAPI.Services.Utility.Tokens.Exceptions;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Accounts;

public class AccountsControllerService : IAccountsControllerService
{
    #region SERVICES

    private readonly ILogger<AccountsControllerService> _logger;
    private readonly DatabaseContext _database;
    private readonly ITokensService _tokensService;
    private readonly IUserService _userService;

    #endregion
    
    
    
    #region CONSTRUCTORS

    public AccountsControllerService(ILogger<AccountsControllerService> logger, DatabaseContext database, ITokensService tokensService, IUserService userService)
    {
        _logger = logger;
        _database = database;
        _tokensService = tokensService;
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS

    #region Basic
    
    public async Task<RequestResult> Register(AccountRegisterRequest data)
    {
        Account account = data.ToAccountEntity(SetPassword);
        await _database.Accounts.AddAsync(account);
        await _database.SaveChangesAsync();
        
        _logger.LogInformation($"New account with ID {account.Id} was created (username: {account.Username}; email: {account.Email})");
        return RequestResult.Created($"accounts/{account.Id}", account.ToAccountRegisterResponse());
    }

    public async Task<RequestResult> Authenticate(AccountAuthenticateRequest data)
    {
        Account? account = await _database.Accounts.FirstOrDefaultAsync(x => string.Equals(x.Email, data.UsernameOrEmail) || string.Equals(x.Username, data.UsernameOrEmail));
        if (account is null || !ComputeHash(data.Password, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        Task<string> refreshTokenTask = _tokensService.CreateRefreshTokenAsync(account, true);
        Task<string> accessTokenTask = _tokensService.CreateAccessTokenAsync(account);
        AccountAuthenticateResponse response = new AccountAuthenticateResponse
        {
            AccessToken = await accessTokenTask,
            RefreshToken = await refreshTokenTask,
        };
        
        account.ActiveDate = DateTimeOffset.Now;
        await _database.SaveChangesAsync();
        
        _logger.LogInformation($"Account with ID {account.Id} was authenticated");
        return RequestResult.Ok(response);
    }

    public async Task<RequestResult> AuthenticateRefresh()
    {
        Guid jti = _userService.GetJti();
        AccountRefreshToken? token = await _database.AccountRefreshTokens.FirstOrDefaultAsync(x => x.Id == jti);
        if (token is null || token.ExpirationDate < DateTime.UtcNow)
        {
            return RequestResult.Unauthorized();
        }

        string refreshToken;
        try
        {
            refreshToken = await _tokensService.ExtendRefreshTokenAsync(token.Account, token.Id);
        }
        catch (TokenNotFoundException)
        {
            return RequestResult.Unauthorized();
        }
        catch (TokenNotExtendableException)
        {
            refreshToken = _userService.GetRawToken().Replace("Bearer ", string.Empty);
        }
        
        string accessToken = await _tokensService.CreateAccessTokenAsync(token.Account);
        
        token.Account.ActiveDate = DateTime.UtcNow;
        await _database.SaveChangesAsync();
        
        _logger.LogInformation($"Account with ID {token.AccountId} was authenticated by token refreshing");
        return RequestResult.Ok(new AccountAuthenticateResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    public async Task<RequestResult> Logout()
    {
        Guid jti = _userService.GetJti();
        AccountRefreshToken? token = await _database.AccountRefreshTokens.FindAsync(jti);
        if (token is not null)
        {
            _database.AccountRefreshTokens.Attach(token);
            _database.AccountRefreshTokens.Remove(token);
            await _database.SaveChangesAsync();
        }
        return RequestResult.NoContent();
    }

    #endregion

    #region Profile picture

    public async Task<RequestResult> GetAccountProfilePicture(long id)
    {
        AccountProfilePicture? picture = await _database.AccountProfilePictures.FindAsync(id);
        if (picture is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(picture.ToImageResponse());
    }
    
    public async Task<RequestResult> PutAccountProfilePicture(ImageRequest data)
    {
        long accountId = _userService.GetUserId();
        AccountProfilePicture? picture = await _database.AccountProfilePictures.FindAsync(accountId);

        if (picture is null)
        {
            picture = AccountMapperExtensions.ToAccountProfilePictureEntity(data, accountId);
            await _database.AccountProfilePictures.AddAsync(picture);
        }
        else
        {
            picture.UpdateImageEntityWithRequest(data);
        }
        await _database.SaveChangesAsync();

        return RequestResult.Ok(picture.ToImageResponse());
    }

    public async Task<RequestResult> DeleteAccountProfilePicture()
    {
        AccountProfilePicture? picture = await _database.AccountProfilePictures.FindAsync(_userService.GetUserId());

        if (picture is not null)
        {
            _database.AccountProfilePictures.Attach(picture);
            _database.AccountProfilePictures.Remove(picture);
            await _database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }

    #endregion

    #region Profile background
    
    public async Task<RequestResult> GetAccountProfileBackground(long id)
    {
        AccountBackgroundPicture? background = await _database.AccountBackgroundPictures.FindAsync(id);

        if (background is null)
        {
            return RequestResult.NotFound();
        }
        
        return RequestResult.Ok(background.Background.Photo.ToImageResponse());
    }

    public async Task<RequestResult> PutAccountProfileBackground(AccountBackgroundPictureRequest data)
    {
        long accountId = _userService.GetUserId();
        
        AccountBackgroundPicture? background = await _database.AccountBackgroundPictures.FindAsync(accountId);

        if (background is null)
        {
            background = data.ToAccountBackgroundPictureEntity(_userService.GetUserId());
        }
        else
        {
            background.UpdateAccountBackgroundPictureEntityWithRequest(data);
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok(background.Background.Photo.ToImageResponse());
    }
    
    public async Task<RequestResult> DeleteAccountProfileBackground()
    {
        AccountBackgroundPicture? background = await _database.AccountBackgroundPictures.FindAsync(_userService.GetUserId());

        if (background is not null)
        {
            _database.AccountBackgroundPictures.Attach(background);
            _database.AccountBackgroundPictures.Remove(background);
            await _database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }

    #endregion
    
    #region Info
    
    public async Task<RequestResult> GetAccounts(AccountResponseQueryParameters query)
    {
        IEnumerable<Account> accounts = await _database.Accounts.ToListAsync();
        IEnumerable<AccountResponse> accountsData = accounts.Select(x => x.ToAccountResponse());
        accountsData = query.PrepareData(accountsData);
        return RequestResult.Ok(accountsData);
    }
    
    public async Task<RequestResult> GetAccount(long id)
    {
        Account? account = await _database.Accounts.FindAsync(id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(account.ToAccountResponse());
    }

    public async Task<RequestResult> PutAccountProfileInfo(AccountProfileInfoRequest data)
    {
        Account? account = await _database.Accounts.FindAsync(_userService.GetUserId());
        if (account is null)
        {
            return RequestResult.NotFound();
        }
        
        account.UpdateWithAccountProfileInfoRequest(data);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> PatchAccountUsername(AccountUsernameRequest data)
    {
        Account? account = await _database.Accounts.FindAsync(_userService.GetUserId());
        if (account is null)
        {
            return RequestResult.NotFound();
        }
        if (!ComputeHash(data.Password, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        account.UpdateWithAccountUsernameRequest(data);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> PatchAccountEmail(AccountEmailRequest data)
    {
        Account? account = await _database.Accounts.FindAsync(_userService.GetUserId());
        if (account is null)
        {
            return RequestResult.NotFound();
        }
        if (!ComputeHash(data.Password, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        account.UpdateWithAccountEmailRequest(data);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> PatchAccountPassword(AccountPasswordRequest data)
    {
        Account? account = await _database.Accounts.FindAsync(_userService.GetUserId());
        if (account is null)
        {
            return RequestResult.NotFound();
        }
        if (!ComputeHash(data.OldPassword, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        account.UpdateWithAccountPasswordRequest(data, SetPassword);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    #endregion
    
    #region Media
    
    public async Task<RequestResult> GetAccountRatedMovies(long id, MediumMovieUserRatedResponseQueryParameters query)
    {
        Account? account = await _database.Accounts.FindAsync(id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<MediumMovieUserRatedResponse> response = account.MediaRated
                                                                    .OfType<MediumMovie>()
                                                                    .Select(x => x.ToResponse(id))
                                                                    .PrepareData(query);
        return RequestResult.Ok(response);
    }
    
    public async Task<RequestResult> GetAccountRatedSeries(long id, MediumSeriesUserRatedResponseQueryParameters query)
    {
        Account? account = await _database.Accounts
                                          .FindAsync(id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<MediumSeriesUserRatedResponse> response = account.MediaRated
                                                                     .OfType<MediumSeries>()
                                                                     .Select(x => x.ToResponse(id))
                                                                     .PrepareData(query);
        return RequestResult.Ok(response);
    }
    
    public async Task<RequestResult> GetAccountRatedPersons(long id, PersonUserRatedResponseQueryParameters query)
    {
        Account? account = await _database.Accounts.FindAsync(id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<PersonUserRatedResponse> response = account.RolesRated
                                                               .Select(x => x.Person)
                                                               .DistinctBy(x => x.Id)
                                                               .Select(x => x.ToPersonUserRatedResponse(id))
                                                               .PrepareData(query);
        return RequestResult.Ok(response);
    }
    
    #endregion
    
    #region Follows

    public async Task<RequestResult> GetAccountFollows(long id, AccountResponseQueryParameters query)
    {
        Account? account = await _database.Accounts
                                          .FindAsync(id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<AccountResponse> data = account.Follows
                                                   .Select(x => x.ToAccountResponse())
                                                   .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetAccountFollowers(long id, AccountResponseQueryParameters query)
    {
        Account? account = await _database.Accounts
                                          .FindAsync(id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<AccountResponse> data = account.Followers
                                                   .Select(x => x.ToAccountResponse())
                                                   .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostAccountFollow(long userId)
    {
        long followerId = _userService.GetUserId();
        Account? account = await _database.Accounts
                                         .FindAsync(followerId);
        if (account is null)
        {
            return RequestResult.Unauthorized();
        }
        
        if (userId == followerId)
        {
            return RequestResult.BadRequest().AddValidationError("user_id", "You cannot follow yourself");
        }
        if (!await _database.Accounts.AnyAsync(x => x.Id == userId))
        {
            return RequestResult.BadRequest().AddValidationError("user_id", "User with this id doesn't exist");
        }
        
        if (account.Follows.Any(x => x.Id == userId))
        {
            AccountFollow follow = new AccountFollow
            {
                FollowerId = followerId,
                FollowedId = userId,
            };
            await _database.AccountFollows.AddAsync(follow);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> DeleteAccountFollow(long userId)
    {
        AccountFollow? accountFollow = await _database.AccountFollows
                                                      .FindAsync(_userService.GetUserId(), userId);

        if (accountFollow is not null)
        {
            _database.AccountFollows.Attach(accountFollow);
            _database.AccountFollows.Remove(accountFollow);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }
    
    #endregion

    #endregion



    #region PRIVATE METHODS

    protected byte[] ComputeHash(string password, string leftSalt, string rightSalt) => SHA512.HashData(Encoding.UTF8.GetBytes($"{leftSalt}{password}{rightSalt}"));

    private Password SetPassword(string password)
    {
        string leftSalt = StringExtensions.CreateRandom(20);
        string rightSalt = StringExtensions.CreateRandom(20);
        byte[] hash = ComputeHash(password, leftSalt, rightSalt);
        return new Password
        {
            LeftSalt = leftSalt,
            RightSalt = rightSalt,
            PasswordHash = hash,
        };
    }

    #endregion
}