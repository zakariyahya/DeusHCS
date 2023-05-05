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

namespace WebAdmin.Pages.JobFitReports
{
    public partial class AddJobFitReport
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
        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForUserId;
        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.JobFitReport> grid0;

        protected override async Task OnInitializedAsync()
        {
            usersForUserId = await adminPanelProjectService.GetUsers();
            jobFitReport = new WebAdmin.Models.adminPanelProject.JobFitReport();
        }
        protected bool isBusy;

        protected bool errorVisible;
        protected WebAdmin.Models.adminPanelProject.JobFitReport jobFitReport;

        protected async Task FormSubmit()
        {
            try
            {
                await adminPanelProjectService.CreateJobFitReport(jobFitReport);
                DialogService.Close(jobFitReport);
            }
            catch (Exception ex)
            {
                errorVisible = true;
            }
        }

        IFileListEntry file;
        List<WebAdmin.Models.adminPanelProject.JobFitReport> jobFitReports =
            new List<WebAdmin.Models.adminPanelProject.JobFitReport>();

        WebAdmin.Models.adminPanelProject.JobFitReport jobFitReportDb =
            new WebAdmin.Models.adminPanelProject.JobFitReport();

        async Task HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            if (file != null)
            {
                adminPanelProjectService.Upload(file);
                jobFitReports = adminPanelProjectService?.ReadJobFitReportExcel();
                foreach (var item in jobFitReports)
                {
                    // var company = adminPanelProjectService.GetUserById(item.id);
                    var jobFitReport = adminPanelProjectService.GetJobFitReportByID(item.id);
                    jobFitReportDb = jobFitReport;
                }
            }
        }

        protected string info;
        protected bool infoVisible;

        private void AddExcel()
        {
            try
            {
                adminPanelProjectService.AddJobFitReportExcel(jobFitReports);
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

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }

        bool hasUseridValue;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            jobFitReport = new WebAdmin.Models.adminPanelProject.JobFitReport();

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
