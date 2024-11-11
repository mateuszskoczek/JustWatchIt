using WatchIt.Database.Model.Genres;

namespace WatchIt.Database.Model.Media;

public class MediumGenre
{
    #region PROPERTIES
    
    public long MediumId { get; set; }
    public short GenreId { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION
    
    // Medium
    public virtual Medium Medium { get; set; } = default!;
    
    // Genre
    public virtual Genre Genre { get; set; } = default!;
    
    #endregion
}