using WatchIt.Common.Model.Photos.PhotoBackground;
using WatchIt.WebAPI.Services.Controllers.Common;

namespace WatchIt.WebAPI.Services.Controllers.Photos;

public interface IPhotosControllerService
{
    Task<RequestResult> GetPhotoRandomBackground();
    Task<RequestResult> DeletePhoto(Guid id);
    Task<RequestResult> PutPhotoBackgroundData(Guid id, PhotoBackgroundRequest data);
    Task<RequestResult> DeletePhotoBackgroundData(Guid id);
}