using WatchIt.Database.Model;

namespace WatchIt.Common.Model.Generics.Image;

public static class ImageMapperExtensions
{
    #region PUBLIC METHODS
    
    public static ImageResponse ToImageResponse(this IImageEntity entity) => new ImageResponse
    {
        Image = entity.Image,
        MimeType = entity.MimeType,
        UploadDate = entity.UploadDate,
    };
    
    public static void UpdateImageEntityWithRequest(this IImageEntity entity, ImageRequest request)
    {
        entity.Image = request.Image;
        entity.MimeType = request.MimeType;
        entity.UploadDate = DateTimeOffset.Now;
    }
    
    #endregion
}