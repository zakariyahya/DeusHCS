using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebAdmin.Pages.Admin.Assessments
{
    public partial class Assessments
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

        protected IEnumerable<WebAdmin.Models.adminPanelProject.Assessment> assessments;

        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.Assessment> grid0;

        protected string search = "";
        IEnumerable<string> selectedCompanyNames;
        protected List<WebAdmin.Models.ApplicationUser> usersForUserId;

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            assessments = await adminPanelProjectService.GetAssessments(new Query { Filter = $@"i => i.id.Contains(@0) || i.UserId.Contains(@0)  ", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            assessments = await adminPanelProjectService.GetAssessments(new Query { Filter = $@"i => i.id.Contains(@0) || i.UserId.Contains(@0) ", FilterParameters = new object[] { search } });
            usersForUserId = await adminPanelProjectService.GetAllUsers();
            

        }


        void OnSelectedCompanyNamesChange(object value)
        {
            if (selectedCompanyNames != null && !selectedCompanyNames.Any())
            {
                selectedCompanyNames = null;
            }
        }
        protected async Task AddButtonClick(MouseEventArgs args)
        {
            // await grid0.InsertRow(new WebAdmin.Models.adminPanelProject.Assessment());
            await DialogService.OpenAsync<AddAssessment>("Add Assessment", null);
            await grid0.Reload();
        }

        protected async Task GridDeleteButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.Assessment assessment)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var deleteResult = await adminPanelProjectService.DeleteAssessment(assessment.id);

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
                    Detail = $"Unable to delete Assessment"
                });
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await adminPanelProjectService.ExportAssessmentsToCSV(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
                }, "Assessments");
            }

            if (args == null || args.Value == "xlsx")
            {
                await adminPanelProjectService.ExportAssessmentsToExcel(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
                }, "Assessments");
            }
        }

        protected async Task GridRowUpdate(WebAdmin.Models.adminPanelProject.Assessment args)
        {
            await adminPanelProjectService.UpdateAssessment(args.id, args);
        }

        protected async Task GridRowCreate(WebAdmin.Models.adminPanelProject.Assessment args)
        {
            await adminPanelProjectService.CreateAssessment(args);
            await grid0.Reload();
        }

        protected async Task EditButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.Assessment data)
        {
            await grid0.EditRow(data);
        }

        protected async Task SaveButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.Assessment data)
        {
            await grid0.UpdateRow(data);
        }

        protected async Task CancelButtonClick(MouseEventArgs args, WebAdmin.Models.adminPanelProject.Assessment data)
        {
            grid0.CancelEditRow(data);
            await adminPanelProjectService.CancelAssessmentChanges(data);
        }
    }
}