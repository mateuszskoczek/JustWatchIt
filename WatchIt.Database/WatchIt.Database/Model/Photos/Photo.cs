using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Model.Photos;

public class Photo : IImageEntity
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
    public virtual PhotoBackground? Background { get; set; }

    #endregion
}