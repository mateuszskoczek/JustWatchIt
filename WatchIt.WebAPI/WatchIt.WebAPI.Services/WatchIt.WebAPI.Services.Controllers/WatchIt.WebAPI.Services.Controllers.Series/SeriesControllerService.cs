using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Series;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Series;

public class SeriesControllerService : ISeriesControllerService
{
    #region SERVICES

    private readonly DatabaseContextOld _database;
    
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public SeriesControllerService(DatabaseContextOld database, IUserService userService)
    {
        _database = database;
        
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main
    
    public async Task<RequestResult> GetAllSeries(SeriesQueryParameters query)
    {
        IEnumerable<MediaSeries> rawData = await _database.MediaSeries.ToListAsync();
        IEnumerable<SeriesResponse> data = rawData.Select(x => new SeriesResponse(x));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetSeries(long id)
    {
        MediaSeries? item = await _database.MediaSeries.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        SeriesResponse data = new SeriesResponse(item);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostSeries(SeriesRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        Media mediaItem = data.CreateMedia();
        await _database.Media.AddAsync(mediaItem);
        await _database.SaveChangesAsync();
        MediaSeries mediaSeriesItem = data.CreateMediaSeries(mediaItem.Id);
        await _database.MediaSeries.AddAsync(mediaSeriesItem);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"series/{mediaItem.Id}", new SeriesResponse(mediaSeriesItem));
    }
    
    public async Task<RequestResult> PutSeries(long id, SeriesRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediaSeries? item = await _database.MediaSeries.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        data.UpdateMediaSeries(item);
        data.UpdateMedia(item.Media);
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
        
        MediaSeries? item = await _database.MediaSeries.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        _database.MediaSeries.Attach(item);
        _database.MediaSeries.Remove(item);
        _database.MediaPosterImages.Attach(item.Media.MediaPosterImage!);
        _database.MediaPosterImages.Remove(item.Media.MediaPosterImage!);
        _database.MediaPhotoImages.AttachRange(item.Media.MediaPhotoImages);
        _database.MediaPhotoImages.RemoveRange(item.Media.MediaPhotoImages);
        _database.MediaGenres.AttachRange(item.Media.MediaGenres);
        _database.MediaGenres.RemoveRange(item.Media.MediaGenres);
        _database.MediaProductionCountries.AttachRange(item.Media.MediaProductionCountries);
        _database.MediaProductionCountries.RemoveRange(item.Media.MediaProductionCountries);
        _database.PersonActorRoles.AttachRange(item.Media.PersonActorRoles);
        _database.PersonActorRoles.RemoveRange(item.Media.PersonActorRoles);
        _database.PersonCreatorRoles.AttachRange(item.Media.PersonCreatorRoles);
        _database.PersonCreatorRoles.RemoveRange(item.Media.PersonCreatorRoles);
        _database.RatingsMedia.AttachRange(item.Media.RatingMedia);
        _database.RatingsMedia.RemoveRange(item.Media.RatingMedia);
        _database.ViewCountsMedia.AttachRange(item.Media.ViewCountsMedia);
        _database.ViewCountsMedia.RemoveRange(item.Media.ViewCountsMedia);
        _database.Media.Attach(item.Media);
        _database.Media.Remove(item.Media);
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
        IEnumerable<MediaSeries> rawData = await _database.MediaSeries.OrderByDescending(x => x.Media.ViewCountsMedia.Where(y => y.Date >= startDate)
                                                                                                                     .Sum(y => y.ViewCount))
                                                                      .ThenBy(x => x.Id)
                                                                      .Take(first)
                                                                      .ToListAsync();
        
        IEnumerable<SeriesResponse> data = rawData.Select(x => new SeriesResponse(x));
        
        return RequestResult.Ok(data);
    }

    #endregion
    
    #endregion
}