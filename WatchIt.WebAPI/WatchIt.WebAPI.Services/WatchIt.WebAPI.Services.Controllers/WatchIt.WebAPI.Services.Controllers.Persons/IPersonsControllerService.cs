using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.People.Person;
using WatchIt.Common.Model.Roles.Role;
using WatchIt.WebAPI.Services.Controllers.Common;

namespace WatchIt.WebAPI.Services.Controllers.Persons;

public interface IPersonsControllerService
{
    Task<RequestResult> GetPersons(PersonResponseQueryParameters query);
    Task<RequestResult> GetPerson(long id);
    Task<RequestResult> PostPerson(PersonRequest data);
    Task<RequestResult> PutPerson(long id, PersonRequest data);
    Task<RequestResult> DeletePerson(long id);
    Task<RequestResult> GetPersonsViewRank(int first, int days);
    Task<RequestResult> PostPersonsView(long id);
    Task<RequestResult> GetPersonPhoto(long id);
    Task<RequestResult> PutPersonPhoto(long id, ImageRequest data);
    Task<RequestResult> DeletePersonPhoto(long id);
    Task<RequestResult> GetPersonAllActorRoles(long id, RoleActorResponseQueryParameters query);
    Task<RequestResult> GetPersonAllCreatorRoles(long id, RoleCreatorResponseQueryParameters query);
    Task<RequestResult> GetPersonGlobalRating(long id);
    Task<RequestResult> GetPersonUserRating(long id, long userId);
}