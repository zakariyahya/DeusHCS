using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebAdmin.Models.adminPanelProject;

namespace WebAdmin.Pages.Visualizations
{
    public partial class Visualizations
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

        protected IEnumerable<WebAdmin.Models.adminPanelProject.Employee> employees;
        protected WebAdmin.Models.adminPanelProject.Employee employee;
        protected WebAdmin.Models.adminPanelProject.JobFitReport report;

        protected IEnumerable<WebAdmin.Models.adminPanelProject.JobFitReport> reports;
        protected List<WebAdmin.Models.adminPanelProject.DataItem> dataItem;


        protected string employeeId;
        protected double jobFitValue;
        protected string search = "";
        protected double minFRC;

        protected override async Task OnInitializedAsync()
        {
            employee = await adminPanelProjectService.GetPlayerById(employeeId);

            employees = await adminPanelProjectService.GetPlayers(
                new Query
                {
                    Filter =
                        $@"i => i.id.Contains(@0) || i.FullName.Contains(@0) || i.Email.Contains(@0) || i.Gender.Contains(@0) || i.createdBy.Contains(@0)",
                    FilterParameters = new object[] { search }
                }
            );
            reports = await adminPanelProjectService.GetJobFitReports();
            report = await adminPanelProjectService.GetJobFitReportById(employeeId);
 
        }
        bool showDataLabels = false;
        int value;
        void OnSeriesClick(SeriesClickEventArgs args)
        {
        }


        List<WebAdmin.Models.adminPanelProject.DataItem> data = new List<WebAdmin.Models.adminPanelProject.DataItem> {
            new WebAdmin.Models.adminPanelProject.DataItem
            {
                employeeID = "24Wa",
                Quarter = "CEOGeneralDirector",
                Revenue = 0.7*100
            },
                     new DataItem
            {
                employeeID = "24Wa",

                Quarter = "AdministrativeStaff",
                Revenue = 0.3*100
            },
                         new DataItem
            {
                employeeID = "24Wa",

                Quarter = "CreativeDesignManager",
                Revenue = 0.9*100
            },             
            // new DataItem
            // {
            //     Quarter = "FinanceStaff",
            //     Revenue = 0.55162
            // },             new DataItem
            // {
            //     Quarter = "FinanceManager",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "HRStaff",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "HRManager",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "ITStaff",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "ITManager",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "MarketingStaff",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "ProductStaff",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "ProductManager",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "SalesStaff",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "CustomerService",
            //     Revenue = 0.55162*100
            // },             new DataItem
            // {
            //     Quarter = "SalesManager",
            //     Revenue = 0.55162*100
            // },
         };

    }
}