using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Radzen;
using WebAdmin.Models.adminPanelProject;

namespace WebAdmin.Pages.User.Visualizations
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
        protected string employeeId2;

        protected string stringData;
        protected string companyId;

        protected string employeeIdForPosition;

        protected string JobTitle;

        protected string employeeIdDb;

        protected string parameterValue;
        protected double jobFitValue;
        protected string search = "";
        protected decimal minFRC;
        protected string userData;
        protected WebAdmin.Models.ApplicationUser user;

        [Parameter]
        public string Id { get; set; }
        [Inject]
        protected SecurityService Security { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationState { get; set; }

        protected override async Task OnInitializedAsync()
        {

            employees = await adminPanelProjectService.GetPlayers(
                new Query
                {
                    Filter =
                        $@"i => i.id.Contains(@0) || i.FullName.Contains(@0) || i.Email.Contains(@0) || i.Gender.Contains(@0) || i.createdBy.Contains(@0)",
                    FilterParameters = new object[] { search }
                }
            );

            companies = await adminPanelProjectService.GetUsers(

            );
            reports = await adminPanelProjectService.GetJobFitReports();
            var authState = await authenticationState;
            var userId = authState.User.Claims.FirstOrDefault().Value.ToString();

            user = await Security.GetUserById(userId);
            userData = user.CompanyId;

            employee = adminPanelProjectService.GetEmployeeByName(userData);
        }

        bool showDataLabels = false;
        int value;
        void OnSeriesClick(SeriesClickEventArgs args)
        {
        }
        bool hasuseridValue;

        [Parameter]
        public string userid { get; set; }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            employee = new WebAdmin.Models.adminPanelProject.Employee();

            hasuseridValue = parameters.TryGetValue<string>("employeeId2", out var hasuseridResult);

            if (hasuseridValue)
            {
                employee.CompanyId = hasuseridResult;
            }
            await base.SetParametersAsync(parameters);
        }

    }
}