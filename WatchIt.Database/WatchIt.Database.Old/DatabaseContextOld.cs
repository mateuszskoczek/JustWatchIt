using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SimpleToolkit.Extensions;
using WatchIt.Database.Model.Account;
using WatchIt.Database.Model.Common;
using WatchIt.Database.Model.Configuration.Account;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.Person;
using WatchIt.Database.Model.Rating;
using WatchIt.Database.Model.ViewCount;

namespace WatchIt.Database;

public class DatabaseContextOld : DbContext
{
    #region CONSTRUCTORS

    public DatabaseContextOld()
    {
    }

    public DatabaseContextOld(DbContextOptions<DatabaseContextOld> options) : base(options)
    {
    }

    #endregion



    #region PROPERTIES

    // Common
    public virtual DbSet<Country> Countries { get; set; }
    public virtual DbSet<Genre> Genres { get; set; }
    public virtual DbSet<Gender> Genders { get; set; }

    // Account
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<AccountFollow> AccountFollows { get; set; }
    public virtual DbSet<AccountProfilePicture> AccountProfilePictures { get; set; }
    public virtual DbSet<AccountRefreshToken> AccountRefreshTokens { get; set; }

    // Media
    public virtual DbSet<Media> Media { get; set; }
    public virtual DbSet<MediaMovie> MediaMovies { get; set; }
    public virtual DbSet<MediaSeries> MediaSeries { get; set; }
    public virtual DbSet<MediaSeriesSeason> MediaSeriesSeasons { get; set; }
    public virtual DbSet<MediaSeriesEpisode> MediaSeriesEpisodes { get; set; }
    public virtual DbSet<MediaPosterImage> MediaPosterImages { get; set; }
    public virtual DbSet<MediaPhotoImage> MediaPhotoImages { get; set; }
    public virtual DbSet<MediaPhotoImageBackground> MediaPhotoImageBackgrounds { get; set; }
    public virtual DbSet<MediaGenre> MediaGenres { get; set; }
    public virtual DbSet<MediaProductionCountry> MediaProductionCountries { get; set; }

    // Person
    public virtual DbSet<Person> Persons { get; set; }
    public virtual DbSet<PersonPhotoImage> PersonPhotoImages { get; set; }
    public virtual DbSet<PersonActorRole> PersonActorRoles { get; set; }
    public virtual DbSet<PersonActorRoleType> PersonActorRoleTypes { get; set; }
    public virtual DbSet<PersonCreatorRole> PersonCreatorRoles { get; set; }
    public virtual DbSet<PersonCreatorRoleType> PersonCreatorRoleTypes { get; set; }

    // Rating
    public virtual DbSet<RatingMedia> RatingsMedia { get; set; }
    public virtual DbSet<RatingPersonActorRole> RatingsPersonActorRole { get; set; }
    public virtual DbSet<RatingPersonCreatorRole> RatingsPersonCreatorRole { get; set; }
    public virtual DbSet<RatingMediaSeriesSeason> RatingsMediaSeriesSeason { get; set; }
    public virtual DbSet<RatingMediaSeriesEpisode> RatingsMediaSeriesEpisode { get; set; }

    // ViewCount
    public virtual DbSet<ViewCountPerson> ViewCountsPerson { get; set; }
    public virtual DbSet<ViewCountMedia> ViewCountsMedia { get; set; }

    #endregion



    #region PROTECTED METHODS

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql("name=Default");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetAssembly(typeof(AccountConfiguration)));
        CreateRootUser(modelBuilder);
    }

    protected void CreateRootUser(ModelBuilder modelBuilder)
    {
        IConfigurationSection configuration = this.GetService<IConfiguration>().GetSection("RootUser");

        string leftSalt = StringExtensions.CreateRandom(20);
        string rightSalt = StringExtensions.CreateRandom(20);
        byte[] hash = SHA512.HashData(Encoding.UTF8.GetBytes($"{leftSalt}{configuration["Password"]}{rightSalt}"));

        modelBuilder.Entity<Account>().HasData(new Account
        {
            Id = 1,
            Username = configuration["Username"]!,
            Email = configuration["Email"]!,
            Password = hash,
            LeftSalt = leftSalt,
            RightSalt = rightSalt,
            IsAdmin = true,
        });
    }

    #endregion
}