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
            // jobFitReport = new WebAdmin.Models.adminPanelProject.JobFitReport();
        }

        protected bool isBusy;

        protected bool errorVisible;
        protected WebAdmin.Models.adminPanelProject.JobFitReport jobFitReport;

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
            }
        }

        protected async Task FormSubmit()
        {
            // try
            // {
            //     foreach (var item in jobFitReports)
            //     {
            //         isBusy = true;
            //         item.UserId = jobFitReport.UserId;
            //         adminPanelProjectService.CreateJobFitReport(item);
            //     }
            //     DialogService.Close(jobFitReport);
            // }
            // catch (Exception ex)
            // {
            //     errorVisible = true;
            // }
            foreach (var item in jobFitReports)
            {
                isBusy = true;
                item.UserId = jobFitReport.UserId;
                adminPanelProjectService.CreateJobFitReport(item);
            }
            DialogService.Close(jobFitReport);
        }

        protected string info;
        protected bool infoVisible;

        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }

        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }

        bool hasUseridValue;

        [Parameter]
        public string UserId { get; set; }

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            jobFitReport = new WebAdmin.Models.adminPanelProject.JobFitReport();
            hasUseridValue = parameters.TryGetValue<string>("UserId", out var hasUseridResult);
            await base.SetParametersAsync(parameters);
        }
    }
}
