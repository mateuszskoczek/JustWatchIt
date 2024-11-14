using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Genders;
using WatchIt.Common.Model.Genders.Gender;
using WatchIt.Common.Query;
using WatchIt.Database;
using WatchIt.Database.Model.Genders;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;
using GenderResponse = WatchIt.Common.Model.Genders.Gender.GenderResponse;

namespace WatchIt.WebAPI.Services.Controllers.Genders;

public class GendersControllerService : IGendersControllerService
{
    #region SERVICES

    private readonly DatabaseContext _database;
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public GendersControllerService(DatabaseContext database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion



    #region PUBLIC METHODS
    
    public async Task<RequestResult> GetGenders(GenderResponseQueryParameters query)
    {
        IEnumerable<Gender> rawData = await _database.Genders.ToListAsync();
        IEnumerable<GenderResponse> data = rawData.Select(x => x.ToGenderResponse()).PrepareData(query);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> GetGender(short id)
    {
        Gender? item = await _database.Genders.FindAsync(id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }
        return RequestResult.Ok(item.ToGenderResponse());
    }

    public async Task<RequestResult> PostGender(GenderRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Gender entity = data.ToGenderEntity();
        await _database.Genders.AddAsync(entity);
        await _database.SaveChangesAsync();
        
        return RequestResult.Ok(entity.ToGenderResponse());
    }
    
    public async Task<RequestResult> DeleteGender(short id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Gender? item = await _database.Genders.FindAsync(id);
        if (item is not null)
        {
            _database.Genders.Attach(item);
            _database.Genders.Remove(item);
            await _database.SaveChangesAsync();
        }
        
        return RequestResult.NoContent();
    }
    
    #endregion
}