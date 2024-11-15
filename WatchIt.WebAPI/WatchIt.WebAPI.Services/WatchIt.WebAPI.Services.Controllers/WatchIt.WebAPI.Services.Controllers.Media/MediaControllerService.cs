using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Generics.Rating;
using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.Genres;
using WatchIt.Database.Model.Media;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;
using GenreResponse = WatchIt.Common.Model.Genres.Genre.GenreResponse;

namespace WatchIt.WebAPI.Services.Controllers.Media;

public class MediaControllerService : IMediaControllerService
{
    #region SERVICES
    
    private readonly DatabaseContext _database;
    private readonly IUserService _userService;

    #endregion



    #region CONSTRUCTORS

    public MediaControllerService(DatabaseContext database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }

    #endregion
    

    
    #region PUBLIC METHODS

    #region Main

    public async Task<RequestResult> GetMedia(MediumResponseQueryParameters query)
    {
        IEnumerable<Medium> rawData = await _database.Media
                                                     .ToListAsync();
        IEnumerable<MediumResponse> data = rawData.Select(x => x.ToResponse())
                                                  .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetMedium(long id)
    {
        Medium? entity = await _database.Media
                                        .FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(entity.ToResponse());
    }

    #endregion

    #region Genres

    public async Task<RequestResult> GetMediumGenres(long id)
    {
        Medium? entity = await _database.Media
                                        .FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<GenreResponse> response = entity.Genres
                                                    .Select(x => x.ToResponse());
        return RequestResult.Ok(response);
    }

    public async Task<RequestResult> PostMediumGenre(long id, short genreId)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Medium? mediaItem = await _database.Media.FindAsync(id);
        Genre? genreItem = await _database.Genres.FindAsync(genreId);
        if (mediaItem is null || genreItem is null)
        {
            return RequestResult.NotFound();
        }
        MediumGenre? mediumGenreItem = await _database.MediumGenres.FindAsync(genreId, id);

        if (mediumGenreItem is null)
        {
            MediumGenre entity = MediaMappers.CreateMediumGenre(id, genreId);
            await _database.MediumGenres.AddAsync(entity);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteMediumGenre(long id, short genreId)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediumGenre? mediumGenreItem = await _database.MediumGenres.FindAsync(genreId, id);
        if (mediumGenreItem is not null)
        {
            _database.MediumGenres.Attach(mediumGenreItem);
            _database.MediumGenres.Remove(mediumGenreItem);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }

    #endregion

    #region Rating

    public async Task<RequestResult> GetMediumRating(long id)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(entity.Ratings.GetRatingResponseFromRatingEntitiesCollection());
    }

    public async Task<RequestResult> GetMediumRatingByUser(long id, long userId)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(entity.Ratings.);
    }
    
    public async Task<RequestResult> PutMediaRating(long mediaId, RatingRequest data)
    {
        Database.Model.Media.Media? item = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        long userId = _userService.GetUserId();

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
            await _database.RatingsMedia.AddAsync(rating);
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> DeleteMediaRating(long mediaId)
    { 
        long userId = _userService.GetUserId();
        
        RatingMedia? item = await _database.RatingsMedia.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.AccountId == userId);
        if (item is null)
        {
            return RequestResult.Ok();
        }
        
        _database.RatingsMedia.Attach(item);
        _database.RatingsMedia.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    #endregion

    #region View count

    public async Task<RequestResult> PostMediaView(long mediaId)
    {
        Database.Model.Media.Media? item = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
        ViewCountMedia? viewCount = await _database.ViewCountsMedia.FirstOrDefaultAsync(x => x.MediaId == mediaId && x.Date == dateNow);
        if (viewCount is null)
        {
            viewCount = new ViewCountMedia
            {
                MediaId = mediaId,
                Date = dateNow,
                ViewCount = 1
            };
            await _database.ViewCountsMedia.AddAsync(viewCount);
        }
        else
        {
            viewCount.ViewCount++;
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    #endregion
    
    #region Poster
    
    public async Task<RequestResult> GetMediaPoster(long mediaId)
    {
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
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
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.BadRequest();
        }

        if (media.MediaPosterImage is null)
        {
            MediaPosterImage image = data.CreateMediaPosterImage();
            await _database.MediaPosterImages.AddAsync(image);
            await _database.SaveChangesAsync();

            media.MediaPosterImageId = image.Id;
        }
        else
        {
            data.UpdateMediaPosterImage(media.MediaPosterImage);
        }
        
        await _database.SaveChangesAsync();

        MediaPosterResponse returnData = new MediaPosterResponse(media.MediaPosterImage!);
        return RequestResult.Ok(returnData);
    }

    public async Task<RequestResult> DeleteMediaPoster(long mediaId)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);

        if (media?.MediaPosterImage != null)
        {
            _database.MediaPosterImages.Attach(media.MediaPosterImage);
            _database.MediaPosterImages.Remove(media.MediaPosterImage);
            await _database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }
    
    #endregion

    #region Photos

    public async Task<RequestResult> GetMediaPhotos(long mediaId, PhotoQueryParameters queryParameters)
    {
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<MediaPhotoImage> imagesRaw = await _database.MediaPhotoImages.Where(x => x.MediaId == mediaId).ToListAsync();
        IEnumerable<PhotoResponse> images = imagesRaw.Select(x => new PhotoResponse(x));
        images = queryParameters.PrepareData(images);
        return RequestResult.Ok(images);
    }
    
    public Task<RequestResult> GetMediaPhotoRandomBackground(long mediaId)
    {
        MediaPhotoImage? image = _database.MediaPhotoImages.Where(x => x.MediaId == mediaId && x.MediaPhotoImageBackground != null).Random();
        if (image is null)
        {
            return Task.FromResult<RequestResult>(RequestResult.NotFound());
        }

        PhotoResponse data = new PhotoResponse(image);
        return Task.FromResult<RequestResult>(RequestResult.Ok(data));
    }

    public async Task<RequestResult> PostMediaPhoto(long mediaId, MediaPhotoRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }

        MediaPhotoImage item = data.CreateMediaPhotoImage(mediaId);
        await _database.MediaPhotoImages.AddAsync(item);
        await _database.SaveChangesAsync();

        MediaPhotoImageBackground? background = data.CreateMediaPhotoImageBackground(item.Id);
        if (background is not null)
        {
            await _database.MediaPhotoImageBackgrounds.AddAsync(background);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Created($"photos/{item.Id}", new PhotoResponse(item));
    }
    
    #endregion
    
    #region Roles
    
    public async Task<RequestResult> GetMediaAllActorRoles(long mediaId, ActorRoleMediaQueryParameters queryParameters)
    {
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<PersonActorRole> dataRaw = await _database.PersonActorRoles.Where(x => x.MediaId == mediaId).ToListAsync();
        IEnumerable<ActorRoleResponse> data = dataRaw.Select(x => new ActorRoleResponse(x));
        data = queryParameters.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostMediaActorRole(long mediaId, ActorRoleMediaRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }

        PersonActorRole item = data.CreateActorRole(mediaId);
        await _database.PersonActorRoles.AddAsync(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/actor/{item.Id}", new ActorRoleResponse(item));
    }
    
    public async Task<RequestResult> GetMediaAllCreatorRoles(long mediaId, CreatorRoleMediaQueryParameters queryParameters)
    {
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<PersonCreatorRole> dataRaw = await _database.PersonCreatorRoles.Where(x => x.MediaId == mediaId).ToListAsync();
        IEnumerable<CreatorRoleResponse> data = dataRaw.Select(x => new CreatorRoleResponse(x));
        data = queryParameters.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostMediaCreatorRole(long mediaId, CreatorRoleMediaRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Database.Model.Media.Media? media = await _database.Media.FirstOrDefaultAsync(x => x.Id == mediaId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }

        PersonCreatorRole item = data.CreateCreatorRole(mediaId);
        await _database.PersonCreatorRoles.AddAsync(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/creator/{item.Id}", new CreatorRoleResponse(item));
    }
    
    #endregion
    
    #endregion
}