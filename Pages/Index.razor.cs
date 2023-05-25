using System.Net.Http;
using AdminPanel.Data;
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
        ApplicationIdentityDbContext Context
        {
            get { return this.context; }
        }
        private readonly ApplicationIdentityDbContext context;

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
        protected IJSRuntime js { get; set; }
        [Inject]
        UserManager<ApplicationUser> userManager { get; set; }


        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.Assessment> grid0;


        protected WebAdmin.Models.ApplicationUser user;
        protected string userData;



        [CascadingParameter]
        private Task<AuthenticationState> authenticationState { get; set; }
        protected IEnumerable<WebAdmin.Models.adminPanelProject.Assessment> assessmentDatas;


        protected override async Task OnInitializedAsync()
        {

            var authState = await authenticationState;
            var userId = authState.User.Claims.FirstOrDefault().Value.ToString();

            user = await Security.GetUserById(userId);
            userData = user.CompanyName;

            // assessmentDatas =  Context.GetTableNames();

        }
        protected WebAdmin.Models.adminPanelProject.Assessment assessmentData;

        string lastFilter;

        protected async void Grid0Render(
    DataGridRenderEventArgs<WebAdmin.Models.adminPanelProject.Assessment> args
)
        {
            if (grid0.Query.Filter != lastFilter)
            {
                assessmentData = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter)
            {
                await grid0.SelectRow(assessmentData);
            }

            lastFilter = grid0.Query.Filter;
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