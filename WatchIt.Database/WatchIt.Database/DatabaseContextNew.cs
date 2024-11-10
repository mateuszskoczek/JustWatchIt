using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using SimpleToolkit.Extensions;
using WatchIt.Database.Model.Accounts;
using WatchIt.Database.Model.Genders;

namespace WatchIt.Database;

public class DatabaseContextNew : DbContext
{
    #region CONSTRUCTORS

    public DatabaseContextNew()
    {
    }

    public DatabaseContextNew(DbContextOptions<DatabaseContextNew> options) : base(options)
    {
    }

    #endregion



    #region PROPERTIES
    
    // Accounts
    public virtual DbSet<Account> Accounts { get; set; }
    public virtual DbSet<AccountFollow> AccountFollows { get; set; }
    public virtual DbSet<AccountProfilePicture> AccountProfilePictures { get; set; }
    public virtual DbSet<AccountRefreshToken> AccountRefreshTokens { get; set; }
    
    // Genders
    public virtual DbSet<Gender> Genders { get; set; }

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