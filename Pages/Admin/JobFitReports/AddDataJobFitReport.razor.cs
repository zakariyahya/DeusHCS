// using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.JSInterop;
// using Microsoft.AspNetCore.Components;
// using Microsoft.AspNetCore.Components.Web;
// using Radzen;
// using Radzen.Blazor;
// using BlazorInputFile;

// namespace WebAdmin.Pages.Admin.JobFitReports
// {
//     public partial class AddDataJobFitReport
//     {
//         [Inject]
//         protected IJSRuntime JSRuntime { get; set; }

//         [Inject]
//         protected NavigationManager NavigationManager { get; set; }

//         [Inject]
//         protected DialogService DialogService { get; set; }

//         [Inject]
//         protected TooltipService TooltipService { get; set; }

//         [Inject]
//         protected ContextMenuService ContextMenuService { get; set; }

//         [Inject]
//         protected NotificationService NotificationService { get; set; }

//         [Inject]
//         public adminPanelProjectService adminPanelProjectService { get; set; }
//         protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForUserId;
//         protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.DataForJobFitReport> grid0;

//         protected override async Task OnInitializedAsync()
//         {
//             usersForUserId = await adminPanelProjectService.GetUsers();
//             // jobFitReports = new WebAdmin.Models.adminPanelProject.JobFitReport();
//         }
//         List<WebAdmin.Models.adminPanelProject.DataItem> dataItems =
//            new List<WebAdmin.Models.adminPanelProject.DataItem>();
//         protected bool isBusy;

//         protected bool errorVisible;
//         protected WebAdmin.Models.adminPanelProject.DataForJobFitReport jobFitReport;

//         IFileListEntry file;
//         protected IEnumerable<WebAdmin.Models.adminPanelProject.JobFitReport> jobFitReports;


//         WebAdmin.Models.adminPanelProject.DataForJobFitReport jobFitReportDb =
//             new WebAdmin.Models.adminPanelProject.DataForJobFitReport();

//         async Task HandleFileSelected(IFileListEntry[] files)
//         {
//             file = files.FirstOrDefault();
//             if (file != null)
//             {
//                 adminPanelProjectService.Upload(file);
//                 // jobFitReports = adminPanelProjectService?.ReadJobFitReportExcel(file);
//             }
//         }

//         protected async Task FormSubmit()
//         {
//             try
//             {
//                 // foreach (var item in jobFitReports)
//                 // {
//                 //     isBusy = true;
//                 //     item.UserId = jobFitReport.UserId;
//                 //     adminPanelProjectService.CreateJobFitReport(item);
//                 // }
//                 DialogService.Close(jobFitReport);
//             }
//             catch (Exception ex)
//             {
//                 errorVisible = true;
//             }
//             // foreach (var item in jobFitReports)
//             // {
//             //     isBusy = true;
//             //     item.UserId = jobFitReport.UserId;

//             //     adminPanelProjectService.CreateJobFitReport(item);

//             //     // foreach (var dataItem in dataItems)
//             //     // {
//             //     //     dataItem.employeeID = item.EmployeeId;
//             //     //     dataItem.Quarter.Add(((char)item.HRManager));
//             //     //     dataItem.Revenue.Add(item.HRManager);
//             //     //     dataItem.Revenue.Add(item.CEOGeneralDirector);

//             //     // }
//             // }
//             // DialogService.Close(jobFitReport);
//         }

//         protected string info;
//         protected bool infoVisible;

//         protected async Task CancelButtonClick(MouseEventArgs args)
//         {
//             DialogService.Close(null);
//         }

//         protected async Task CancelClick()
//         {
//             DialogService.Close(null);
//         }

//         bool hasUseridValue;

//         [Parameter]
//         public string UserId { get; set; }

//         public override async Task SetParametersAsync(ParameterView parameters)
//         {
//             jobFitReport = new WebAdmin.Models.adminPanelProject.DataForJobFitReport();
//             hasUseridValue = parameters.TryGetValue<string>("UserId", out var hasUseridResult);
//             await base.SetParametersAsync(parameters);
//         }
//     }
// }
