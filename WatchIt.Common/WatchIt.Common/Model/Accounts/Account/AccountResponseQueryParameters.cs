using Microsoft.AspNetCore.Mvc;
using WatchIt.Common.Query;

namespace WatchIt.Common.Model.Accounts.Account;

public class AccountResponseQueryParameters : QueryParameters<AccountResponse>
{
    #region CONSTANTS

    public override IDictionary<string, Func<AccountResponse, IComparable>>? OrderKeys => _orderKeys;
    private static readonly IDictionary<string, Func<AccountResponse, IComparable>> _orderKeys = new Dictionary<string, Func<AccountResponse, IComparable>>
    {
        { "id", x => x.Id },
        { "username", x => x.Username },
        { "email", x => x.Email },
        { "is_admin", x => x.IsAdmin },
        { "active_date", x => x.ActiveDate },
        { "join_date", x => x.JoinDate },
        { "description", x => x.Description },
        { "gender", x => x.Gender?.Name },
    };

    #endregion
    
    
    
    #region PROPERTIES
    
    [FromQuery(Name = "username")]
    public string? Username { get; set; }
    
    [FromQuery(Name = "email")]
    public string? Email { get; set; }
    
    [FromQuery(Name = "is_admin")]
    public bool? IsAdmin { get; set; }

    [FromQuery(Name = "active_date")]
    public DateOnly? ActiveDate { get; set; }

    [FromQuery(Name = "active_date_from")]
    public DateOnly? ActiveDateFrom { get; set; }

    [FromQuery(Name = "active_date_to")]
    public DateOnly? ActiveDateTo { get; set; }

    [FromQuery(Name = "join_date")]
    public DateOnly? JoinDate { get; set; }

    [FromQuery(Name = "join_date_from")]
    public DateOnly? JoinDateFrom { get; set; }

    [FromQuery(Name = "join_date_to")]
    public DateOnly? JoinDateTo { get; set; }
    
    [FromQuery(Name = "description")]
    public string? Description { get; set; }
    
    [FromQuery(Name = "gender_id")]
    public short? GenderId { get; set; }

    #endregion
    
    
    
    #region PRIVATE METHODS

    protected override bool IsMeetingConditions(AccountResponse item) =>
    (
        TestStringWithRegex(item.Username, Username)
        &&
        TestStringWithRegex(item.Email, Email)
        &&
        TestStringWithRegex(item.Description, Description)
        &&
        Test(item.Gender?.Id, GenderId)
        &&
        TestComparable(item.ActiveDate, ActiveDate, ActiveDateFrom, ActiveDateTo)
        &&
        TestComparable(item.JoinDate, JoinDate, JoinDateFrom, JoinDateTo)
        &&
        Test(item.IsAdmin, IsAdmin)
    );

    #endregion
}