using System.Drawing;

namespace WatchIt.Common.Model.Photos.PhotoBackground;

public class PhotoBackgroundRequest
{
    #region PROPERTIES

    public required bool IsUniversal { get; set; }
    public required Color FirstGradientColor { get; set; }
    public required Color SecondGradientColor { get; set; }

    #endregion
}