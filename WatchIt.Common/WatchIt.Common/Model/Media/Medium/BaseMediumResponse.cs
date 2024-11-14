using WatchIt.Common.Model.Generics.Rating;
using WatchIt.Common.Model.Genres.Genre;

namespace WatchIt.Common.Model.Media.Medium;

public abstract class BaseMediumResponse
{
    #region PROPERTIES

    public long Id { get; set; }
    public string Title { get; set; }
    public string? OriginalTitle { get; set; }
    public string? Description { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public TimeSpan? Duration { get; set; }
    public IEnumerable<GenreResponse> Genres { get; set; }
    public RatingOverallResponse Rating { get; set; }

    #endregion
}