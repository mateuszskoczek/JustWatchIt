namespace WatchIt.Common.Query;

public static class EnumerableExtensions
{
    #region PUBLIC METHODS

    public static IEnumerable<T> PrepareData<T>(this IEnumerable<T> data, QueryParameters<T> query) => query.PrepareData(data);

    #endregion
}