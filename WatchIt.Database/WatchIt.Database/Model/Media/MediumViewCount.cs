using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Configuration.Media;

public class MediumViewCount
{
    #region PROPERTIES

    public Guid Id { get; set; }
    public long MediumId { get; set; }
    public DateOnly Date { get; set; }
    public long ViewCount { get; set; }

    #endregion
    
    
    
    #region NAVIGATION
    
    public virtual Medium Medium { get; set; } = default!;
    
    #endregion
}