using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace WebAdmin.Pages.Company
{
    public partial class ApplicationUsers
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

        protected IEnumerable<WebAdmin.Models.ApplicationUser> users;
        protected RadzenDataGrid<WebAdmin.Models.ApplicationUser> grid0;
        protected string error;
        protected bool errorVisible;

        [Inject]
        protected SecurityService Security { get; set; }

        protected string search = "";

        protected async Task Search(ChangeEventArgs args)
        {
            search = $"{args.Value}";

            await grid0.GoToPage(0);

            users = await adminPanelProjectService.GetUsers(new Query { Filter = $@"i => i.username.Contains(@0) || i.email.Contains(@0) || i.companyname.Contains(@0)", FilterParameters = new object[] { search } });
        }
        protected override async Task OnInitializedAsync()
        {
            users = await Security.GetUsers();
        }

        protected async Task AddClick()
        {
            // await DialogService.OpenAsync<AddApplicationUser>("Add Application User");
            await DialogService.OpenAsync<AddApplicationUser>("Add Companies");
            users = await Security.GetUsers();
            await grid0.Reload();

        }

        protected async Task RowSelect(WebAdmin.Models.ApplicationUser user)
        {
            await DialogService.OpenAsync<EditApplicationUser>("Edit Company", new Dictionary<string, object>{ {"Id", user.Id} });

            users = await Security.GetUsers();
        }

        protected async Task DeleteClick(WebAdmin.Models.ApplicationUser user)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this user?") == true)
                {
                    await Security.DeleteUser($"{user.Id}");

                    users = await Security.GetUsers();
                }
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

         protected async Task ExportClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await adminPanelProjectService.ExportUsersToCSV(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
                }, "ApplicationUser");
            }

            if (args == null || args.Value == "xlsx")
            {
                await adminPanelProjectService.ExportUsersToExcel(new Query
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = string.Join(",", grid0.ColumnsCollection.Where(c => c.GetVisible() && !string.IsNullOrEmpty(c.Property)).Select(c => c.Property))
                }, "ApplicationUser");
            }
        }

        //   protected async Task AddButtonClick(MouseEventArgs args)
        // {
        //     await DialogService.OpenAsync<AddApplicationUser>("Add Company", null);
        //     await grid0.Reload();
        // }
    }
}