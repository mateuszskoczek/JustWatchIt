using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Photos.PhotoBackground;

namespace WatchIt.Common.Model.Photos.Photo;

public class PhotoResponse : ImageResponse
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public long MediumId { get; set; }
    public PhotoBackgroundResponse? Background { get; set; }
    
    #endregion
}