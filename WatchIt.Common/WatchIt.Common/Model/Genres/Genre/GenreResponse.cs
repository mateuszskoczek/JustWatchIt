using System.Text.Json.Serialization;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Genres.Genre;

public class GenreResponse
{
    #region PROPERTIES
    
    public short Id { get; set; }
    public string Name { get; set; }

    #endregion
}