using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using BlazorInputFile;
using WebAdmin.Models.adminPanelProject;

namespace WebAdmin.Pages.Assessments
{
    public partial class AddAssessment
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
        protected IEnumerable<WebAdmin.Models.ApplicationUser> usersForUserId;
        bool hasUseridValue;
        protected RadzenDataGrid<WebAdmin.Models.adminPanelProject.Assessment> grid0;

        protected override async Task OnInitializedAsync()
        {
            assessment = new WebAdmin.Models.adminPanelProject.Assessment();
            usersForUserId = await adminPanelProjectService.GetUsers();

        }
        protected bool errorVisible;
        protected string error;
        Assessment assessmentDb = new Assessment();
        List<Assessment> assessments = new List<Assessment>();
        protected WebAdmin.Models.adminPanelProject.Assessment assessment;
        [Parameter]
        public string UserId { get; set; }
        protected bool isBusy;

        IFileListEntry file;
        protected bool errorExtension;
        protected string errorExt;
        protected string info;
        protected bool infoVisible;
        async Task HandleFileSelected(IFileListEntry[] files)
        {
            file = files.FirstOrDefault();
            if (file != null)
            {
                // if (file.Type != "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                // {
                //     errorExtension = true;
                //     errorExt = "Upload file excel only";
                // }
                adminPanelProjectService.Upload(file);
                assessments = adminPanelProjectService.ReadAssessmentExcel();
                foreach (var item in assessments)
                {
                    // var company = adminPanelProjectService.GetUserById(item.id);
                    assessmentDb = assessment;
                }
            }
        }
        protected async Task FormSubmit()
        {
            try
            {
                isBusy = true;
                await adminPanelProjectService.CreateAssessment(assessment);
                assessment = new WebAdmin.Models.adminPanelProject.Assessment();

                // user = adminPanelProjectService.GetUserById(UserId);
                assessment.UserId = UserId;
                assessment.EmployeeId = UserId;
                DialogService.Close(assessment);
            }
            catch (Exception ex)
            {
                errorVisible = true;
                error = ex.Message;
            }
        }

        private void AddExcel()
        {
            try
            {
                adminPanelProjectService.AddAssessmentExcel(assessments);
                // Message = "Data Saved";
                infoVisible = true;

                info = "Companies was created";
            }
            catch (System.Exception ex)
            {
                errorVisible = true;
                // Message = ex.Message;
            }

        }
        protected async Task CancelButtonClick(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
        protected async Task CancelClick()
        {
            DialogService.Close(null);
        }
        public override async Task SetParametersAsync(ParameterView parameters)
        {
            assessment = new WebAdmin.Models.adminPanelProject.Assessment();

            hasUseridValue = parameters.TryGetValue<string>("EmployeeId", out var hasUseridResult);
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
    }
}