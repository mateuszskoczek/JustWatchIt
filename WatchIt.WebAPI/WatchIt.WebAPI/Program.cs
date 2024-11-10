using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using WatchIt.Database;
using WatchIt.WebAPI.Services.Controllers.Accounts;
using WatchIt.WebAPI.Services.Controllers.Genders;
using WatchIt.WebAPI.Services.Controllers.Genres;
using WatchIt.WebAPI.Services.Controllers.Media;
using WatchIt.WebAPI.Services.Controllers.Movies;
using WatchIt.WebAPI.Services.Controllers.Persons;
using WatchIt.WebAPI.Services.Controllers.Photos;
using WatchIt.WebAPI.Services.Controllers.Roles;
using WatchIt.WebAPI.Services.Controllers.Series;
using WatchIt.WebAPI.Services.Utility.Configuration;
using WatchIt.WebAPI.Services.Utility.Tokens;
using WatchIt.WebAPI.Services.Utility.User;
using WatchIt.WebAPI.Validators;
using WatchIt.WebAPI.WorkerServices;

namespace WatchIt.WebAPI;

public static class Program
{
    #region PUBLIC METHODS

    public static void Main(string[] args)
    {
        WebApplication app = WebApplication.CreateBuilder(args)
                                            .SetupAuthentication()
                                            .SetupDatabase()
                                            .SetupWorkerServices()
                                            .SetupServices()
                                            .SetupApplication()
                                            .Build();

        using (IServiceScope scope = app.Services.CreateScope())
        {
            DatabaseContext dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            while (!dbContext.Database.CanConnect())
            {
                app.Logger.LogInformation("Waiting for database...");
                Thread.Sleep(1000);
            }
            
            dbContext.Database.Migrate();
        }
        
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }

    #endregion
    

    
    #region PRIVATE METHODS

    private static WebApplicationBuilder SetupAuthentication(this WebApplicationBuilder builder)
    {
        JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();
        JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
        
        AuthenticationBuilder authenticationBuilder = builder.Services.AddAuthentication(x =>
        {
            x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        authenticationBuilder.AddJwtBearer(x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Authentication:Key")!)),
                ValidateAudience = true,
                ValidAudience = "access",
                ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1),
            };
            x.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }

                    return Task.CompletedTask;
                },
            };
        });
        authenticationBuilder.AddJwtBearer("refresh", x =>
        {
            x.RequireHttpsMetadata = false;
            x.SaveToken = true;
            x.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetValue<string>("Authentication:Key")!)),
                ValidateAudience = true,
                ValidIssuer = builder.Configuration.GetValue<string>("Authentication:Issuer"),
                ValidAudience = "refresh",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromMinutes(1)
            };
            x.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        context.Response.Headers.Append("Token-Expired", "true");
                    }
                    return Task.CompletedTask;
                }
            };
        });
        builder.Services.AddAuthorization();

        return builder;
    }
    
    private static WebApplicationBuilder SetupDatabase(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<DatabaseContext>(x => x.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("Default")), ServiceLifetime.Transient);
        builder.Services.AddDbContext<DatabaseContextNew>(x => x.UseLazyLoadingProxies().UseNpgsql(builder.Configuration.GetConnectionString("Default")), ServiceLifetime.Transient);
        return builder;
    }

    private static WebApplicationBuilder SetupWorkerServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddHostedService<DeleteExpiredRefreshTokensService>();
        return builder;
    }
    
    private static WebApplicationBuilder SetupServices(this WebApplicationBuilder builder)
    {
        // Utility
        builder.Services.AddTransient<IConfigurationService, ConfigurationService>();
        builder.Services.AddTransient<ITokensService, TokensService>();
        builder.Services.AddTransient<IUserService, UserService>();
        
        // Controller
        builder.Services.AddTransient<IAccountsControllerService, AccountsControllerService>();
        builder.Services.AddTransient<IGendersControllerService, GendersControllerService>();
        builder.Services.AddTransient<IGenresControllerService, GenresControllerService>();
        builder.Services.AddTransient<IMoviesControllerService, MoviesControllerService>();
        builder.Services.AddTransient<IMediaControllerService, MediaControllerService>();
        builder.Services.AddTransient<ISeriesControllerService, SeriesControllerService>();
        builder.Services.AddTransient<IPhotosControllerService, PhotosControllerService>();
        builder.Services.AddTransient<IPersonsControllerService, PersonsControllerService>();
        builder.Services.AddTransient<IRolesControllerService, RolesControllerService>();
        
        return builder;
    }

    private static WebApplicationBuilder SetupApplication(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetAssembly(typeof(CustomValidators)));
        builder.Services.AddFluentValidationAutoValidation();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
        
        return builder;
    }
    
    #endregion
}
