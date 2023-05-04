using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebAdmin.Pages.JobFitReports
{
    public partial class EditJobFitReport
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
            jobFitReport = await adminPanelProjectService.GetJobFitReportById(id);
        }
        protected bool errorVisible;
        protected WebAdmin.Models.adminPanelProject.JobFitReport jobFitReport;

        protected async Task FormSubmit()
        {
            try
            {
                await adminPanelProjectService.UpdateJobFitReport(id, jobFitReport);
                DialogService.Close(jobFitReport);
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
    }
}