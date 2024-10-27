using Microsoft.AspNetCore.Components;
using WatchIt.Website.Services.Client.Media;
using WatchIt.Website.Services.Client.Persons;
using WatchIt.Website.Services.Client.Roles;

namespace WatchIt.Website.Components.Pages.PersonPage.Panels;

public partial class PersonActorRolesPanelComponent : ComponentBase
{
    #region SERVICES
    
    [Inject] private IMediaClientService MediaClientService { get; set; } = default!;
    [Inject] private IPersonsClientService PersonsClientService { get; set; } = default!;
    [Inject] private IRolesClientService RolesClientService { get; set; } = default!;
    
    #endregion
    
    
    
    #region PARAMETERS
    
    [Parameter] public string Class { get; set; } = string.Empty;
    [Parameter] public required long Id { get; set; }
    [Parameter] public Action? OnRatingChanged { get; set; }
    
    #endregion
}