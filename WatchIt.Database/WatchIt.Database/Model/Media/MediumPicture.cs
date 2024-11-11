namespace WatchIt.Database.Model.Media;

public class MediumPicture : IImageEntity
{
    #region PROPERTIES

    public long MediumId { get; set; }
    public byte[] Image { get; set; } = default!;
    public string MimeType { get; set; } = default!;
    public DateTimeOffset UploadDate { get; set; }

    #endregion

    
    
    #region NAVIGATION
    
    public virtual Medium Medium { get; set; } = default!;
    
    #endregion
}