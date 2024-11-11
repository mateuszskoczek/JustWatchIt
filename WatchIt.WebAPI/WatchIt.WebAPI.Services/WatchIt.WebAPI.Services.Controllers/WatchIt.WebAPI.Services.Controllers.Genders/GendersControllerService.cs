using Microsoft.EntityFrameworkCore;
using WatchIt.Common.Model.Genders;
using WatchIt.Database;
using WatchIt.Database.Model.Common;
using WatchIt.WebAPI.Services.Controllers.Common;
using WatchIt.WebAPI.Services.Utility.User;
using Gender = WatchIt.Database.Model.Common.Gender;

namespace WatchIt.WebAPI.Services.Controllers.Genders;

public class GendersControllerService : IGendersControllerService
{
    #region SERVICES

    private readonly DatabaseContextOld _database;
    private readonly IUserService _userService;
    
    #endregion
    
    
    
    #region CONSTRUCTORS

    public GendersControllerService(DatabaseContextOld database, IUserService userService)
    {
        _database = database;
        _userService = userService;
    }
    
    #endregion



    #region PUBLIC METHODS
    
    #region Main
    
    public async Task<RequestResult> GetAllGenders(GenderQueryParameters query)
    {
        IEnumerable<Gender> rawData = await _database.Genders.ToListAsync();
        IEnumerable<GenderResponse> data = rawData.Select(x => new GenderResponse(x));
        data = query.PrepareData(data);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> GetGender(short id)
    {
        Gender? item = await _database.Genders.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NotFound();
        }

        GenderResponse data = new GenderResponse(item);
        return RequestResult.Ok(data);
    }

    public async Task<RequestResult> PostGender(GenderRequest data)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }

        Gender item = data.CreateGender();
        await _database.Genders.AddAsync(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.Created($"genres/{item.Id}", new GenderResponse(item));
    }
    
    public async Task<RequestResult> DeleteGender(short id)
    {
        UserValidator validator = _userService.GetValidator().MustBeAdmin();
        if (!validator.IsValid)
        {
            return RequestResult.Forbidden();
        }
        
        Gender? item = await _database.Genders.FirstOrDefaultAsync(x => x.Id == id);
        if (item is null)
        {
            return RequestResult.NoContent();
        }

        _database.Genders.Attach(item);
        _database.Genders.Remove(item);
        await _database.SaveChangesAsync();
        
        return RequestResult.NoContent();
    }
    
    #endregion
    
    #endregion
}