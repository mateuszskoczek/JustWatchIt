using WatchIt.Database.Model.Genres;

namespace WatchIt.Database.Model.Media;

public abstract class Medium
{
    #region PROPERTIES
    
    public long Id { get; set; }
    public MediumType Type { get; set; }
    public string Title { get; set; } = default!;
    public string? OriginalTitle { get; set; }
    public string? Description { get; set; }
    public TimeSpan? Duration { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    
    #endregion



    #region NAVIGATION

    // Genres
    public virtual IEnumerable<MediumGenre> GenresRelationshipObjects { get; set; } = [];
    public virtual IEnumerable<Genre> Genres { get; set; } = [];

    #endregion
}