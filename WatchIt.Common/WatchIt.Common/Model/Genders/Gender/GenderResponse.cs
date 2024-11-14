using System.Text.Json.Serialization;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Genders.Gender;

public class GenderResponse
{
    #region PROPERTIES
    
    public short Id { get; set; }
    public string Name { get; set; }

    #endregion
}