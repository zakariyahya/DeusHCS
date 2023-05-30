using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;


namespace WebAdmin.Pages.Admin.JobFitReports
{
    public partial class JobFitReports
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

        protected IEnumerable<WebAdmin.Models.adminPanelProject.JobFitReport> jobFitReports;

        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.JobFitReport> grid0;
        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.DataForJobFitReport> grid1;


        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            jobFitReports = await adminPanelProjectService.GetJobFitReports(new Query { Filter = $@"i => i.id.Contains(@0) || i.UserId.Contains(@0) || i.EmployeeId.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            jobFitReports = await adminPanelProjectService.GetJobFitReports(new Query { Filter = $@"i => i.id.Contains(@0) || i.UserId.Contains(@0) || i.EmployeeId.Contains(@0)", FilterParameters = new object[] { search } });
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            // await grid0.InsertRow(new WebAdmin.Models.adminPanelProject.JobFitReport());
            await DialogService.OpenAsync<AddJobFitReport>("Add Job Fit Report", null);
            await grid0.Reload();
        }
        protected async Task AddButtonClick2(MouseEventArgs args)
        {
            // await grid0.InsertRow(new WebAdmin.Models.adminPanelProject.JobFitReport());
            await DialogService.OpenAsync<AddDataJobFitReport>("Add Additional Data", null);
            await grid1.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.JobFitReport jobFitReport)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await adminPanelProjectService.DeleteJobFitReport(jobFitReport.id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Summary = $"Error",
                    Detail = $"Unable to delete JobFitReport"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await adminPanelProjectService.ExportJobFitReportsToCSV(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
                }, "JobFitReports");
            }

            if (args == null || args.Value == "xlsx")
            {
                await adminPanelProjectService.ExportJobFitReportsToExcel(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
                }, "JobFitReports");
            }
        }

        protected async Task GridRowUpdate(WebAdmin.Models.adminPanelProject.JobFitReport args)
        {
            await adminPanelProjectService.UpdateJobFitReport(args.id, args);
        }

        protected async Task GridRowCreate(WebAdmin.Models.adminPanelProject.JobFitReport args)
        {
            await adminPanelProjectService.CreateJobFitReport(args);
            await grid0.Reload();
        }

        protected async Task EditButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.JobFitReport data)
        {
            await grid0.EditRow(data);
        }

        protected async Task SaveButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.JobFitReport data)
        {
            await grid0.UpdateRow(data);
        }

        protected async Task CancelButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.JobFitReport data)
        {
            grid0.CancelEditRow(data);
            await adminPanelProjectService.CancelJobFitReportChanges(data);
        }
    }
}