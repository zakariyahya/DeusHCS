using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebAdmin.Pages.Employees
{
    public partial class Players
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

        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.Employee> grid0;
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
            
            infoVisible = !string.IsNullOrEmpty(info);
            usersForUserId = await adminPanelProjectService.GetUsers();
        }

        protected async Task AddButtonClick(MouseEventArgs args)
        {
            await DialogService.OpenAsync<AddPlayer>("Add Employees", null);
            await grid0.Reload();
        }

        protected async Task EditRow(
            DataGridRowMouseEventArgs<WebAdmin.Models.adminPanelProject.Employee> args
        )
        {
            await DialogService.OpenAsync<EditPlayer>(
                "Edit Employees",
                new Dictionary<string, object> { { "id", args.Data.id } }
            );
        }

        protected async Task GridDeleteButtonClick(
            MouseEventArgs args,
            WebAdmin.Models.adminPanelProject.Employee employee
        )
        {
            try
            {
                if (
                    await DialogService.Confirm("Are you sure you want to delete this record?")
                    == true
                )
                {
                    // var deleteResult = await adminPanelProjectService.DeleteCompany(player.id);
                    var deleteResult = await adminPanelProjectService.DeletePlayer(employee.id);

                    if (deleteResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (Exception ex)
            {
                NotificationService.Notify(
                    new NotificationMessage
                    {
                        Severity = NotificationSeverity.Error,
                        Summary = $"Error",
                        Detail = $"Unable to delete Player"
                    }
                );
            }
        }

        protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await adminPanelProjectService.ExportPlayersToCSV(
                    new Query
                    {
                        Filter =
                            $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                        OrderBy = $"{grid0.Query.OrderBy}",
                        Expand = "",
                        Select = string.Join(
                            ",",
                            grid0.ColumnsCollection
                                .Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property))
                                .Select(c => c.Property)
                        )
                    },
                    "Players"
                );
            }

            if (args == null || args.Value == "xlsx")
            {
                await adminPanelProjectService.ExportPlayersToExcel(
                    new Query
                    {
                        Filter =
                            $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                        OrderBy = $"{grid0.Query.OrderBy}",
                        Expand = "",
                        Select = string.Join(
                            ",",
                            grid0.ColumnsCollection
                                .Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property))
                                .Select(c => c.Property)
                        )
                    },
                    "Players"
                );
            }
        }

        protected WebAdmin.Models.adminPanelProject.Employee employee;

        string lastFilter;

        [Inject]
        protected SecurityService Security { get; set; }

        protected async void Grid0Render(
            DataGridRenderEventArgs<WebAdmin.Models.adminPanelProject.Employee> args
        )
        {
            if (grid0.Query.Filter != lastFilter)
            {
                employee = grid0.View.FirstOrDefault();
            }

            if (grid0.Query.Filter != lastFilter)
            {
                await grid0.SelectRow(employee);
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
