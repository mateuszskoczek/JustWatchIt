using Microsoft.AspNetCore.Components;
using WatchIt.Common.Model.Accounts;
using WatchIt.Common.Model.Photos;
using WatchIt.Website.Services.Utility.Authentication;
using WatchIt.Website.Services.Utility.Tokens;
using WatchIt.Website.Services.WebAPI.Accounts;
using WatchIt.Website.Services.WebAPI.Media;
using WatchIt.Website.Services.WebAPI.Photos;

namespace WatchIt.Website.Layout;

public partial class MainLayout : LayoutComponentBase
{
    #region SERVICES
    
    [Inject] public ILogger<MainLayout> Logger { get; set; } = default!;
    [Inject] public NavigationManager NavigationManager { get; set; } = default!;
    [Inject] public ITokensService TokensService { get; set; } = default!;
    [Inject] public IAuthenticationService AuthenticationService { get; set; } = default!;
    [Inject] public IAccountsWebAPIService AccountsWebAPIService { get; set; } = default!;
    [Inject] public IMediaWebAPIService MediaWebAPIService { get; set; } = default!;
    [Inject] public IPhotosWebAPIService PhotosWebAPIService { get; set; } = default!;
    
    #endregion
    
    
    
    #region FIELDS

    private bool _loaded = false;
    
    private User? _user = null;
    private AccountProfilePictureResponse? _userProfilePicture;
    
    private bool _menuUserIsActive;
    
    #endregion
    
    
    
    #region PROPERTIES
    
    public PhotoResponse? BackgroundPhoto { get; set; }
    
    #endregion
    
    
    
    #region PRIVATE METHODS
    
    #region Main
    
    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            List<Task> endTasks = new List<Task>();
            List<Task> step1Tasks = new List<Task>();
            
            // STEP 0
            step1Tasks.AddRange(
            [
                Task.Run(async () => _user = await AuthenticationService.GetUserAsync())
            ]);
            endTasks.AddRange(
            [
                PhotosWebAPIService.GetPhotoRandomBackground(data => BackgroundPhoto = data)
            ]);
            
            // STEP 1
            await Task.WhenAll(step1Tasks);
            if (_user is not null)
            {
                endTasks.AddRange(
                [
                    AccountsWebAPIService.GetAccountProfilePicture(_user.Id, data => _userProfilePicture = data)
                ]);
            }
            
            // END
            await Task.WhenAll(endTasks);
            
            _loaded = true;
            StateHasChanged();
        }
    }
    
    #endregion
    
    #region User menu
    
    private async Task UserMenuLogOut()
    {
        await AuthenticationService.LogoutAsync();
        await TokensService.RemoveAuthenticationData();
        NavigationManager.Refresh(true);
    }
    
    #endregion
    
    #endregion
}