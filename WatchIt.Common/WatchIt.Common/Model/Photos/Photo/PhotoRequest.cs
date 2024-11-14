using WatchIt.Common.Model.Generics.Image;

namespace WatchIt.Common.Model.Photos.Photo;

public class PhotoRequest : ImageRequest
{
    #region PROPERTIES
    
    public long MediaId { get; set; }
    
    #endregion
}