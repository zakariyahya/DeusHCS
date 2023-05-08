using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebAdmin.Pages.Employees
{
    public partial class EditPlayer
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

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
        public adminPanelProjectService adminPanelProjectService { get; set; }

        [Parameter]
        public string id { get; set; }

        protected override async Task OnInitializedAsync()
        {
            employee = await adminPanelProjectService.GetPlayerById(id);

            usersForUserId = await adminPanelProjectService.GetUsers();

        }
        protected bool errorVisible;
        protected WebAdmin.Models.adminPanelProject.Employee employee;
        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForUserId;

        // protected IEnumerable<WebAdmin.Models.adminPanelProject.Company> companiesForCompanyid;

        protected async Task FormSubmit()
        {
            try
            {
                await adminPanelProjectService.UpdatePlayer(id, employee);
                DialogService.Close(employee);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }





        bool hasUseridValue;

        [Parameter]
        public string userid { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            employee = new WebAdmin.Models.adminPanelProject.Employee();

            hasUseridValue = parameters.TryGetValue<string>("userid", out var hasUseridResult);

            if (hasUseridValue)
            {
                employee.userid = hasUseridResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}