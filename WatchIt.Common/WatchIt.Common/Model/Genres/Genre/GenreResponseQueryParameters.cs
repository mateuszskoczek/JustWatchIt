using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Genres.Genre;

public class GenreResponseQueryParameters : QueryParameters<GenreResponse>
{
    #region CONSTANTS

    public override IDictionary<string, Func<GenreResponse, IComparable>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<GenreResponse, IComparable>> _orderKeys = new Dictionary<string, Func<GenreResponse, IComparable>>
    {
        { "id", x => x.Id },
        { "name", item => item.Name }
    };

    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "name")]
    public string? Name { get; set; }
    
    #endregion
    
    
    
    #region PRIVATE METHODS

    protected override bool IsMeetingConditions(GenreResponse item) =>
    (
        TestStringWithRegex(item.Name, Name)
    );

    #endregion
}