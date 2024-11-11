using System.Drawing;

namespace WatchIt.Database.Model.Media;

public class MediumPhotoBackgroundSettings
{
    #region PROPERTIES

    public Guid PhotoId { get; set; }
    public bool IsUniversal { get; set; }
    public Color FirstGradientColor { get; set; }
    public Color SecondGradientColor { get; set; }

    #endregion
    
    
    
    #region NAVIGATION
    
    public virtual MediumPhoto Photo { get; set; } = default!;
    
    #endregion
}