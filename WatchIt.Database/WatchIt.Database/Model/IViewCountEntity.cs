namespace WatchIt.Database.Model;

public interface IViewCountEntity
{
    #region PROPERTIES

    Guid Id { get; set; }
    DateOnly Date { get; set; }
    long ViewCount { get; set; }

    #endregion
}