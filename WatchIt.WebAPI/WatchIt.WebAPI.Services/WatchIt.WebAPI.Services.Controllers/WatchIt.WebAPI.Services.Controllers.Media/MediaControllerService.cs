using Microsoft.EntityFrameworkCore;
using SimpleToolkit.Extensions;
using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Generics.Rating;
using WatchIt.Common.Model.Genres;
using WatchIt.Common.Model.Media;
using WatchIt.Common.Model.Media.Medium;
using WatchIt.Common.Model.Photos;
using WatchIt.Common.Model.Photos.Photo;
using WatchIt.Common.Model.Roles;
using WatchIt.Common.Model.Roles.Role;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.Genres;
using WatchIt.Database.Model.Media;
using WatchIt.Database.Model.Photos;
using WatchIt.Database.Model.Roles;
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
        return RequestResult.Ok(entity.Ratings.ToOverallResponse());
    }

    public async Task<RequestResult> GetMediumRatingByUser(long id, long userId)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }
        MediumRating? ratingEntity = entity.Ratings.SingleOrDefault(x => x.AccountId == userId);
        if (ratingEntity is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(ratingEntity.ToUserResponse());
    }
    
    public async Task<RequestResult> PutMediaRating(long id, RatingRequest data)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }
        
        long userId = _userService.GetUserId();
        MediumRating? ratingEntity = entity.Ratings.SingleOrDefault(x => x.AccountId == userId);
        if (ratingEntity is not null)
        {
            ratingEntity.UpdateWithRequest(data);
        }
        else
        {
            ratingEntity = data.ToEntity(id, userId);
            await _database.MediumRatings.AddAsync(ratingEntity);
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    public async Task<RequestResult> DeleteMediaRating(long id)
    { 
        long userId = _userService.GetUserId();
        MediumRating? entity = await _database.MediumRatings.SingleOrDefaultAsync(x => x.MediumId == id && x.AccountId == userId);
        if (entity is not null)
        {
            _database.MediumRatings.Attach(entity);
            _database.MediumRatings.Remove(entity);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Ok();
    }

    #endregion

    #region View count

    public async Task<RequestResult> PostMediaView(long id)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        MediumViewCount? viewCount = entity.ViewCounts.SingleOrDefault(x => x.Date == DateOnly.FromDateTime(DateTime.UtcNow));
        if (viewCount is null)
        {
            viewCount = MediaMappers.CreateMediumViewCount(id);
            await _database.MediumViewCounts.AddAsync(viewCount);
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
    
    public async Task<RequestResult> GetMediaPoster(long id)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        MediumPicture? poster = entity.Picture;
        if (poster is null)
        {
            return RequestResult.NotFound();
        }
        
        return RequestResult.Ok(poster.ToResponse());
    }

    public async Task<RequestResult> PutMediaPoster(long id, ImageRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        MediumPicture? poster = entity.Picture;
        if (poster is null)
        {
            poster = data.ToEntity(id);
            await _database.MediumPictures.AddAsync(poster);
        }
        else
        {
            poster.UpdateWithRequest(data);
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok(poster.ToResponse());
    }

    public async Task<RequestResult> DeleteMediaPoster(long id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        MediumPicture? entity = await _database.MediumPictures.FindAsync(id);
        if (entity is not null)
        {
            _database.MediumPictures.Attach(entity);
            _database.MediumPictures.Remove(entity);
            await _database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }
    
    #endregion

    #region Photos

    public async Task<RequestResult> GetMediaPhotos(long id, PhotoResponseQueryParameters query)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<PhotoResponse> photos = entity.Photos
                                                  .Select(x => x.ToResponse())
                                                  .PrepareData(query);
        return RequestResult.Ok(photos);
    }
    
    public async Task<RequestResult> GetMediaPhotoRandomBackground(long id)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        Photo? photo = entity.Photos
                             .Where(x => x.Background is not null)
                             .Random();
        if (photo is null)
        {
            return RequestResult.NotFound();
        }
        
        return RequestResult.Ok(photo.ToResponse());
    }

    public async Task<RequestResult> PostMediaPhoto(long id, PhotoRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        Photo photo = data.ToEntity();
        await _database.Photos.AddAsync(photo);
        await _database.SaveChangesAsync();

        PhotoBackground? background = data.BackgroundData?.ToEntity(photo.Id);
        if (background is not null)
        {
            await _database.PhotoBackgrounds.AddAsync(background);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.Created($"photos/{photo.Id}", photo.ToResponse());
    }
    
    #endregion
    
    #region Roles
    
    public async Task<RequestResult> GetMediaAllActorRoles(long id, RoleActorResponseQueryParameters query)
    {
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        IEnumerable<RoleActorResponse> roles = entity.Roles
                                                     .OfType<RoleActor>()
                                                     .Select(x => x.ToResponse())
                                                     .PrepareData(query);
        return RequestResult.Ok(roles);
    }
    
    public async Task<RequestResult> PostMediaActorRole(long id, RoleActorRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Medium? entity = await _database.Media.FindAsync(id);
        if (entity is null)
        {
            return RequestResult.NotFound();
        }

        data.
        var role = data.CreateActorRole(mediaId);
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