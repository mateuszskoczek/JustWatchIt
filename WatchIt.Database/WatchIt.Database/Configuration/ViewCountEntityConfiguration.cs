using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model;

namespace WatchIt.Database.Configuration;

public abstract class ViewCountEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IViewCountEntity
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
        
        // Date
        builder.Property(x => x.Date)
               .IsRequired()
               .HasDefaultValueSql("now()");
        
        // View count
        builder.Property(x => x.ViewCount)
               .IsRequired();
    }
    
    #endregion
}