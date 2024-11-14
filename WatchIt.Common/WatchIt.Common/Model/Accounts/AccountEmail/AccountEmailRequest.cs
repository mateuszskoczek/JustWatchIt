using System.Text.Json.Serialization;

namespace WatchIt.Common.Model.Accounts.AccountEmail;

public class AccountEmailRequest
{
    #region PROPERTIES
    
    public string NewEmail { get; set; }
    public string Password { get; set; }
    
    #endregion
}