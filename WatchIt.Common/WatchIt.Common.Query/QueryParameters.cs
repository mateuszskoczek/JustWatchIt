using System.Collections;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc;

namespace WatchIt.Common.Query;

public abstract class QueryParameters
{
    #region PROPERTIES

    [FromQuery(Name = "order_by")]
    public string? OrderBy { get; set; }

    [FromQuery(Name = "order")]
    public string? Order { get; set; }

    [FromQuery(Name = "first")]
    public int? First { get; set; }

    [FromQuery(Name = "after")]
    public int? After { get; set; }

    #endregion



    #region PUBLIC METHODS

    public override string ToString()
    {
        List<string> queries = new List<string>();
        PropertyInfo[] properties = this.GetType().GetProperties();
        foreach (PropertyInfo property in properties)
        {
            object? value = property.GetValue(this);
            FromQueryAttribute? attribute = property.GetCustomAttributes<FromQueryAttribute>(true).FirstOrDefault();
            if (value is not null && attribute is not null)
            {
                if (value is IEnumerable enumerable and not string)
                {
                    IEnumerable<string> arrayQueryElements = enumerable.Cast<object>().Select(x => QueryElementToString(attribute.Name!, x.ToString()));
                    queries.AddRange(arrayQueryElements);
                }
                else
                {
                    string query = QueryElementToString(attribute.Name!, value);
                    queries.Add(query);
                }
            }
        }

        return $"?{string.Join('&', queries)}";
    }

    #endregion
    
    
    
    #region PRIVATE METHODS

    private string QueryElementToString(string name, object value)
    {
        string valueString = (value switch
        {
            decimal d => d.ToString(CultureInfo.InvariantCulture),
            _ => value.ToString()
        })!;
        string query = $"{name}={valueString}";
        return query;
    }
    
    
    protected static bool Test<T>(T? property, T? query) =>
    (
        query is null
        ||
        (
            property is not null
            &&
            property.Equals(query)
        )
    );

    protected static bool TestStringWithRegex(string? property, string? regexQuery) => 
    (
        string.IsNullOrEmpty(regexQuery) 
        || 
        (
            !string.IsNullOrEmpty(property) 
            && 
            Regex.IsMatch(property, regexQuery, RegexOptions.IgnoreCase)
        )
    );

    protected static bool TestComparable(IComparable? property, IComparable? exact, IComparable? from, IComparable? to) =>
    (
        (
            exact is null
            ||
            (
                property is not null
                &&
                property.CompareTo(exact) == 0
            )
        )
        &&
        (
            from is null
            ||
            (
                property is not null
                &&
                property.CompareTo(from) >= 0
            )
        )
        &&
        (
            to is null
            ||
            (
                property is not null
                &&
                property.CompareTo(to) < 0
            )
        )
    );

    protected static bool TestContains<T>(IEnumerable<T>? shouldBeInCollection, IEnumerable<T>? collection) => 
    (
        collection is null
        ||
        shouldBeInCollection is null
        ||
        shouldBeInCollection.All(collection.Contains)
    );

    #endregion
}



public abstract class QueryParameters<T> : QueryParameters where T : IQueryOrderable<T> 
{
    #region PUBLIC METHODS

    protected abstract bool IsMeetingConditions(T item);

    public IEnumerable<T> PrepareData(IEnumerable<T> data)
    {
        data = data.Where(IsMeetingConditions);
        
        if (OrderBy is not null)
        {
            if (T.OrderableProperties.TryGetValue(OrderBy, out Func<T, IComparable>? orderFunc))
            {
                data = Order == "asc" ? data.OrderBy(orderFunc) : data.OrderByDescending(orderFunc);
            }
        }
        if (After is not null)
        {
            data = data.Skip(After.Value);
        }
        if (First is not null)
        {
            data = data.Take(First.Value);
        }
        return data;
    }

    #endregion
}