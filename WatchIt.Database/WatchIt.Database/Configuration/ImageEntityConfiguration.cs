using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WatchIt.Database.Model;

namespace WatchIt.Database.Configuration;

public abstract class ImageEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : class, IImageEntity
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
        
        // Image
        builder.Property(x => x.Image)
               .HasMaxLength(-1)
               .IsRequired();
        
        // MimeType
        builder.Property(x => x.MimeType)
               .HasMaxLength(50)
               .IsRequired();
        
        // UploadDate
        builder.Property(x => x.UploadDate)
               .IsRequired()
               .HasDefaultValueSql("now()");
    }
    
    #endregion
}