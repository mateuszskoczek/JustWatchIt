namespace WatchIt.Database.Model.People;

public class PersonPicture : IImageEntity
{
    #region PROPERTIES
    
    public long PersonId { get; set; }
    public byte[] Image { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public DateTimeOffset UploadDate { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION
    
    public virtual Person Person { get; set; } = default!;
    
    #endregion
}