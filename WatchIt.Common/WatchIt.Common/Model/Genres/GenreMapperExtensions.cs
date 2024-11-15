using WatchIt.Common.Model.Genres.Genre;

namespace WatchIt.Common.Model.Genres;

public static class GenreMapperExtensions
{
    #region PUBLIC METHODS

    public static Genre.GenreResponse ToResponse(this Database.Model.Genres.Genre entity) => new Genre.GenreResponse
    {
        Id = entity.Id,
        Name = entity.Name,
    };

    public static Database.Model.Genres.Genre ToEntity(this GenreRequest request) => new Database.Model.Genres.Genre
    {
        Name = request.Name,
    };

    #endregion
}