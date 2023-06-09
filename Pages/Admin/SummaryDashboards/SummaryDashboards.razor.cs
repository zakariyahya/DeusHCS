using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebAdmin.Pages.Admin.SummaryDashboards
{
    public partial class SummaryDashboards
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
        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForUserId;

        [Inject]
        public adminPanelProjectService adminPanelProjectService { get; set; }

        protected IEnumerable<WebAdmin.Models.adminPanelProject.Employee> employees;

        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.AssessmentReportEmployeeFile> grid0;
        protected string search = "";
        protected string info;
        protected bool infoVisible;
        string userid;
        bool frozen;
        IList<string> values = new string[] { };

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            employees = await adminPanelProjectService.GetPlayers(
                new Query
                {
                    Filter =
                        $@"i => i.id.Contains(@0) || i.FullName.Contains(@0) || i.Email.Contains(@0) || i.Gender.Contains(@0) || i.createdBy.Contains(@0)",
                    FilterParameters = new object[] { search }
                }
            );
        }
        private string cwdPath;
        private string FilePath;
        protected WebAdmin.Models.ApplicationUser user;

        private readonly IWebHostEnvironment environment;

        protected override async Task OnInitializedAsync()
        {
            user = await Security.GetUserById(Security.User.Id);

            // employees = await adminPanelProjectService.GetPlayers(
            //     new Query
            //     {
            //         Filter =
            //             $@"i => i.id.Contains(@0) || i.FullName.Contains(@0) || i.Email.Contains(@0) || i.Gender.Contains(@0) || i.createdBy.Contains(@0)",
            //         FilterParameters = new object[] { search }
            //     }
            // );
            reportFiles = await adminPanelProjectService.GetReports();

            infoVisible = !string.IsNullOrEmpty(info);
            usersForUserId = await adminPanelProjectService.GetUsers();
            cwdPath = Environment.CurrentDirectory;
            FilePath =
                Path.Combine(
                    Environment.CurrentDirectory+"uploads"
            );
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddSumm>("Add SummaryDashboard", null);
            await grid0.Reload();
        }





        protected WebAdmin.Models.adminPanelProject.Employee employee;
        protected IEnumerable<WebAdmin.Models.adminPanelProject.AssessmentReportEmployeeFile> reportFiles;
        protected WebAdmin.Models.adminPanelProject.AssessmentReportEmployeeFile reportFile;



        string lastFilter;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async void Grid0Render(
            DataGridRenderEventArgs<WebAdmin.Models.adminPanelProject.AssessmentReportEmployeeFile> args
        )
        {
            if (grid0.Query.Filter != lastFilter)
            {
                reportFile = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter)
            {
                await grid0.SelectRow(reportFile);
            }

            lastFilter = grid0.Query.Filter;
        }

        bool hasUseridValue;

        public override async Task SetParametersAsync(ParameterView parameters)
        {
            employee = new WebAdmin.Models.adminPanelProject.Employee();

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

        void LoadData(LoadDataArgs args)
        {
            var query = adminPanelProjectService.GetPlayers(
                new Query
                {
                    Filter =
                            $@"i => i.id.Contains(@0) || i.FullName.Contains(@0) || i.Email.Contains(@0) || i.Gender.Contains(@0) || i.createdBy.Contains(@0)",
                    FilterParameters = new object[] { search }
                }
            );

            // if (!string.IsNullOrEmpty(args.Filter))
            // {
            //     query = query.Where(c => c.CustomerID.ToLower().Contains(args.Filter.ToLower()) || c.ContactName.ToLower().Contains(args.Filter.ToLower()));
            // }

            // employees = query();

            // InvokeAsync(StateHasChanged);
        }
    }
}
