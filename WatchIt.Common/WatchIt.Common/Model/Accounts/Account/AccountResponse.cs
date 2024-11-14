using WatchIt.Common.Model.Genders.Gender;

namespace WatchIt.Common.Model.Accounts.Account;

public class AccountResponse
{
    #region PROPERTIES
    
    public long Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public bool IsAdmin { get; set; }
    public DateTimeOffset ActiveDate { get; set; }
    public DateTimeOffset JoinDate { get; set; }
    public string? Description { get; set; }
    public GenderResponse? Gender { get; set; }
    
    #endregion
}