namespace WatchIt.Database.Model.Media;

public class MediumPhoto : IImageEntity
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public long MediumId { get; set; }
    public byte[] Image { get; set; }
    public string MimeType { get; set; }
    public DateTimeOffset UploadDate { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION

    // Medium
    public virtual Medium Medium { get; set; } = default!;
    
    // Background settings
    public virtual MediumPhotoBackgroundSettings? BackgroundSettings { get; set; }

    #endregion
}