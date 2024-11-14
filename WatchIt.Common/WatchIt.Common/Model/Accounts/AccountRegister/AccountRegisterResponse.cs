using System.Text.Json.Serialization;

namespace WatchIt.Common.Model.Accounts.AccountRegister;

public class AccountRegisterResponse
{
    #region PROPERTIES

    public required long Id { get; init; }
    public required string Username { get; init; }
    public required string Email { get; init; }
    public required DateTimeOffset JoinDate { get; init; }
    
    #endregion
}