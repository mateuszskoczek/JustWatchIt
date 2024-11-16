using Microsoft.EntityFrameworkCore;
using SimpleToolkit.Extensions;
using WatchIt.Common.Model.Photos;
using WatchIt.Common.Model.Photos.Photo;
using WatchIt.Common.Model.Photos.PhotoBackground;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.Photos;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Photos;

public class PhotosControllerService : IPhotosControllerService
{
    #region FIELDS

    private readonly DatabaseContext _database;
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONTRUCTORS

    public PhotosControllerService(DatabaseContext database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main
    
    public Task<RequestResult> GetPhotoRandomBackground()
    {
        PhotoResponse? image = _database.PhotoBackgrounds
                                        .Where(x => x.IsUniversal)
                                        .Random()?
                                        .Photo
                                        .ToResponse();
                                        
        if (image is null)
        {
            return Task.FromResult<RequestResult>(RequestResult.NotFound());
        }
        return Task.FromResult<RequestResult>(RequestResult.Ok(image));
    }
    
    public async Task<RequestResult> DeletePhoto(Guid id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Photo? item = await _database.Photos.FindAsync(id);
        if (item is not null)
        {
            _database.Photos.Attach(item);
            _database.Photos.Remove(item);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.NoContent();
    }
    
    #endregion
    
    #region Background data

    public async Task<RequestResult> PutPhotoBackgroundData(Guid id, PhotoBackgroundRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Photo? photo = await _database.Photos.FindAsync(id);
        if (photo is null)
        {
            return RequestResult.NotFound();
        }

        PhotoBackground? background = photo.Background;
        if (background is null)
        {
            background = data.ToEntity(id);
            await _database.PhotoBackgrounds.AddAsync(background);
        }
        else
        {
            background.UpdateWithRequest(data);
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> DeletePhotoBackgroundData(Guid id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PhotoBackground? background = await _database.PhotoBackgrounds.SingleOrDefaultAsync(x => x.PhotoId == id);
        if (background is not null)
        {
            _database.PhotoBackgrounds.Attach(background);
            _database.PhotoBackgrounds.Remove(background);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }
    
    #endregion
    
    #endregion
}