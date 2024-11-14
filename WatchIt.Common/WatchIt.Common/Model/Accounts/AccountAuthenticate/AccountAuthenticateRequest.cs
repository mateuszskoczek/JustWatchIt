namespace WatchIt.Common.Model.Accounts.AccountAuthenticate;

public class AccountAuthenticateRequest
{
    #region PROPERTIES

    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }

    #endregion
}