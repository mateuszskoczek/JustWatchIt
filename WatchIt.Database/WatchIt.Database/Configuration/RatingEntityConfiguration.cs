using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model;
using WatchIt.Database.Model.Accounts;

namespace WatchIt.Database.Configuration;

public abstract class RatingEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IRatingEntity
{
    #region PUBLIC METHODS
    
    public virtual void Configure(EntityTypeBuilder<T> builder)
    {
        // Id
        builder.HasKey(x => x.Id);
        builder.HasIndex(x => x.Id)
               .IsUnique();
        builder.Property(x => x.Id)
               .IsRequired();
        
        // Account
        // You have to configure FK by yourself
        builder.Property(x => x.AccountId)
               .IsRequired();
        
        // Rating
        builder.Property(x => x.Rating)
               .IsRequired();
        
        // Date
        builder.Property(x => x.Date)
               .IsRequired()
               .HasDefaultValueSql("now()");
    }
    
    #endregion
}