using System.Text.Json.Serialization;

namespace WatchIt.Common.Model.Accounts.AccountRegister;

public class AccountRegisterRequest
{
    #region PROPERTIES

    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string PasswordConfirmation { get; set; }

    #endregion
}