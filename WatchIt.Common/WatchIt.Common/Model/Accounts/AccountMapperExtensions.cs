using WatchIt.Common.Model.Accounts.Account;
using WatchIt.Common.Model.Accounts.AccountBackgroundPicture;
using WatchIt.Common.Model.Accounts.AccountEmail;
using WatchIt.Common.Model.Accounts.AccountPassword;
using WatchIt.Common.Model.Accounts.AccountProfileInfo;
using WatchIt.Common.Model.Accounts.AccountRegister;
using WatchIt.Common.Model.Accounts.AccountUsername;
using WatchIt.Common.Model.Genders;
using WatchIt.Common.Model.Generics.Image;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Common.Model.Accounts;

public static class AccountMapperExtensions
{
    #region PUBLIC METHODS
    
    #region Account

    public static Account.AccountResponse ToAccountResponse(this Database.Model.Accounts.Account account) => new Account.AccountResponse
    {
        Id = account.Id,
        Username = account.Username,
        Email = account.Email,
        IsAdmin = account.IsAdmin,
        JoinDate = account.JoinDate,
        ActiveDate = account.ActiveDate,
        Description = account.Description,
        Gender = account.Gender?.ToGenderResponse(),
    };
    
    #endregion

    #region AccountRegister
    
    public static Database.Model.Accounts.Account ToAccountEntity(this AccountRegisterRequest request, Func<string, Password> passwordGenFunc)
    {
        Password generatedPassword = passwordGenFunc(request.Password);
        return new Database.Model.Accounts.Account
        {
            Username = request.Username,
            Email = request.Email,
            Password = generatedPassword.PasswordHash,
            LeftSalt = generatedPassword.LeftSalt,
            RightSalt = generatedPassword.RightSalt,
        };
    }

    public static AccountRegisterResponse ToAccountRegisterResponse(this Database.Model.Accounts.Account account) => new AccountRegisterResponse
    {
        Id = account.Id,
        Username = account.Username,
        Email = account.Email,
        JoinDate = account.JoinDate,
    };
    
    #endregion
    
    #region AccountUsername

    public static void UpdateWithAccountUsernameRequest(this Database.Model.Accounts.Account account, AccountUsernameRequest request)
    {
        account.Username = request.NewUsername;
    }
    
    #endregion
    
    #region AccountEmail

    public static void UpdateWithAccountEmailRequest(this Database.Model.Accounts.Account account, AccountEmailRequest request)
    {
        account.Email = request.NewEmail;
    }
    
    #endregion
    
    #region AccountPassword

    public static void UpdateWithAccountPasswordRequest(this Database.Model.Accounts.Account account, AccountPasswordRequest request, Func<string, Password> passwordGenFunc)
    {
        Password generatedPassword = passwordGenFunc(request.NewPassword);
        account.Password = generatedPassword.PasswordHash;
        account.LeftSalt = generatedPassword.LeftSalt;
        account.RightSalt = generatedPassword.RightSalt;
    }
    
    #endregion
    
    #region AccountProfileInfo

    public static void UpdateWithAccountProfileInfoRequest(this Database.Model.Accounts.Account account, AccountProfileInfoRequest request)
    {
        account.Description = request.Description;
        account.GenderId = request.GenderId;
    }
    
    #endregion
    
    #region AccountProfilePicture

    public static AccountProfilePicture ToAccountProfilePictureEntity(this ImageRequest request, long accountId) => new Database.Model.Accounts.AccountProfilePicture
    {
        AccountId = accountId,
        Image = request.Image,
        MimeType = request.MimeType,
    };

    #endregion
    
    #region AccountBackgroundPicture

    public static Database.Model.Accounts.AccountBackgroundPicture ToAccountBackgroundPictureEntity(this AccountBackgroundPictureRequest request, long accountId) => new Database.Model.Accounts.AccountBackgroundPicture
    {
        AccountId = accountId,
        BackgroundId = request.Id,
    };

    public static void UpdateAccountBackgroundPictureEntityWithRequest(this Database.Model.Accounts.AccountBackgroundPicture entity, AccountBackgroundPictureRequest request)
    {
        entity.BackgroundId = request.Id;
    }

    #endregion

    #endregion
}