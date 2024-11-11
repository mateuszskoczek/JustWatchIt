namespace WatchIt.Database.Model.People;

public class PersonViewCount : IViewCountEntity
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public long PersonId { get; set; }
    public DateOnly Date { get; set; }
    public long ViewCount { get; set; }
    
    #endregion



    #region NAVIGATION

    public virtual Person Person { get; set; } = default!;

    #endregion
}