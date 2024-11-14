using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model;
using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Generics.Rating;
using WatchIt.Common.Model.People;
using WatchIt.Common.Model.People.Person;
using WatchIt.Common.Model.Roles;
using WatchIt.Common.Model.Roles.Role;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.People;
using WatchIt.Database.Model.Roles;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;

namespace WatchIt.WebAPI.Services.Controllers.Persons;

public class PersonsControllerService : IPersonsControllerService
{
    #region SERVICES

    private readonly DatabaseContext _database;
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public PersonsControllerService(DatabaseContext database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main

    public async Task<RequestResult> GetPersons(PersonResponseQueryParameters query)
    {
        IEnumerable<Person> rawData = await _database.People.ToListAsync();
        IEnumerable<PersonResponse> data = rawData.Select(x => x.ToPersonResponse())
                                                  .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetPerson(long id)
    {
        Person? item = await _database.People.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(item.ToPersonResponse());
    }
    
    public async Task<RequestResult> PostPerson(PersonRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        Person entity = data.ToPersonEntity();
        await _database.People.AddAsync(entity);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"persons/{entity.Id}", entity.ToPersonResponse());
    }
    
    public async Task<RequestResult> PutPerson(long id, PersonRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? item = await _database.People.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        item.UpdateWithPersonRequest(data);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }
    
    public async Task<RequestResult> DeletePerson(long id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? item = await _database.People.FindAsync(id);
        if (item is not null)
        {
            _database.People.Attach(item);
            _database.People.Remove(item);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.NoContent();
    }
    
    #endregion

    #region View count

    public async Task<RequestResult> GetPersonsViewRank(int first, int days)
    {
        if (first < 1 || days < 1)
        {
            return RequestResult.BadRequest();
        }
        
        DateOnly startDate = DateOnly.FromDateTime(DateTime.Now).AddDays(-days);
        IEnumerable<Person> rawData = await _database.People.OrderByDescending(x => x.ViewCounts
                                                                                     .Where(y => y.Date >= startDate)
                                                                                     .Sum(y => y.ViewCount))
                                                            .ThenBy(x => x.Id)
                                                            .Take(first)
                                                            .ToListAsync();
        
        IEnumerable<PersonResponse> data = rawData.Select(x => x.ToPersonResponse());
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostPersonsView(long id)
    {
        Person? item = await _database.People.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
        PersonViewCount? viewCount = await _database.PersonViewCounts.FirstOrDefaultAsync(x => x.PersonId == id && x.Date == dateNow);
        if (viewCount is null)
        {
            viewCount = new PersonViewCount
            {
                PersonId = id,
                Date = dateNow,
                ViewCount = 1
            };
            await _database.PersonViewCounts.AddAsync(viewCount);
        }
        else
        {
            viewCount.ViewCount++;
        }
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok();
    }

    #endregion
    
    #region Photo
    
    public async Task<RequestResult> GetPersonPhoto(long id)
    {
        PersonPicture? picture = await _database.PersonPictures.FindAsync(id);
        if (picture is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(picture.ToImageResponse());
    }

    public async Task<RequestResult> PutPersonPhoto(long id, ImageRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? person = await _database.People.FindAsync(id);
        if (person is null)
        {
            return RequestResult.BadRequest();
        }
        
        PersonPicture? picture = person.Picture;
        if (picture is null)
        {
            picture = data.ToPersonPictureEntity(id);
        }
        else
        {
            picture.UpdateImageEntityWithRequest(data);
        }
        
        await _database.SaveChangesAsync();
        return RequestResult.Ok(picture.ToImageResponse());
    }

    public async Task<RequestResult> DeletePersonPhoto(long id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        PersonPicture? picture = await _database.PersonPictures.FindAsync(id);
        if (picture is not null)
        {
            _database.PersonPictures.Attach(picture);
            _database.PersonPictures.Remove(picture);
            await _database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }
    
    #endregion
    
    #region Roles
    
    public async Task<RequestResult> GetPersonAllActorRoles(long id, RoleActorResponseQueryParameters query)
    {
        Person? person = await _database.People.FindAsync(id);
        if (person is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<RoleActorResponse> data = person.Roles
                                                    .OfType<RoleActor>()
                                                    .Select(x => x.ToRoleActorResponse())
                                                    .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetPersonAllCreatorRoles(long id, RoleCreatorResponseQueryParameters query)
    {
        Person? person = await _database.People.FindAsync(id);
        if (person is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<RoleCreatorResponse> data = person.Roles
                                                    .OfType<RoleCreator>()
                                                    .Select(x => x.ToRoleCreatorResponse())
                                                    .PrepareData(query);
        return RequestResult.Ok(data);
    }
    
    #endregion

    #region Rating

    public async Task<RequestResult> GetPersonGlobalRating(long id)
    {
        Person? item = await _database.People.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        RatingOverallResponse response = item.Roles
                                             .SelectMany(x => x.Ratings)
                                             .GetRatingResponseFromRatingEntitiesCollection();
        return RequestResult.Ok(response);
    }

    public async Task<RequestResult> GetPersonUserRating(long id, long userId)
    {
        Person? item = await _database.People.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        RatingUserOverallResponse response = item.Roles
                                                 .SelectMany(x => x.Ratings
                                                                   .Where(y => y.AccountId == userId))
                                                 .GetRatingUserOverallResponseFromRatingEntitiesCollection();
        return RequestResult.Ok(response);
    }

    #endregion
    
    #endregion
}