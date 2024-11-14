namespace WatchIt.Common.Model.Generics.Image;

public class ImageResponse
{
    #region PROPERTIES

    public byte[] Image { get; set; }
    public string MimeType { get; set; }
    public DateTimeOffset UploadDate { get; set; }

    #endregion
}