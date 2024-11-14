using System.Text.Json.Serialization;

namespace WatchIt.Common.Model.Accounts.AccountPassword;

public class AccountPasswordRequest
{
    #region PROPERTIES
    
    public string OldPassword { get; set; }
    public string NewPassword { get; set; }
    public string NewPasswordConfirmation { get; set; }
    
    #endregion
}