using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

using AdminPanel.Data;

namespace WebAdmin.Controllers
{
    public partial class ExportadminPanelProjectController : ExportController
    {
        private readonly ApplicationIdentityDbContext context;
        private readonly adminPanelProjectService service;

        public ExportadminPanelProjectController(ApplicationIdentityDbContext context, adminPanelProjectService service)
        {
            this.service = service;
            this.context = context;
        }

        [HttpGet("/export/adminPanelProject/jobfitreports/csv")]
        [HttpGet("/export/adminPanelProject/jobfitreports/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobFitReportsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetJobFitReports(), Request.Query), fileName);
        }

        [HttpGet("/export/adminPanelProject/jobfitreports/excel")]
        [HttpGet("/export/adminPanelProject/jobfitreports/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportJobFitReportsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetJobFitReports(), Request.Query), fileName);
        }

        [HttpGet("/export/adminPanelProject/assessments/csv")]
        [HttpGet("/export/adminPanelProject/assessments/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAssessmentsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetAssessments(), Request.Query), fileName);
        }

        [HttpGet("/export/adminPanelProject/assessments/excel")]
        [HttpGet("/export/adminPanelProject/assessments/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportAssessmentsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetAssessments(), Request.Query), fileName);
        }

        [HttpGet("/export/adminPanelProject/employees/csv")]
        [HttpGet("/export/adminPanelProject/employees/csv(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmployeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(await service.GetEmployees(), Request.Query), fileName);
        }

        [HttpGet("/export/adminPanelProject/employees/excel")]
        [HttpGet("/export/adminPanelProject/employees/excel(fileName='{fileName}')")]
        public async Task<FileStreamResult> ExportEmployeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(await service.GetEmployees(), Request.Query), fileName);
        }
    }
}
