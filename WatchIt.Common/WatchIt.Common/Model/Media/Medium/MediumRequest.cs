namespace WatchIt.Common.Model.Media.Medium;

public abstract class MediumRequest
{
    #region PROPERTIES
    
    public string Title { get; set; }
    public string? OriginalTitle { get; set; }
    public string? Description { get; set; }
    public DateOnly? ReleaseDate { get; set; }
    public short? Length { get; set; }
    
    #endregion
}