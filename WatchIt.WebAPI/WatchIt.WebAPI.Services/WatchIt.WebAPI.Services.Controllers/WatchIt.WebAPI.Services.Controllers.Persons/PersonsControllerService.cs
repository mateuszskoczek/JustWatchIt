using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Persons;
using WatchIt.Common.Model.Rating;
using WatchIt.Common.Model.Roles;
using WatchIt.Database;
using WatchIt.Database.Model.Person;
using WatchIt.Database.Model.Rating;
using WatchIt.Database.Model.ViewCount;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;
using Person = WatchIt.Database.Model.Person.Person;

namespace WatchIt.WebAPI.Services.Controllers.Persons;

public class PersonsControllerService : IPersonsControllerService
{
    #region SERVICES

    private readonly DatabaseContextOld _database;
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public PersonsControllerService(DatabaseContextOld database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion
    
    
    
    #region PUBLIC METHODS
    
    #region Main

    public async Task<RequestResult> GetAllPersons(PersonQueryParameters query)
    {
        IEnumerable<Person> rawData = await _database.Persons.ToListAsync();
        IEnumerable<PersonResponse> data = rawData.Select(x => new PersonResponse(x));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> GetPerson(long id)
    {
        Person? item = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        PersonResponse data = new PersonResponse(item);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostPerson(PersonRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        Person personItem = data.CreatePerson();
        await _database.Persons.AddAsync(personItem);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"persons/{personItem.Id}", new PersonResponse(personItem));
    }
    
    public async Task<RequestResult> PutPerson(long id, PersonRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? item = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        data.UpdatePerson(item);
        
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
        
        Person? item = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        _database.PersonCreatorRoles.AttachRange(item.PersonCreatorRoles);
        _database.PersonCreatorRoles.RemoveRange(item.PersonCreatorRoles);
        _database.PersonActorRoles.AttachRange(item.PersonActorRoles);
        _database.PersonActorRoles.RemoveRange(item.PersonActorRoles);
        _database.ViewCountsPerson.AttachRange(item.ViewCountsPerson);
        _database.ViewCountsPerson.RemoveRange(item.ViewCountsPerson);
        _database.Persons.Attach(item);
        _database.Persons.Remove(item);
        await _database.SaveChangesAsync();
        
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
        IEnumerable<Person> rawData = await _database.Persons.OrderByDescending(x => x.ViewCountsPerson.Where(y => y.Date >= startDate)
                                                                                                       .Sum(y => y.ViewCount))
                                                             .ThenBy(x => x.Id)
                                                             .Take(first)
                                                             .ToListAsync();
        
        IEnumerable<PersonResponse> data = rawData.Select(x => new PersonResponse(x));
        
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostPersonsView(long personId)
    {
        Database.Model.Media.Media? item = await _database.Media.FirstOrDefaultAsync(x => x.Id == personId);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        DateOnly dateNow = DateOnly.FromDateTime(DateTime.Now);
        ViewCountPerson? viewCount = await _database.ViewCountsPerson.FirstOrDefaultAsync(x => x.PersonId == personId && x.Date == dateNow);
        if (viewCount is null)
        {
            viewCount = new ViewCountPerson
            {
                PersonId = personId,
                Date = dateNow,
                ViewCount = 1
            };
            await _database.ViewCountsPerson.AddAsync(viewCount);
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
        Person? person = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (person is null)
        {
            return RequestResult.BadRequest();
        }

        PersonPhotoImage? photo = person.PersonPhoto;
        if (photo is null)
        {
            return RequestResult.NotFound();
        }

        PersonPhotoResponse data = new PersonPhotoResponse(photo);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> PutPersonPhoto(long id, PersonPhotoRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? person = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (person is null)
        {
            return RequestResult.BadRequest();
        }

        if (person.PersonPhoto is null)
        {
            PersonPhotoImage image = data.CreatePersonPhotoImage();
            await _database.PersonPhotoImages.AddAsync(image);
            await _database.SaveChangesAsync();

            person.PersonPhotoId = image.Id;
        }
        else
        {
            data.UpdatePersonPhotoImage(person.PersonPhoto);
        }
        
        await _database.SaveChangesAsync();

        PersonPhotoResponse returnData = new PersonPhotoResponse(person.PersonPhoto);
        return RequestResult.Ok(returnData);
    }

    public async Task<RequestResult> DeletePersonPhoto(long id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? person = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);

        if (person?.PersonPhoto != null)
        {
            _database.PersonPhotoImages.Attach(person.PersonPhoto);
            _database.PersonPhotoImages.Remove(person.PersonPhoto);
            await _database.SaveChangesAsync();
        }

        return RequestResult.NoContent();
    }
    
    #endregion
    
    #region Roles
    
    public async Task<RequestResult> GetPersonAllActorRoles(long personId, ActorRolePersonQueryParameters queryParameters)
    {
        Database.Model.Person.Person? person = await _database.Persons.FirstOrDefaultAsync(x => x.Id == personId);
        if (person is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<PersonActorRole> dataRaw = await _database.PersonActorRoles.Where(x => x.PersonId == personId).ToListAsync();
        IEnumerable<ActorRoleResponse> data = dataRaw.Select(x => new ActorRoleResponse(x));
        data = queryParameters.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostPersonActorRole(long personId, ActorRolePersonRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? person = await _database.Persons.FirstOrDefaultAsync(x => x.Id == personId);
        if (person is null)
        {
            return RequestResult.NotFound();
        }

        PersonActorRole item = data.CreateActorRole(personId);
        await _database.PersonActorRoles.AddAsync(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/actor/{item.Id}", new ActorRoleResponse(item));
    }
    
    public async Task<RequestResult> GetPersonAllCreatorRoles(long personId, CreatorRolePersonQueryParameters queryParameters)
    {
        Person? media = await _database.Persons.FirstOrDefaultAsync(x => x.Id == personId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }
            
        IEnumerable<PersonCreatorRole> dataRaw = await _database.PersonCreatorRoles.Where(x => x.PersonId == personId).ToListAsync();
        IEnumerable<CreatorRoleResponse> data = dataRaw.Select(x => new CreatorRoleResponse(x));
        data = queryParameters.PrepareData(data);
        return RequestResult.Ok(data);
    }
    
    public async Task<RequestResult> PostPersonCreatorRole(long personId, CreatorRolePersonRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Person? media = await _database.Persons.FirstOrDefaultAsync(x => x.Id == personId);
        if (media is null)
        {
            return RequestResult.NotFound();
        }

        PersonCreatorRole item = data.CreateCreatorRole(personId);
        await _database.PersonCreatorRoles.AddAsync(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"roles/creator/{item.Id}", new CreatorRoleResponse(item));
    }
    
    #endregion

    #region Rating

    public async Task<RequestResult> GetPersonGlobalRating(long id)
    {
        Person? item = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        
        RatingResponse ratingResponse = RatingResponseBuilder.Initialize()
                                                             .Add(item.PersonActorRoles.SelectMany(x => x.RatingPersonActorRole), x => x.Rating)
                                                             .Add(item.PersonCreatorRoles.SelectMany(x => x.RatingPersonCreatorRole), x => x.Rating)
                                                             .Build();
        
        return RequestResult.Ok(ratingResponse);
    }

    public async Task<RequestResult> GetPersonUserRating(long id, long userId)
    {
        Person? item = await _database.Persons.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        RatingResponse ratingResponse = RatingResponseBuilder.Initialize()
                                                             .Add(item.PersonActorRoles.SelectMany(x => x.RatingPersonActorRole).Where(x => x.AccountId == userId), x => x.Rating)
                                                             .Add(item.PersonCreatorRoles.SelectMany(x => x.RatingPersonCreatorRole).Where(x => x.AccountId == userId), x => x.Rating)
                                                             .Build();
        
        return RequestResult.Ok(ratingResponse);
    }

    #endregion
    
    #endregion
}