namespace WatchIt.Common.Model.Accounts.AccountRegister;

public struct Password
{
    #region PROPERTIES
    
    public byte[] PasswordHash { get; set; }
    public string LeftSalt { get; set; }
    public string RightSalt { get; set; }
    
    #endregion
}