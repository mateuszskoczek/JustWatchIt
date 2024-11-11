using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleToolkit.Extensions;
using WatchIt.Common.Model.Accounts;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Movies;
using WatchIt.Common.Model.Persons;
using WatchIt.Common.Model.Photos;
using WatchIt.Common.Model.Series;
using WatchIt.Database;
using WatchIt.Database.Model.Account;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.Rating;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.Tokens;
using WatchIt.WebAPI.Services.Utility.Tokens.Exceptions;
using WatchIt.WebAPI.Services.Utility.User;
using Account = WatchIt.Database.Model.Account.Account;
using AccountProfilePicture = WatchIt.Common.Model.Accounts.AccountProfilePicture;
using Person = WatchIt.Database.Model.Person.Person;

namespace WatchIt.WebAPI.Services.Controllers.Accounts;

public class AccountsControllerService(
    ILogger<AccountsControllerService> logger,
    DatabaseContextOld database,
    ITokensService tokensService,
    IUserService userService
) : IAccountsControllerService
{
    #region PUBLIC METHODS

    #region Basic
    
    public async Task<RequestResult> Register(RegisterRequest data)
    {
        Account account = new Account
        {
            Username = data.Username,
            Email = data.Email,
        };
        
        SetPassword(account, data.Password);
        await database.Accounts.AddAsync(account);
        await database.SaveChangesAsync();
        
        logger.LogInformation($"New account with ID {account.Id} was created (username: {account.Username}; email: {account.Email})");
        return RequestResult.Created($"accounts/{account.Id}", new RegisterResponse(account));
    }

    public async Task<RequestResult> Authenticate(AuthenticateRequest data)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => string.Equals(x.Email, data.UsernameOrEmail) || string.Equals(x.Username, data.UsernameOrEmail));
        if (account is null || !ComputeHash(data.Password, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        Task<string> refreshTokenTask = tokensService.CreateRefreshTokenAsync(account, true);
        Task<string> accessTokenTask = tokensService.CreateAccessTokenAsync(account);
        AuthenticateResponse response = new AuthenticateResponse
        {
            AccessToken = await accessTokenTask,
            RefreshToken = await refreshTokenTask,
        };
        
        account.LastActive = DateTime.UtcNow;
        await database.SaveChangesAsync();
        
        logger.LogInformation($"Account with ID {account.Id} was authenticated");
        return RequestResult.Ok(response);
    }

    public async Task<RequestResult> AuthenticateRefresh()
    {
        Guid jti = userService.GetJti();
        AccountRefreshToken? token = await database.AccountRefreshTokens.FirstOrDefaultAsync(x => x.Id == jti);
        if (token is null || token.ExpirationDate < DateTime.UtcNow)
        {
            return RequestResult.Unauthorized();
        }

        string refreshToken;
        try
        {
            refreshToken = await tokensService.ExtendRefreshTokenAsync(token.Account, token.Id);
        }
        catch (TokenNotFoundException)
        {
            return RequestResult.Unauthorized();
        }
        catch (TokenNotExtendableException)
        {
            refreshToken = userService.GetRawToken().Replace("Bearer ", string.Empty);
        }
        
        string accessToken = await tokensService.CreateAccessTokenAsync(token.Account);
        
        token.Account.LastActive = DateTime.UtcNow;
        await database.SaveChangesAsync();
        
        logger.LogInformation($"Account with ID {token.AccountId} was authenticated by token refreshing");
        return RequestResult.Ok(new AuthenticateResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken,
        });
    }

    public async Task<RequestResult> Logout()
    {
        Guid jti = userService.GetJti();
        AccountRefreshToken? token = await database.AccountRefreshTokens.FirstOrDefaultAsync(x => x.Id == jti);
        if (token is not null)
        {
            database.AccountRefreshTokens.Attach(token);
            database.AccountRefreshTokens.Remove(token);
            await database.SaveChangesAsync();
        }
        return RequestResult.NoContent();
    }

    #endregion

    #region Profile picture

    public async Task<RequestResult> GetAccountProfilePicture(long id)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.BadRequest()
                                .AddValidationError("id", "Account with this id does not exists");
        }

        if (account.ProfilePicture is null)
        {
            return RequestResult.NotFound();
        }
        
        AccountProfilePictureResponse picture = new AccountProfilePictureResponse(account.ProfilePicture);
        return RequestResult.Ok(picture);
    }
    
    public async Task<RequestResult> PutAccountProfilePicture(AccountProfilePictureRequest data)
    {
        Account account = await database.Accounts.FirstAsync(x => x.Id == userService.GetUserId());
        Database.Model.Account.AccountProfilePicture? picture = account.ProfilePicture;

        if (picture is null)
        {
            picture = data.CreateMediaPosterImage();
            await database.AccountProfilePictures.AddAsync(picture);
            await database.SaveChangesAsync();

            account.ProfilePictureId = picture.Id;
        }
        else
        {
            data.UpdateMediaPosterImage(picture);
        }
        
        await database.SaveChangesAsync();

        AccountProfilePictureResponse returnData = new AccountProfilePictureResponse(picture);
        return RequestResult.Ok(returnData);
    }

    public async Task<RequestResult> DeleteAccountProfilePicture()
    {
        Account account = await database.Accounts.FirstAsync(x => x.Id == userService.GetUserId());
        Database.Model.Account.AccountProfilePicture? picture = account.ProfilePicture;

        if (picture is not null)
        {
            account.ProfilePictureId = null;
            await database.SaveChangesAsync();
            
            database.AccountProfilePictures.Attach(picture);
            database.AccountProfilePictures.Remove(picture);
            await database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }

    #endregion

    #region Profile background
    
    public async Task<RequestResult> GetAccountProfileBackground(long id)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.BadRequest()
                                .AddValidationError("id", "Account with this id does not exists");
        }

        if (account.BackgroundPicture is null)
        {
            return RequestResult.NotFound();
        }
        
        PhotoResponse response = new PhotoResponse(account.BackgroundPicture);
        return RequestResult.Ok(response);
    }

    public async Task<RequestResult> PutAccountProfileBackground(AccountProfileBackgroundRequest data)
    {
        Account account = await database.Accounts.FirstAsync(x => x.Id == userService.GetUserId());
        
        account.BackgroundPictureId = data.Id;
        
        await database.SaveChangesAsync();

        PhotoResponse returnData = new PhotoResponse(account.BackgroundPicture!);
        return RequestResult.Ok(returnData);
    }
    
    public async Task<RequestResult> DeleteAccountProfileBackground()
    {
        Account account = await database.Accounts.FirstAsync(x => x.Id == userService.GetUserId());

        if (account.BackgroundPicture is not null)
        {
            account.BackgroundPictureId = null;
            await database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }

    #endregion
    
    #region Info
    
    public async Task<RequestResult> GetAccounts(AccountQueryParameters query)
    {
        IEnumerable<Account> accounts = await database.Accounts.ToListAsync();
        IEnumerable<AccountResponse> accountsData = accounts.Select(x => new AccountResponse(x));
        accountsData = query.PrepareData(accountsData);
        return RequestResult.Ok(accountsData);
    }
    
    public async Task<RequestResult> GetAccount(long id)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }
        
        AccountResponse profileInfoResponse = new AccountResponse(account);
        return RequestResult.Ok(profileInfoResponse);
    }

    public async Task<RequestResult> PutAccountProfileInfo(AccountProfileInfoRequest data)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == userService.GetUserId());
        if (account is null)
        {
            return RequestResult.NotFound();
        }
        
        data.UpdateAccount(account);
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> PatchAccountUsername(AccountUsernameRequest data)
    {
        Account account = await database.Accounts.FirstAsync(x => x.Id == userService.GetUserId());
        
        if (!ComputeHash(data.Password, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        data.UpdateAccount(account);
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> PatchAccountEmail(AccountEmailRequest data)
    {
        Account account = await database.Accounts.FirstAsync(x => x.Id == userService.GetUserId());
        
        if (!ComputeHash(data.Password, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        data.UpdateAccount(account);
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> PatchAccountPassword(AccountPasswordRequest data)
    {
        Account account = await database.Accounts.FirstAsync(x => x.Id == userService.GetUserId());
        
        if (!ComputeHash(data.OldPassword, account.LeftSalt, account.RightSalt).SequenceEqual(account.Password))
        {
            return RequestResult.Unauthorized();
        }
        
        SetPassword(account, data.NewPassword);
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    #endregion
    
    #region Media
    
    public async Task<RequestResult> GetAccountRatedMovies(long id, MovieRatedQueryParameters query)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<MovieRatedResponse> response = account.RatingMedia.Join(database.MediaMovies, x => x.MediaId, x => x.Id, (x, y) => new MovieRatedResponse(y, x));
        response = query.PrepareData(response);
        return RequestResult.Ok(response);
    }
    
    public async Task<RequestResult> GetAccountRatedSeries(long id, SeriesRatedQueryParameters query)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<SeriesRatedResponse> response = account.RatingMedia.Join(database.MediaSeries, x => x.MediaId, x => x.Id, (x, y) => new SeriesRatedResponse(y, x));
        response = query.PrepareData(response);
        return RequestResult.Ok(response);
    }
    
    public async Task<RequestResult> GetAccountRatedPersons(long id, PersonRatedQueryParameters query)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<RatingPersonActorRole> actorRolesRatings = account.RatingPersonActorRole;
        IEnumerable<RatingPersonCreatorRole> creatorRolesRatings = account.RatingPersonCreatorRole;
        IEnumerable<Person> persons = actorRolesRatings.Select(x => x.PersonActorRole.Person)
                                                       .Union(creatorRolesRatings.Select(x => x.PersonCreatorRole.Person));

        IEnumerable<PersonRatedResponse> response = persons.Select(x => new PersonRatedResponse(x, actorRolesRatings.Where(y => y.PersonActorRole.Person.Id == x.Id), creatorRolesRatings.Where(y => y.PersonCreatorRole.Person.Id == x.Id)));
        response = query.PrepareData(response);
        return RequestResult.Ok(response);
    }
    
    #endregion
    
    #region Follows

    public async Task<RequestResult> GetAccountFollows(long id, AccountQueryParameters query)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<AccountResponse> data = account.AccountFollows.Select(x => new AccountResponse(x.AccountFollowed));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetAccountFollowers(long id, AccountQueryParameters query)
    {
        Account? account = await database.Accounts.FirstOrDefaultAsync(x => x.Id == id);
        if (account is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<AccountResponse> data = account.AccountFollowers.Select(x => new AccountResponse(x.AccountFollower));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostAccountFollow(long userId)
    {
        long followerId = userService.GetUserId();
        if (userId == followerId)
        {
            return RequestResult.BadRequest().AddValidationError("user_id", "You cannot follow yourself");
        }
        if (!database.Accounts.Any(x => x.Id == userId))
        {
            return RequestResult.BadRequest().AddValidationError("user_id", "User with this id doesn't exist");
        }
        
        Account account = await database.Accounts.FirstAsync(x => x.Id == followerId);
        if (account.AccountFollows.All(x => x.AccountFollowedId != userId))
        {
            AccountFollow follow = new AccountFollow()
            {
                AccountFollowerId = followerId,
                AccountFollowedId = userId,
            };
            await database.AccountFollows.AddAsync(follow);
            await database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> DeleteAccountFollow(long userId)
    {
        AccountFollow? accountFollow = await database.AccountFollows.FirstOrDefaultAsync(x => x.AccountFollowerId == userService.GetUserId() && x.AccountFollowedId == userId);

        if (accountFollow is not null)
        {
            database.AccountFollows.Attach(accountFollow);
            database.AccountFollows.Remove(accountFollow);
            await database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }
    
    #endregion

    #endregion



    #region PRIVATE METHODS

    protected byte[] ComputeHash(string password, string leftSalt, string rightSalt) => SHA512.HashData(Encoding.UTF8.GetBytes($"{leftSalt}{password}{rightSalt}"));

    private void SetPassword(Account account, string password)
    {
        string leftSalt = StringExtensions.CreateRandom(20);
        string rightSalt = StringExtensions.CreateRandom(20);
        byte[] hash = ComputeHash(password, leftSalt, rightSalt);
        
        account.Password = hash;
        account.LeftSalt = leftSalt;
        account.RightSalt = rightSalt;
    }

    #endregion
}