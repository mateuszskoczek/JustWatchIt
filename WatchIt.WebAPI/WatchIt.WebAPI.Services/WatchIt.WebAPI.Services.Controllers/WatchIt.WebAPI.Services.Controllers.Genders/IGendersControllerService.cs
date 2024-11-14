using WatchIt.Common.Model.Genders;
using WatchIt.Common.Model.Genders.Gender;
using WatchIt.WebAPI.Services.Controllers.Common;

namespace WatchIt.WebAPI.Services.Controllers.Genders;

public interface IGendersControllerService
{
    #region METHODS
    
    Task<RequestResult> GetGenders(GenderResponseQueryParameters query);
    Task<RequestResult> GetGender(short id);
    Task<RequestResult> PostGender(GenderRequest data);
    Task<RequestResult> DeleteGender(short id);
    
    #endregion
}