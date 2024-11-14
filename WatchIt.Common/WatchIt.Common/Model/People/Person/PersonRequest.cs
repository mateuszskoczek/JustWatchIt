using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace WatchIt.Common.Model.People.Person;

public class PersonRequest
{
    #region PROPERTIES
    
    public string Name { get; set; }
    public string? FullName { get; set; }
    public string? Description { get; set; }
    public DateOnly? BirthDate { get; set; }
    public DateOnly? DeathDate { get; set; }
    public short? GenderId { get; set; }
    
    #endregion
}