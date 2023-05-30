using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebAdmin.Pages.Admin.Employees
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
            employee = await adminPanelProjectService.GetEmployeeById(id);
        }
        protected bool errorVisible;
        protected WebAdmin.Models.adminPanelProject.Employee employee;

        protected async Task FormSubmit()
        {
            try
            {
                await adminPanelProjectService.UpdateEmployee(id, employee);
                NavigationManager.NavigateTo("employees");
            }
            catch (Exception ex)
            {
                hasChanges = ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException;
                canEdit = !(ex is Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException);
                errorVisible = true;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            NavigationManager.NavigateTo("employees");
        }


        protected bool hasChanges = false;
        protected bool canEdit = true;


        protected async Task ReloadButtonClick(MouseEventArgs args)
        {
           adminPanelProjectService.Reset();
            hasChanges = false;
            canEdit = true;

            employee = await adminPanelProjectService.GetEmployeeById(id);
        }
    }
}