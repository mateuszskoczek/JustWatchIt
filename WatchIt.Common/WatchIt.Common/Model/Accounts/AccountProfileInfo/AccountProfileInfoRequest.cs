using System.Text.Json.Serialization;

namespace WatchIt.Common.Model.Accounts.AccountProfileInfo;

public class AccountProfileInfoRequest
{
    #region PROPERTIES
    
    public string? Description { get; set; }
    public short? GenderId { get; set; }

    #endregion
}