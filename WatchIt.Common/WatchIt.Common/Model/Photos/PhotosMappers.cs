using WatchIt.Common.Model.Photos.Photo;
using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Photos.PhotoBackground;

namespace WatchIt.Common.Model.Photos;

public static class PhotosMappers
{
    #region PUBLIC METHODS

    #region Photo

    public static Database.Model.Photos.Photo ToEntity(this PhotoRequest request) => new Database.Model.Photos.Photo
    {
        MediumId = request.MediumId,
        Image = request.Image,
        MimeType = request.MimeType,
    };
    
    public static PhotoResponse ToResponse(this Database.Model.Photos.Photo entity) => new PhotoResponse
    {
        Id = entity.Id,
        MediumId = entity.MediumId,
        Image = entity.Image,
        MimeType = entity.MimeType,
        UploadDate = entity.UploadDate,
        Background = entity.Background?.ToResponse()
    };

    #endregion
    
    #region PhotoBackground

    public static Database.Model.Photos.PhotoBackground ToEntity(this PhotoBackgroundRequest request, Guid photoId) => new Database.Model.Photos.PhotoBackground
    {
        PhotoId = photoId,
        IsUniversal = request.IsUniversal,
        FirstGradientColor = request.FirstGradientColor,
        SecondGradientColor = request.SecondGradientColor,
    };

    public static void UpdateWithRequest(this Database.Model.Photos.PhotoBackground entity, PhotoBackgroundRequest request)
    {
        entity.IsUniversal = request.IsUniversal;
        entity.FirstGradientColor = request.FirstGradientColor;
        entity.SecondGradientColor = request.SecondGradientColor;
    }

    public static PhotoBackgroundResponse ToResponse(this Database.Model.Photos.PhotoBackground entity) => new PhotoBackgroundResponse
    {
        IsUniversal = entity.IsUniversal,
        FirstGradientColor = entity.FirstGradientColor,
        SecondGradientColor = entity.SecondGradientColor,
    };

    #endregion

    #endregion
}