using Microsoft.AspNetCore.Mvc;

namespace WatchIt.Common.Model.Media.Medium;

public abstract class BaseMediumMovieResponseQueryParameters<T> : BaseMediumResponseQueryParameters<T> where T : MediumMovieResponse
{
    #region CONSTANTS

    public override IDictionary<string, Func<T, IComparable?>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<T, IComparable?>> _partialOrderKeys = new Dictionary<string, Func<T, IComparable?>>
    {
        { "budget", x => x.Budget },
    };
    protected new static readonly IDictionary<string, Func<T, IComparable?>> _orderKeys = BaseMediumResponseQueryParameters<T>._orderKeys.Concat(_partialOrderKeys).ToDictionary(x => x.Key, x => x.Value);
    
    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "budget")]
    public decimal? Budget { get; set; }

    [FromQuery(Name = "budget_from")]
    public decimal? BudgetFrom { get; set; }

    [FromQuery(Name = "budget_to")]
    public decimal? BudgetTo { get; set; }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    protected override bool IsMeetingConditions(T item) =>
    (
        base.IsMeetingConditions(item)
        &&
        TestComparable(item.Budget, Budget, BudgetFrom, BudgetTo)
    );
    
    #endregion
}