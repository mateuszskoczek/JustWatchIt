using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SimpleToolkit.Extensions;
using WatchIt.Database.Model.Accounts;
using WatchIt.Database.Model.Genders;
using WatchIt.Database.Model.Genres;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.People;
using WatchIt.Database.Model.Roles;

namespace WatchIt.Database;

public class DatabaseContext : DbContext
{
    #region CONSTRUCTORS

    public DatabaseContext()
    {
    }

    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    #endregion



    #region PROPERTIES
    
    // Media
    public virtual DbSet<Medium> Media { get; set; }
    public virtual DbSet<MediumGenre> MediumGenres { get; set; }
    public virtual DbSet<MediumRating> MediumRatings { get; set; }
    public virtual DbSet<MediumViewCount> MediumViewCounts { get; set; }
    public virtual DbSet<MediumPicture> MediumPictures { get; set; }
    public virtual DbSet<MediumPhoto> MediumPhotos { get; set; }
    public virtual DbSet<MediumPhotoBackgroundSettings> MediumPhotoBackgroundSettings { get; set; }
    
    // People
    public virtual DbSet<Person> People { get; set; }
    public virtual DbSet<PersonViewCount> PersonViewCounts { get; set; }
    public virtual DbSet<PersonPicture> PersonPictures { get; set; }
    
    // Roles
    public virtual DbSet<Role> Roles { get; set; }
    public virtual DbSet<RoleActorType> RoleActorTypes { get; set; }
    public virtual DbSet<RoleCreatorType> RoleCreatorTypes { get; set; }
    
    // Accounts
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<AccountFollow> AccountFollows { get; set; }
    public virtual DbSet<AccountProfilePicture> AccountProfilePictures { get; set; }
    public virtual DbSet<AccountRefreshToken> AccountRefreshTokens { get; set; }
    
    // Genders
    public virtual DbSet<Gender> Genders { get; set; }
    
    // Genres
    public virtual DbSet<Genre> Genres { get; set; }
    
    #endregion



    #region PROTECTED METHODS

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("name=Default");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(Account))!);
    }

    #endregion
}