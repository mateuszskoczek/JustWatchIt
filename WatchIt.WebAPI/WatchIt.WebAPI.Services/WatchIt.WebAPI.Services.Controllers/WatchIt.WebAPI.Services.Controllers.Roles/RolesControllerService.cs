using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Rating;
using WatchIt.Common.Model.Roles;
using WatchIt.Database;
using WatchIt.Database.Model.Person;
using WatchIt.Database.Model.Rating;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Roles;

public class RolesControllerService : IRolesControllerService
{
    #region SERVICES

    private readonly DatabaseContextOld _database;
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public RolesControllerService(DatabaseContextOld database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion



    #region PUBLIC METHODS
    
    #region Actor
    
    public async Task<RequestResult> GetActorRole(Guid id)
    {
        PersonActorRole? item = await _database.PersonActorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        ActorRoleResponse data = new ActorRoleResponse(item);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PutActorRole(Guid id, ActorRoleUniversalRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PersonActorRole? item = await _database.PersonActorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        data.UpdateActorRole(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteActorRole(Guid id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PersonActorRole? item = await _database.PersonActorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NoContent();
        }

        _database.PersonActorRoles.Attach(item);
        _database.PersonActorRoles.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.NoContent();
    }
    
    public async Task<RequestResult> GetActorRoleRating(Guid id)
    {
        PersonActorRole? item = await _database.PersonActorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        RatingResponse ratingResponse = RatingResponseBuilder.Initialize()
                                                             .Add(item.RatingPersonActorRole, x => x.Rating)
                                                             .Build();
        
        return RequestResult.Ok(ratingResponse);
    }
    
    public async Task<RequestResult> GetActorRoleRatingByUser(Guid id, long userId)
    {
        RatingPersonActorRole? rating = await _database.RatingsPersonActorRole.FirstOrDefaultAsync(x => x.PersonActorRoleId == id && x.AccountId == userId);
        if (rating is null)
        {
            return RequestResult.NotFound();
        }
        
        return RequestResult.Ok(rating.Rating);
    }
    
    public async Task<RequestResult> PutActorRoleRating(Guid id, RatingRequest data)
    {
        PersonActorRole? item = await _database.PersonActorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        long userId = _userService.GetUserId();

        RatingPersonActorRole? rating = item.RatingPersonActorRole.FirstOrDefault(x => x.AccountId == userId);
        if (rating is not null)
        {
            rating.Rating = data.Rating;
        }
        else
        {
            rating = new RatingPersonActorRole
            {
                AccountId = userId,
                PersonActorRoleId = id,
                Rating = data.Rating
            };
            await _database.RatingsPersonActorRole.AddAsync(rating);
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteActorRoleRating(Guid id)
    { 
        long userId = _userService.GetUserId();
        
        RatingPersonActorRole? item = await _database.RatingsPersonActorRole.FirstOrDefaultAsync(x => x.PersonActorRoleId == id && x.AccountId == userId);
        if (item is null)
        {
            return RequestResult.Ok();
        }
        
        _database.RatingsPersonActorRole.Attach(item);
        _database.RatingsPersonActorRole.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> GetAllActorRoleTypes(RoleTypeQueryParameters query)
    {
        IEnumerable<PersonActorRoleType> rawData = await _database.PersonActorRoleTypes.ToListAsync();
        IEnumerable<RoleTypeResponse> data = rawData.Select(x => new RoleTypeResponse(x));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> GetActorRoleType(short id)
    {
        PersonActorRoleType? item = await _database.PersonActorRoleTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        RoleTypeResponse data = new RoleTypeResponse(item);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> PostActorRoleType(RoleTypeRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        PersonActorRoleType item = data.CreateActorRoleType();
        await _database.PersonActorRoleTypes.AddAsync(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/actor/{item.Id}", new RoleTypeResponse(item));
    }
    
    public async Task<RequestResult> DeleteActorRoleType(short id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PersonActorRoleType? item = await _database.PersonActorRoleTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NoContent();
        }

        _database.PersonActorRoleTypes.Attach(item);
        _database.PersonActorRoleTypes.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.NoContent();
    }
    
    #endregion
    
    #region Creator
    
    public async Task<RequestResult> GetCreatorRole(Guid id)
    {
        PersonCreatorRole? item = await _database.PersonCreatorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        CreatorRoleResponse data = new CreatorRoleResponse(item);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PutCreatorRole(Guid id, CreatorRoleUniversalRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PersonCreatorRole? item = await _database.PersonCreatorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        data.UpdateCreatorRole(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteCreatorRole(Guid id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PersonCreatorRole? item = await _database.PersonCreatorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NoContent();
        }

        _database.PersonCreatorRoles.Attach(item);
        _database.PersonCreatorRoles.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.NoContent();
    }
    
    public async Task<RequestResult> GetCreatorRoleRating(Guid id)
    {
        PersonCreatorRole? item = await _database.PersonCreatorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        RatingResponse ratingResponse = RatingResponseBuilder.Initialize()
                                                             .Add(item.RatingPersonCreatorRole, x => x.Rating)
                                                             .Build();
        
        return RequestResult.Ok(ratingResponse);
    }
    
    public async Task<RequestResult> GetCreatorRoleRatingByUser(Guid id, long userId)
    {
        RatingPersonCreatorRole? rating = await _database.RatingsPersonCreatorRole.FirstOrDefaultAsync(x => x.PersonCreatorRoleId == id && x.AccountId == userId);
        if (rating is null)
        {
            return RequestResult.NotFound();
        }
        
        return RequestResult.Ok(rating.Rating);
    }
    
    public async Task<RequestResult> PutCreatorRoleRating(Guid id, RatingRequest data)
    {
        PersonCreatorRole? item = await _database.PersonCreatorRoles.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        long userId = _userService.GetUserId();

        RatingPersonCreatorRole? rating = item.RatingPersonCreatorRole.FirstOrDefault(x => x.AccountId == userId);
        if (rating is not null)
        {
            rating.Rating = data.Rating;
        }
        else
        {
            rating = new RatingPersonCreatorRole
            {
                AccountId = userId,
                PersonCreatorRoleId = id,
                Rating = data.Rating
            };
            await _database.RatingsPersonCreatorRole.AddAsync(rating);
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeleteCreatorRoleRating(Guid id)
    { 
        long userId = _userService.GetUserId();
        
        RatingPersonCreatorRole? item = await _database.RatingsPersonCreatorRole.FirstOrDefaultAsync(x => x.PersonCreatorRoleId == id && x.AccountId == userId);
        if (item is null)
        {
            return RequestResult.Ok();
        }
        
        _database.RatingsPersonCreatorRole.Attach(item);
        _database.RatingsPersonCreatorRole.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> GetAllCreatorRoleTypes(RoleTypeQueryParameters query)
    {
        IEnumerable<PersonCreatorRoleType> rawData = await _database.PersonCreatorRoleTypes.ToListAsync();
        IEnumerable<RoleTypeResponse> data = rawData.Select(x => new RoleTypeResponse(x));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> GetCreatorRoleType(short id)
    {
        PersonCreatorRoleType? item = await _database.PersonCreatorRoleTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        RoleTypeResponse data = new RoleTypeResponse(item);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> PostCreatorRoleType(RoleTypeRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        PersonCreatorRoleType item = data.CreateCreatorRoleType();
        await _database.PersonCreatorRoleTypes.AddAsync(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/creator/{item.Id}", new RoleTypeResponse(item));
    }
    
    public async Task<RequestResult> DeleteCreatorRoleType(short id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PersonCreatorRoleType? item = await _database.PersonCreatorRoleTypes.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NoContent();
        }

        _database.PersonCreatorRoleTypes.Attach(item);
        _database.PersonCreatorRoleTypes.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.NoContent();
    }
    
    #endregion
    
    #endregion
}