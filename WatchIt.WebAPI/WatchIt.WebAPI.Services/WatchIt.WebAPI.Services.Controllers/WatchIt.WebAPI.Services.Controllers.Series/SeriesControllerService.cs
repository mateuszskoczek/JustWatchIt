using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Common.Model.Series;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Series;

public class SeriesControllerService : ISeriesControllerService
{
    #region SERVICES

    private readonly DatabaseContext _database;
    
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public SeriesControllerService(DatabaseContext database, IUserService userService)
    {
        _database = database;
        
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main

    public async Task<RequestResult> GetSeries(MediumSeriesResponseQueryParameters query)
    {
        IEnumerable<MediumSeries> rawData = await _database.Media
                                                           .OfType<MediumSeries>()
                                                           .ToListAsync();
        IEnumerable<MediumSeriesResponse> data = rawData.Select(x => x.ToResponse())
                                                        .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetSeries(long id)
    {
        MediumSeries? item = await _database.Media
                                            .OfType<MediumSeries>()
                                            .SingleOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(item.ToResponse());
    }
    
    public async Task<RequestResult> PostSeries(MediumSeriesRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        MediumSeries entity = data.ToEntity();
        await _database.Media.AddAsync(entity);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"series/{entity.Id}", entity.ToResponse());
    }
    
    public async Task<RequestResult> PutSeries(long id, MediumSeriesRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediumSeries? item = await _database.Media
                                            .OfType<MediumSeries>()
                                            .SingleOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        item.UpdateWithRequest(data);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteSeries(long id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Medium? item = await _database.Media.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        _database.Media.Attach(item);
        _database.Media.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.NoContent();
    }
    
    #endregion

    #region View count

    public async Task<RequestResult> GetSeriesViewRank(int first, int days)
    {
        if (first < 1 || days < 1)
        {
            return RequestResult.BadRequest();
        }
        
        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-days);
        IEnumerable<MediumSeries> rawData = await _database.Media
                                                           .OfType<MediumSeries>()
                                                           .OrderByDescending(x => x.ViewCounts
                                                                                    .Where(y => y.Date >= startDate)
                                                                                    .Sum(y => y.ViewCount))
                                                           .ThenBy(x => x.Id)
                                                           .Take(first)
                                                           .ToListAsync();
        IEnumerable<MediumSeriesResponse> data = rawData.Select(x => x.ToResponse());
        return RequestResult.Ok(data);
    }

    #endregion

    #endregion
}