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
using WatchIt.WebAPI.Services.Controllers.Common;

namespace WatchIt.WebAPI.Services.Controllers.Accounts;

public interface IAccountsControllerService
{
    Task<RequestResult> Register(AccountRegisterRequest data);
    Task<RequestResult> Authenticate(AccountAuthenticateRequest data);
    Task<RequestResult> AuthenticateRefresh();
    Task<RequestResult> Logout();
    Task<RequestResult> GetAccountProfilePicture(long id);
    Task<RequestResult> PutAccountProfilePicture(ImageRequest data);
    Task<RequestResult> DeleteAccountProfilePicture();
    Task<RequestResult> GetAccountProfileBackground(long id);
    Task<RequestResult> PutAccountProfileBackground(AccountBackgroundPictureRequest data);
    Task<RequestResult> DeleteAccountProfileBackground();
    Task<RequestResult> GetAccounts(AccountResponseQueryParameters query);
    Task<RequestResult> GetAccount(long id);
    Task<RequestResult> PutAccountProfileInfo(AccountProfileInfoRequest data);
    Task<RequestResult> PatchAccountUsername(AccountUsernameRequest data);
    Task<RequestResult> PatchAccountEmail(AccountEmailRequest data);
    Task<RequestResult> PatchAccountPassword(AccountPasswordRequest data);
    Task<RequestResult> GetAccountRatedMovies(long id, MediumMovieUserRatedResponseQueryParameters query);
    Task<RequestResult> GetAccountRatedSeries(long id, MediumSeriesUserRatedResponseQueryParameters query);
    Task<RequestResult> GetAccountRatedPersons(long id, PersonUserRatedResponseQueryParameters query);
    Task<RequestResult> GetAccountFollows(long id, AccountResponseQueryParameters query);
    Task<RequestResult> GetAccountFollowers(long id, AccountResponseQueryParameters query);
    Task<RequestResult> PostAccountFollow(long userId);
    Task<RequestResult> DeleteAccountFollow(long userId);
}