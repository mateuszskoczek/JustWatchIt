using WatchIt.Database.Model.Genres;
using WatchIt.Database.Model.Photos;
using WatchIt.Database.Model.Roles;

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
    
    // Picture
    public virtual MediumPicture? Picture { get; set; }
    
    // Photos
    public virtual IEnumerable<Photo> Photos { get; set; } = [];
    
    // View counts
    public virtual IEnumerable<MediumViewCount> ViewCounts { get; set; } = [];
    
    // Ratings
    public virtual IEnumerable<MediumRating> Ratings { get; set; } = [];
    public virtual IEnumerable<Accounts.Account> RatedBy { get; set; } = [];
    
    // Roles
    public virtual IEnumerable<Role> Roles { get; set; } = [];

    #endregion
}