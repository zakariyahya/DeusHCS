using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BlazorInputFile;

namespace WebAdmin.Pages.Employees
{
    public partial class AddPlayer
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
        IFileListEntry file;

        // Employee
        WebAdmin.Models.adminPanelProject.Employee employeeDb =
            new WebAdmin.Models.adminPanelProject.Employee();
        List<WebAdmin.Models.adminPanelProject.Employee> employees =
            new List<WebAdmin.Models.adminPanelProject.Employee>();

        // [Parameter]
        // public string UserId { get; set; }
        protected WebAdmin.Models.adminPanelProject.Employee employee;

        protected override async Task OnInitializedAsync()
        {
            employee = new WebAdmin.Models.adminPanelProject.Employee();
            usersForUserId = await adminPanelProjectService.GetUsers();
        }

        protected bool errorVisible;
        protected string error;
        protected bool isBusy;

        protected string info;
        protected bool infoVisible;
        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForUserId;
        protected WebAdmin.Models.ApplicationUser user;

        [Parameter]
        public string UserId { get; set; }

        async Task HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            if (file != null)
            {
                adminPanelProjectService.Upload(file);
                employees = adminPanelProjectService?.ReadEmployeeExcel();
                foreach (var item in employees)
                {
                    // var company = adminPanelProjectService.GetUserById(item.id);
                    var employee = adminPanelProjectService.GetPlayerByID(item.id);
                    employeeDb = employee;
                }
            }
        }

        protected async Task FormSubmit()
        {
            try
            {
                isBusy = true;
                await adminPanelProjectService.CreatePlayer(employee);
                employee = new WebAdmin.Models.adminPanelProject.Employee();

                // user = adminPanelProjectService.GetUserById(UserId);
                employee.UserId = UserId;
                // employee.PhoneNumber = "0888";
                // employee.createdBy = "System";
                // player.ApplicationUser = user;
                DialogService.Close(employee);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        private void AddExcel()
        {
            try
            {
                // isBusy = true;
               
                adminPanelProjectService.AddEmployeeExcel(employees);
                //  employee = new WebAdmin.Models.adminPanelProject.Employee();
                // employee.UserId = UserId;
                infoVisible = true;
                info = "Employees was created";
                // DialogService.Close(employee);
            }
            catch (System.Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }

        bool hasUseridValue;

        [Inject]
        protected SecurityService Security { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            employee = new WebAdmin.Models.adminPanelProject.Employee();

            hasUseridValue = parameters.TryGetValue<string>("UserId", out var hasUseridResult);
            // user = adminPanelProjectService.GetUserById(UserId);
            // player.UserId = UserId;
            // player.ApplicationUser = user;
            // if (hasUseridValue)
            // {
            //     player.UserId = hasUseridResult;
            //     player.ApplicationUser = user;
            // }
            await base.SetParametersAsync(parameters);
        }
    }
}
