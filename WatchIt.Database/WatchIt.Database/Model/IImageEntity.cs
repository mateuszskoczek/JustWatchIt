namespace WatchIt.Database.Model;

public interface IImageEntity
{
    #region PROPERTIES
    
    Guid Id { get; }
    byte[] Image { get; set; }
    string MimeType { get; set; }
    DateTimeOffset UploadDate { get; set; }
    
    #endregion
}