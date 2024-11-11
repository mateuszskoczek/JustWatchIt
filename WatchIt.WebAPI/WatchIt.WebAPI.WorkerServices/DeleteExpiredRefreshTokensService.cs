using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using WatchIt.Database;
using WatchIt.Database.Model.Account;

namespace WatchIt.WebAPI.WorkerServices;

public class DeleteExpiredRefreshTokensService : BackgroundService
{
    #region SERVICES

    private readonly ILogger<DeleteExpiredRefreshTokensService> _logger;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public DeleteExpiredRefreshTokensService(ILogger<DeleteExpiredRefreshTokensService> logger, IServiceScopeFactory serviceScopeFactory)
    {
        _logger = logger;
        _serviceScopeFactory = serviceScopeFactory;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            
            Task delayTask = Task.Delay(300000, stoppingToken);
            Task actionTask = Action();

            await Task.WhenAll(delayTask, actionTask);
        }
    }

    protected async Task Action()
    {
        using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        {
            DatabaseContextOld database = scope.ServiceProvider.GetService<DatabaseContextOld>();
            
            IEnumerable<AccountRefreshToken> tokens = database.AccountRefreshTokens.Where(x => x.ExpirationDate < DateTime.UtcNow);
            database.AccountRefreshTokens.AttachRange(tokens);
            database.AccountRefreshTokens.RemoveRange(tokens);
            await database.SaveChangesAsync();
        }
    }
    
    #endregion
}