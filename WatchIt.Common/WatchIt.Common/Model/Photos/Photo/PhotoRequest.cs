using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Photos.PhotoBackground;

namespace WatchIt.Common.Model.Photos.Photo;

public class PhotoRequest : ImageRequest
{
    #region PROPERTIES
    
    public long MediumId { get; set; }
    public PhotoBackgroundRequest? BackgroundData { get; set; }
    
    #endregion
}