﻿using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Genres;

public class GenreResponse : Genre, IQueryOrderable<GenreResponse>
{
    #region PROPERTIES

    [JsonIgnore]
    public static IDictionary<string, Func<GenreResponse, IComparable>> OrderableProperties { get; } = new Dictionary<string, Func<GenreResponse, IComparable>>
    {
        { "id", x => x.Id },
        { "name", x => x.Name },
        { "description", x => x.Description }
    };

    
    [JsonPropertyName("id")]
    public long Id { get; set; }

    #endregion
    
    
    
    #region CONSTRUCTORS

    [JsonConstructor]
    public GenreResponse() {}
    
    [SetsRequiredMembers]
    public GenreResponse(Database.Model.Common.Genre genre)
    {
        Id = genre.Id;
        Name = genre.Name;
        Description = genre.Description;
    }
    
    #endregion
}