using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SimpleToolkit.Extensions;
using WatchIt.Common.Model.Photos;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Photos;

public class PhotosControllerService : IPhotosControllerService
{
    #region FIELDS

    private readonly DatabaseContextOld _database;
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONTRUCTORS

    public PhotosControllerService(DatabaseContextOld database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main
    
    public Task<RequestResult> GetPhotoRandomBackground()
    {
        MediaPhotoImage? image = _database.MediaPhotoImages.Where(x => x.MediaPhotoImageBackground != null && x.MediaPhotoImageBackground.IsUniversalBackground).Random();
        if (image is null)
        {
            return Task.FromResult<RequestResult>(RequestResult.NotFound());
        }

        PhotoResponse data = new PhotoResponse(image);
        return Task.FromResult<RequestResult>(RequestResult.Ok(data));
    }
    
    public async Task<RequestResult> DeletePhoto(Guid photoId)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediaPhotoImage? item = await _database.MediaPhotoImages.FirstOrDefaultAsync(x => x.Id == photoId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        if (item.MediaPhotoImageBackground is not null)
        {
            _database.MediaPhotoImageBackgrounds.Attach(item.MediaPhotoImageBackground);
            _database.MediaPhotoImageBackgrounds.Remove(item.MediaPhotoImageBackground);
            await _database.SaveChangesAsync();
        }

        _database.MediaPhotoImages.Attach(item);
        _database.MediaPhotoImages.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    #endregion
    
    #region Background data

    public async Task<RequestResult> PutPhotoBackgroundData(Guid id, PhotoBackgroundDataRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediaPhotoImage? image = await _database.MediaPhotoImages.FirstOrDefaultAsync(x => x.Id == id);
        if (image is null)
        {
            return RequestResult.NotFound();
        }

        MediaPhotoImageBackground? imageBackground = image.MediaPhotoImageBackground;
        if (imageBackground is null)
        {
            imageBackground = data.CreateMediaPhotoImageBackground(id);
            await _database.MediaPhotoImageBackgrounds.AddAsync(imageBackground);
        }
        else
        {
            data.UpdateMediaPhotoImageBackground(imageBackground);
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
        
        MediaPhotoImage? image = await _database.MediaPhotoImages.FirstOrDefaultAsync(x => x.Id == id);
        if (image is null)
        {
            return RequestResult.NotFound();
        }
        
        MediaPhotoImageBackground? imageBackground = image.MediaPhotoImageBackground;
        if (imageBackground is not null)
        {
            _database.MediaPhotoImageBackgrounds.Attach(imageBackground);
            _database.MediaPhotoImageBackgrounds.Remove(imageBackground);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }
    
    #endregion
    
    #endregion
}