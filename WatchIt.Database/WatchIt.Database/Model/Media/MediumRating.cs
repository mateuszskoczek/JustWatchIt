namespace WatchIt.Database.Model.Media;

public class MediumRating : IRatingEntity
{
    #region PROPERTIES
    
    public Guid Id { get; set; }
    public long MediumId { get; set; }
    public long AccountId { get; set; }
    public byte Rating { get; set; }
    public DateTime Date { get; set; }
    
    #endregion
    
    
    
    #region NAVIGATION
    
    // Medium
    public virtual Medium Medium { get; set; } = default!;
    
    // Account
    public virtual Accounts.Account Account { get; set; } = default!;
    
    #endregion
}