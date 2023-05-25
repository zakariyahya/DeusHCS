using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using WebAdmin.Models.adminPanelProject;
using Microsoft.AspNetCore.Components.Forms;
using System.IO.Compression;
using System;

namespace WebAdmin.Pages.SummaryDashboards
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

        [Inject]
        public adminPanelProjectService adminPanelProjectService { get; set; }
        private readonly IWebHostEnvironment environment;


        protected IEnumerable<WebAdmin.Models.adminPanelProject.JobFitReport> jobFitReports;
        protected WebAdmin.Models.adminPanelProject.JobFitReport jobFitReport;


        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.JobFitReport> grid0;

        protected string search = "";
        private const string DefaultStatus = "Choose a zip file";

        private List<ZipEntry> _entries;
        private string _fileName;

        private string _status = DefaultStatus;
        private async Task HandleFileSelected(InputFileChangeEventArgs e)
        {
            var file = e.File;
            if (file is not null)
            {
                var filePath = await adminPanelProjectService.SaveFile(file);
                // Perform further processing if needed
            }
        }
        public string SaveDirectory = "wwwroot/uploads"; // Change this path according to your requirements

        private List<string> zipEntries;

        private async Task ReadZipFile()
        {
            string FilePath = Path.Combine(
                environment.ContentRootPath,
                "wwwroot/uploads/"
            );
            // var zipFilePath = FilePath; // Replace with the path to your uploaded ZIP file

            using (var fileStream = await adminPanelProjectService.GetFileStream(FilePath))
            using (var zipArchive = new ZipArchive(fileStream, ZipArchiveMode.Read))
            {
                zipEntries = zipArchive.Entries.Select(entry => entry.FullName).ToList();
            }
        }
        private async Task OnInputFileChange(InputFileChangeEventArgs e)
        {
            await using var stream = e.File.OpenReadStream();

            _entries = await adminPanelProjectService.ExtractFiles(stream);
            _fileName = e.File.Name;

            _status = DefaultStatus;
        }

        bool hasUseridValue;
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            jobFitReport = new WebAdmin.Models.adminPanelProject.JobFitReport();
            hasUseridValue = parameters.TryGetValue<string>("employeeId", out var hasUseridResult);
            await base.SetParametersAsync(parameters);
        }

    }


    // protected async Task Search(ChangeEventArgs args)
    // {
    //     search = $"{args.Value}";

    //     await grid0.GoToPage(0);

    //     jobFitReports = await adminPanelProjectService.GetJobFitReports(new Query { Filter = $@"i => i.id.Contains(@0) || i.UserId.Contains(@0) || i.EmployeeId.Contains(@0)", FilterParameters = new object[] { search } });
    // }
    // protected override async Task OnInitializedAsync()
    // {
    //     jobFitReports = await adminPanelProjectService.GetJobFitReports(new Query { Filter = $@"i => i.id.Contains(@0) || i.UserId.Contains(@0) || i.EmployeeId.Contains(@0)", FilterParameters = new object[] { search } });
    // }


}
