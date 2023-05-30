using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using WebAdmin.Models.adminPanelProject;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Authorization;

namespace WebAdmin.Pages.Admin.SummaryDashboards
{
    public partial class AddSumm
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
        protected WebAdmin.Models.adminPanelProject.ZipEntry zip;


        protected string search = "";
        private const string DefaultStatus = "Choose a zip file";

        private List<ZipEntry> _entries;
        private string _fileName;

        private string _status = DefaultStatus;
        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForuserid;

        protected override async Task OnInitializedAsync()
        {
            usersForuserid = await adminPanelProjectService.GetUsers();
            var authState = await authenticationState;
            var userId = authState.User.Claims.FirstOrDefault().Value.ToString();

            user = await Security.GetUserById(userId);
            userData = user.CompanyId;
            // employee = adminPanelProjectService.GetPlayerByCompanyId(userData);

        }

        // private async Task HandleFileSelected(InputFileChangeEventArgs e)
        // {
        //     var file = e.File;
        //     if (file is not null)
        //     {
        //         var filePath = await adminPanelProjectService.SaveFile(file);
        //     }
        // }
        private List<string> zipEntries;

        private string name;
        private string currentUser;

        protected long maxFileSize = 100 * 1024 * 1024; // 100 MB
        [Inject]
        protected SecurityService Security { get; set; }
        protected WebAdmin.Models.ApplicationUser user;
        protected WebAdmin.Models.adminPanelProject.Employee employee;

        protected string userData;



        [CascadingParameter]
        private Task<AuthenticationState> authenticationState { get; set; }
        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {

            if (employee == null)
            {
                new Exception("not found");
            }

            var file = e.File;
            await using var stream = e.File.OpenReadStream(maxFileSize);

            _entries = await adminPanelProjectService.ExtractFiles(stream, file, userData);

            _fileName = e.File.Name;

            _status = DefaultStatus;

        }
        bool hasuseridValue;

        [Parameter]
        public string userid { get; set; }
        public WebAdmin.Models.ApplicationUser company;
        protected List<WebAdmin.Models.adminPanelProject.AssessmentReportEmployeeFile> assessmentReportFiles;

        protected WebAdmin.Models.adminPanelProject.Employee employeeByName;


        public override async Task SetParametersAsync(ParameterView parameters)
        {
            company = new WebAdmin.Models.ApplicationUser();

            hasuseridValue = parameters.TryGetValue<string>("userid", out var hasuseridResult);

            if (hasuseridValue)
            {
                company.CompanyId = hasuseridResult;
            }
            await base.SetParametersAsync(parameters);
        }
    }
}
