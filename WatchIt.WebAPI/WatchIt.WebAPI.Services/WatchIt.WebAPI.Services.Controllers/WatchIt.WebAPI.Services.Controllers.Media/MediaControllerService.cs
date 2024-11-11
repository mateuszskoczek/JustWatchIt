using Microsoft.EntityFrameworkCore;
using SimpleToolkit.Extensions;
using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Photos;
using WatchIt.Common.Model.Rating;
using WatchIt.Common.Model.Roles;
using WatchIt.Database;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.Person;
using WatchIt.Database.Model.Rating;
using WatchIt.Database.Model.ViewCount;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Media;

public class MediaControllerService(DatabaseContextOld database, IUserService userService) : IMediaControllerService
{
    #region PUBLIC METHODS

    #region Main

    public async Task<RequestResult> GetAllMedia(MediaQueryParameters query)
    {
        IEnumerable<Database.Model.Media.Media> rawData = await database.Media.ToListAsync();
        IEnumerable<MediaResponse> data = rawData.Select(x => new MediaResponse(x, database.MediaMovies.Any(y => y.Id == x.Id) ? MediaType.Movie : MediaType.Series));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetMedia(long mediaId)
    {
        Database.Model.Media.Media? item = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        MediaMovie? movie = await database.MediaMovies.FirstOrDefaultAsync(x => x.Id == mediaId);

        MediaResponse mediaResponse = new MediaResponse(item, movie is not null ? MediaType.Movie : MediaType.Series);

        return RequestResult.Ok(mediaResponse);
    }

    #endregion

    #region Genres

    public async Task<RequestResult> GetMediaGenres(long mediaId)
    {
        Database.Model.Media.Media? item = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<GenreResponse> genres = item.MediaGenres.Select(x => new GenreResponse(x.Genre));
        return RequestResult.Ok(genres);
    }

    public async Task<RequestResult> PostMediaGenre(long mediaId, short genreId)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? mediaItem = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        Database.Model.Common.Genre? genreItem = await database.Genres.FirstOrDefaultAsync(x => x.Id == genreId);
        if (mediaItem is null || genreItem is null)
        {
            return RequestResult.NotFound();
        }

        await database.MediaGenres.AddAsync(new MediaGenre
        {
            GenreId = genreId,
            MediaId = mediaId,
        });
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteMediaGenre(long mediaId, short genreId)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediaGenre? item = await database.MediaGenres.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.GenreId == genreId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        database.MediaGenres.Attach(item);
        database.MediaGenres.Remove(item);
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    #endregion

    #region Rating

    public async Task<RequestResult> GetMediaRating(long mediaId)
    {
        Database.Model.Media.Media? item = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        RatingResponse ratingResponse = RatingResponseBuilder.Initialize()
                                                             .Add(item.RatingMedia, x => x.Rating)
                                                             .Build();
        
        return RequestResult.Ok(ratingResponse);
    }

    public async Task<RequestResult> GetMediaRatingByUser(long mediaId, long userId)
    {
        RatingMedia? rating = await database.RatingsMedia.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.AccountId == userId);
        if (rating is null)
        {
            return RequestResult.NotFound();
        }
        
        return RequestResult.Ok(rating.Rating);
    }
    
    public async Task<RequestResult> PutMediaRating(long mediaId, RatingRequest data)
    {
        Database.Model.Media.Media? item = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        long userId = userService.GetUserId();

        RatingMedia? rating = item.RatingMedia.FirstOrDefault(x => x.AccountId == userId);
        if (rating is not null)
        {
            rating.Rating = data.Rating;
        }
        else
        {
            rating = new RatingMedia
            {
                AccountId = userId,
                MediaId = mediaId,
                Rating = data.Rating
            };
            await database.RatingsMedia.AddAsync(rating);
        }
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> DeleteMediaRating(long mediaId)
    { 
        long userId = userService.GetUserId();
        
        RatingMedia? item = await database.RatingsMedia.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.AccountId == userId);
        if (item is null)
        {
            return RequestResult.Ok();
        }
        
        database.RatingsMedia.Attach(item);
        database.RatingsMedia.Remove(item);
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    #endregion

    #region View count

    public async Task<RequestResult> PostMediaView(long mediaId)
    {
        Database.Model.Media.Media? item = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
        ViewCountMedia? viewCount = await database.ViewCountsMedia.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.Date == dateNow);
        if (viewCount is null)
        {
            viewCount = new ViewCountMedia
            {
                MediaId = mediaId,
                Date = dateNow,
                ViewCount = 1
            };
            await database.ViewCountsMedia.AddAsync(viewCount);
        }
        else
        {
            viewCount.ViewCount++;
        }
        await database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    #endregion
    
    #region Poster
    
    public async Task<RequestResult> GetMediaPoster(long mediaId)
    {
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.BadRequest();
        }

        MediaPosterImage? poster = media.MediaPosterImage;
        if (poster is null)
        {
            return RequestResult.NotFound();
        }

        MediaPosterResponse data = new MediaPosterResponse(poster);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> PutMediaPoster(long mediaId, MediaPosterRequest data)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.BadRequest();
        }

        if (media.MediaPosterImage is null)
        {
            MediaPosterImage image = data.CreateMediaPosterImage();
            await database.MediaPosterImages.AddAsync(image);
            await database.SaveChangesAsync();

            media.MediaPosterImageId = image.Id;
        }
        else
        {
            data.UpdateMediaPosterImage(media.MediaPosterImage);
        }
        
        await database.SaveChangesAsync();

        MediaPosterResponse returnData = new MediaPosterResponse(media.MediaPosterImage!);
        return RequestResult.Ok(returnData);
    }

    public async Task<RequestResult> DeleteMediaPoster(long mediaId)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);

        if (media?.MediaPosterImage != null)
        {
            database.MediaPosterImages.Attach(media.MediaPosterImage);
            database.MediaPosterImages.Remove(media.MediaPosterImage);
            await database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }
    
    #endregion

    #region Photos

    public async Task<RequestResult> GetMediaPhotos(long mediaId, PhotoQueryParameters queryParameters)
    {
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<MediaPhotoImage> imagesRaw = await database.MediaPhotoImages.Where(x => x.MediaId == mediaId).ToListAsync();
        IEnumerable<PhotoResponse> images = imagesRaw.Select(x => new PhotoResponse(x));
        images = queryParameters.PrepareData(images);
        return RequestResult.Ok(images);
    }
    
    public Task<RequestResult> GetMediaPhotoRandomBackground(long mediaId)
    {
        MediaPhotoImage? image = database.MediaPhotoImages.Where(x => x.MediaId == mediaId && x.MediaPhotoImageBackground != null).Random();
        if (image is null)
        {
            return Task.FromResult<RequestResult>(RequestResult.NotFound());
        }

        PhotoResponse data = new PhotoResponse(image);
        return Task.FromResult<RequestResult>(RequestResult.Ok(data));
    }

    public async Task<RequestResult> PostMediaPhoto(long mediaId, MediaPhotoRequest data)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }

        MediaPhotoImage item = data.CreateMediaPhotoImage(mediaId);
        await database.MediaPhotoImages.AddAsync(item);
        await database.SaveChangesAsync();

        MediaPhotoImageBackground? background = data.CreateMediaPhotoImageBackground(item.Id);
        if (background is not null)
        {
            await database.MediaPhotoImageBackgrounds.AddAsync(background);
            await database.SaveChangesAsync();
        }
        
        return RequestResult.Created($"photos/{item.Id}", new PhotoResponse(item));
    }
    
    #endregion
    
    #region Roles
    
    public async Task<RequestResult> GetMediaAllActorRoles(long mediaId, ActorRoleMediaQueryParameters queryParameters)
    {
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<PersonActorRole> dataRaw = await database.PersonActorRoles.Where(x => x.MediaId == mediaId).ToListAsync();
        IEnumerable<ActorRoleResponse> data = dataRaw.Select(x => new ActorRoleResponse(x));
        data = queryParameters.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostMediaActorRole(long mediaId, ActorRoleMediaRequest data)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }

        PersonActorRole item = data.CreateActorRole(mediaId);
        await database.PersonActorRoles.AddAsync(item);
        await database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/actor/{item.Id}", new ActorRoleResponse(item));
    }
    
    public async Task<RequestResult> GetMediaAllCreatorRoles(long mediaId, CreatorRoleMediaQueryParameters queryParameters)
    {
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<PersonCreatorRole> dataRaw = await database.PersonCreatorRoles.Where(x => x.MediaId == mediaId).ToListAsync();
        IEnumerable<CreatorRoleResponse> data = dataRaw.Select(x => new CreatorRoleResponse(x));
        data = queryParameters.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostMediaCreatorRole(long mediaId, CreatorRoleMediaRequest data)
    {
        UserValidator validator = userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }

        PersonCreatorRole item = data.CreateCreatorRole(mediaId);
        await database.PersonCreatorRoles.AddAsync(item);
        await database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/creator/{item.Id}", new CreatorRoleResponse(item));
    }
    
    #endregion
    
    #endregion
}