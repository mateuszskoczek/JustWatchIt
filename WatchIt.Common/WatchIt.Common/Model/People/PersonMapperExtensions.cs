using WatchIt.Common.Model.Genders;
using WatchIt.Common.Model.Generics.Image;
using WatchIt.Common.Model.Generics.Rating;
using WatchIt.Common.Model.People.Person;

namespace WatchIt.Common.Model.People;

public static class PersonMapperExtensions
{
    #region PUBLIC METHODS

    #region Person

    public static Database.Model.People.Person ToPersonEntity(this PersonRequest request) => new Database.Model.People.Person
    {
        Name = request.Name,
        FullName = request.FullName,
        Description = request.Description,
        BirthDate = request.BirthDate,
        DeathDate = request.DeathDate,
        GenderId = request.GenderId,
    };

    public static void UpdateWithPersonRequest(this Database.Model.People.Person entity, PersonRequest request)
    {
        entity.Name = request.Name;
        entity.FullName = request.FullName;
        entity.Description = request.Description;
        entity.BirthDate = request.BirthDate;
        entity.DeathDate = request.DeathDate;
        entity.GenderId = request.GenderId;
    }

    public static PersonResponse ToPersonResponse(this Database.Model.People.Person entity)
    {
        PersonResponse response = new PersonResponse();
        response.SetMediumResponseProperties(entity);
        return response;
    }

    public static PersonUserRatedResponse ToPersonUserRatedResponse(this Database.Model.People.Person entity, long accountId)
    {
        PersonUserRatedResponse response = new PersonUserRatedResponse();
        response.SetMediumResponseProperties(entity);
        response.RatingUser = entity.Roles.SelectMany(x => x.Ratings.Where(y => y.AccountId == accountId)).GetRatingUserOverallResponseFromRatingEntitiesCollection();
        return response;
    }

    #endregion

    #region PersonPicture

    public static Database.Model.People.PersonPicture ToPersonPictureEntity(this ImageRequest request, long personId) => new Database.Model.People.PersonPicture
    {
        PersonId = personId,
        Image = request.Image,
        MimeType = request.MimeType,
    };

    #endregion

    #endregion
    
    
    
    #region PRIVATE METHODS
    
    private static void SetMediumResponseProperties(this PersonResponse response, Database.Model.People.Person entity)
    {
        response.Id = entity.Id;
        response.Name = entity.Name;
        response.FullName = entity.FullName;
        response.Description = entity.Description;
        response.BirthDate = entity.BirthDate;
        response.DeathDate = entity.DeathDate;
        response.Gender = entity.Gender?.ToGenderResponse();
        response.Rating = entity.Roles.SelectMany(x => x.Ratings).GetRatingResponseFromRatingEntitiesCollection();
    }
    
    #endregion
}