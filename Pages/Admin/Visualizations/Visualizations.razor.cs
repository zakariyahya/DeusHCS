using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using WebAdmin.Models.adminPanelProject;

namespace WebAdmin.Pages.Admin.Visualizations
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
        protected IEnumerable<WebAdmin.Models.ApplicationUser> companies;

        protected IEnumerable<WebAdmin.Models.adminPanelProject.Employee> employees2;

        protected WebAdmin.Models.adminPanelProject.Employee employee;
        protected WebAdmin.Models.adminPanelProject.JobFitReport report;

        protected IEnumerable<WebAdmin.Models.adminPanelProject.JobFitReport> reports;
        protected IEnumerable<WebAdmin.Models.adminPanelProject.TransposeData> transposeDatas;
        protected WebAdmin.Models.adminPanelProject.Assessment assessment;
        protected List<WebAdmin.Models.adminPanelProject.DataItem> data;


        protected List<WebAdmin.Models.adminPanelProject.TransposeData> transposeDataByIdList;
        protected WebAdmin.Models.adminPanelProject.TransposeData transposeDataById;



        protected List<WebAdmin.Models.adminPanelProject.DataItem> dataItems;
        protected WebAdmin.Models.adminPanelProject.DataItem dataItem;

        protected WebAdmin.Models.adminPanelProject.MergedObject mergedObject;



        protected string employeeId;
        protected string stringData;
        protected string companyId;

        protected string employeeIdForPosition;

        protected string JobTitle;

        protected string employeeIdDb;

        protected string parameterValue;
        protected double jobFitValue;
        protected string search = "";
        protected decimal minFRC;

        protected override async Task OnInitializedAsync()
        {
            employee = adminPanelProjectService.GetPlayerById(employeeId);
            // assessment = adminPanelProjectService.GetAssessmentByID(employeeId);
            employees = await adminPanelProjectService.GetPlayers(
                new Query
                {
                    Filter =
                        $@"i => i.id.Contains(@0) || i.FullName.Contains(@0) || i.Email.Contains(@0) || i.Gender.Contains(@0) || i.createdBy.Contains(@0)",
                    FilterParameters = new object[] { search }
                }
            );
    

            companies = await adminPanelProjectService.GetUsers(
                // new Query
                // {
                //     Filter =
                //         $@"i => i.id.Contains(@0) || i.FullName.Contains(@0) || i.Email.Contains(@0) || i.Gender.Contains(@0) || i.createdBy.Contains(@0)",
                //     FilterParameters = new object[] { search }
                // }
            );
            reports = await adminPanelProjectService.GetJobFitReports();
            // report =  adminPanelProjectService.GetJobFitReportById(employeeId);

            // transposeDatas = await adminPanelProjectService.GetTransposeDatas(
            //      new Query
            //      {
            //          Filter =
            //             $@"i => i.ColumnName.Contains(@0) || i.ColumnValue.Contains(@0)",
            //          FilterParameters = new object[] { search }
            //      }
            // );
   
            // transposeDataByIdList = adminPanelProjectService.GetTransposeDataById(@"employeeId");

            // mergedObject = new MergedObject
            // {
            //     ColumnName = transposeDataByIdList.Select(o => o.ColumnName).FirstOrDefault(),
            //     ColumnValue = transposeDataByIdList.Select(o => o.ColumnValue).FirstOrDefault(),
            // };


            // foreach (var item in reports.Where(x=>x.EmployeeId == employeeId))
            // {
            //     data = new List<WebAdmin.Models.adminPanelProject.DataItem> {
            //         new WebAdmin.Models.adminPanelProject.DataItem
            //         {
            //             employeeID =  item?.EmployeeId,
            //             Quarter = "CEOGeneralDirector",
            //             Revenue = item?.CEOGeneralDirector
            //         },
            //     };
            // }
        
                        
                
                // };

            
            

        }

        bool showDataLabels = false;
        int value;
        void OnSeriesClick(SeriesClickEventArgs args)
        {
        }

    }
}