namespace WatchIt.Database.Model.Accounts;

public class AccountProfilePicture : IImageEntity
{
    #region PROPERTIES

    public long AccountId { get; set; }
    public byte[] Image { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public DateTimeOffset UploadDate { get; set; }

    #endregion



    #region NAVIGATION

    // Account
    public virtual Account Account { get; set; } = default!;

    #endregion
}