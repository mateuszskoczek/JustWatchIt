using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Genders;

public class GenderResponse : IQueryOrderable<GenderResponse>
{
    #region PROPERTIES

    [JsonIgnore]
    public static IDictionary<string, Func<GenderResponse, IComparable>> OrderableProperties { get; } = new Dictionary<string, Func<GenderResponse, IComparable>>
    {
        { "name", item => item.Name }
    };

    
    [JsonPropertyName("id")]
    public required short? Id { get; set; }

    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    #endregion
    
    
    
    #region CONSTRUCTORS
    
    [JsonConstructor]
    public GenderResponse() { }

    [SetsRequiredMembers]
    public GenderResponse(Database.Model.Common.Gender gender)
    {
        Id = gender.Id;
        Name = gender.Name;
    }
    
    #endregion
}