using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Routing;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using WebAdmin.Models;

namespace WebAdmin.Pages
{
    public partial class Index
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

  [Inject]
        protected adminPanelProjectService adminPanelProjectService { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }
        [Inject]
        protected IJSRuntime js {get; set;}
        [Inject]
       UserManager<ApplicationUser> userManager {get; set;}
        
        protected WebAdmin.Models.ApplicationUser user;
        protected string userData;



        [CascadingParameter]
        private Task<AuthenticationState> authenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {

            var authState = await authenticationState;
            var userId = authState.User.Claims.FirstOrDefault().Value.ToString();

            user = await Security.GetUserById(userId);
            userData = user.CompanyName;

        }
        private async Task DisplayGreetingAlert()
        {
            // var authState = await authenticationState;
            // var userId = authState.User.Claims.FirstOrDefault().Value.ToString();
            // var message = $"Hello {userId}";
            // await js.InvokeVoidAsync("alert", message);
        }
    }
}