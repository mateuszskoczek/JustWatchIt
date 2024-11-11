using WatchIt.Database.Model.Media;

namespace WatchIt.Database.Model.Genres;

public class Genre
{
    #region PROPERTIES

    public short Id { get; set; }
    public string Name { get; set; } = default!;

    #endregion
    
    
    
    #region NAVIGATION
    
    // Media
    public virtual IEnumerable<MediumGenre> MediaRelationObjects { get; set; } = [];
    public virtual IEnumerable<Medium> Media { get; set; } = [];
    
    #endregion
}