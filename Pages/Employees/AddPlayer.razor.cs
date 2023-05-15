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

        List<WebAdmin.Models.adminPanelProject.Employee> employees =
            new List<WebAdmin.Models.adminPanelProject.Employee>();


        protected override async Task OnInitializedAsync()
        {
            usersForuserid = await adminPanelProjectService.GetUsers();
        }

        protected WebAdmin.Models.adminPanelProject.Employee employee;

        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForuserid;

        protected bool errorVisible;
        protected string error;
        protected bool isBusy;

        protected string info;
        protected bool infoVisible;
        protected bool sasa;
        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForUserId;
        protected WebAdmin.Models.ApplicationUser user;

        async Task HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            if (file != null)
            {
                adminPanelProjectService.Upload(file);
                employees = adminPanelProjectService?.ReadEmployeeExcel(file);
                // foreach (var item in employees)
                // {
                //     var employee = adminPanelProjectService.GetPlayerByID(item.id);
                //     employeeDb = employee;
                // }
            }
        }

        protected async Task FormSubmit()
        {
            foreach (var item in employees)
            {
                item.userid = employee.userid;
                item.createdTime = DateTime.Now;
                item.PhoneNumber = "0888";
                item.createdBy = "System";
                
                adminPanelProjectService.CreatePlayer(item);
          
            }
            // infoVisible = true;
            // sasa = true;
            // info = "Employees was created";
            DialogService.Close(employee);
        }

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }

        bool hasuseridValue;

        [Parameter]
        public string userid { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            employee = new WebAdmin.Models.adminPanelProject.Employee();

            hasuseridValue = parameters.TryGetValue<string>("userid", out var hasuseridResult);

            if (hasuseridValue)
            {
                employee.userid = hasuseridResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}
