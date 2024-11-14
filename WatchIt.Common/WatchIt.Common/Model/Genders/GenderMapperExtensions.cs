using WatchIt.Common.Model.Genders.Gender;

namespace WatchIt.Common.Model.Genders;

public static class GenderMapperExtensions
{
    #region PUBLIC METHODS

    public static Gender.GenderResponse ToGenderResponse(this Database.Model.Genders.Gender entity) => new Gender.GenderResponse
    {
        Id = entity.Id,
        Name = entity.Name,
    };

    public static Database.Model.Genders.Gender ToGenderEntity(this GenderRequest request) => new Database.Model.Genders.Gender
    {
        Name = request.Name,
    };

    #endregion
}