using System.Drawing;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Model.Photos;

public class PhotoBackground
{
    #region PROPERTIES

    public Guid Id { get; set; }
    public Guid PhotoId { get; set; }
    public bool IsUniversal { get; set; }
    public Color FirstGradientColor { get; set; }
    public Color SecondGradientColor { get; set; }

    #endregion
    
    
    
    #region NAVIGATION
    
    // Photo
    public virtual Photo Photo { get; set; } = default!;
    
    // Background usages
    public virtual IEnumerable<AccountBackgroundPicture> BackgroundUsages { get; set; } = [];
    
    #endregion
}