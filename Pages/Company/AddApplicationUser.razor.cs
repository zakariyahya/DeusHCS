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
using WebAdmin.Models;

namespace WebAdmin.Pages.Company
{
    public partial class AddApplicationUser
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
        protected IEnumerable<WebAdmin.Models.ApplicationRole> roles;
        protected WebAdmin.Models.ApplicationUser user;
        protected IEnumerable<string> userRoles = Enumerable.Empty<string>();
        protected bool isBusy;
        IFileListEntry file;

        protected string error;
        protected string CompanyName;

        protected bool errorVisible;
        protected bool errorExtension;
        protected string errorExt;

        protected string info;
        protected bool infoVisible;
        ApplicationUser companyDb = new ApplicationUser();
        List<ApplicationUser> companies = new List<ApplicationUser>();

        [Inject]
        protected SecurityService Security { get; set; }
        // private List<ApplicationUser> ReadExcel()
        // {
        //     companies = adminPanelProjectService?.ReadExcel();
        //     return companies;
        // }
        protected override async Task OnInitializedAsync()
        {
            // ReadExcel();
            user = new WebAdmin.Models.ApplicationUser();

            roles = await Security.GetRoles();
        }

        async Task HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            if (file != null)
            {
                if (file.Type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {
                    errorExtension = true;
                    errorExt = "Upload file excel only";
                }
                adminPanelProjectService.Upload(file);
                companies = adminPanelProjectService.ReadExcel();
                foreach (var item in companies)
                {
                    var company = adminPanelProjectService.GetUserById(item.Id);
                    companyDb = company;
                }
            }

        }
        private void AddExcel()
        {
            try
            {
                adminPanelProjectService.AddExcel(companies);
                // Message = "Data Saved";
                infoVisible = true;

                info = "Companies was created";
            }
            catch (System.Exception ex)
            {
                errorVisible = true;
                // Message = ex.Message;
            }

        }
        protected async Task FormSubmit(WebAdmin.Models.ApplicationUser user)
        {
            try
            {
                isBusy = true;
                user.Roles = roles.Where(role => userRoles.Contains(role.Id)).ToList();
                await Security.Register(user.Email, user.Password, user.CompanyName);
                DialogService.Close(null);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
            isBusy = false;
        }


        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
    }
}