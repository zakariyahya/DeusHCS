using System;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;

using BlazorInputFile;
using AdminPanel.Data;
using Microsoft.AspNetCore.Identity;
using WebAdmin.Models;
using OfficeOpenXml;
using WebAdmin.Models.adminPanelProject;
using Microsoft.EntityFrameworkCore.Metadata;
using System.IO.Compression;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;

namespace WebAdmin
{
    public partial class adminPanelProjectService
    {
        ApplicationIdentityDbContext Context
        {
            get { return this.context; }
        }
        private readonly ApplicationIdentityDbContext context;
        private readonly SecurityService securityService;
        private readonly NavigationManager navigationManager;
        private readonly IWebHostEnvironment environment;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration;
        private readonly ContextHelper contextHelper;

        public adminPanelProjectService(
            ApplicationIdentityDbContext context,
            NavigationManager navigationManager,
            IWebHostEnvironment environment,
            SecurityService securityService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration,
            ContextHelper contextHelper
        )
        {
            this.context = context;
            this.navigationManager = navigationManager;
            this.environment = environment;
            this.securityService = securityService;
            this.userManager = userManager;
            this.configuration = configuration;
            this.contextHelper = contextHelper;
        }

        public void Reset() =>
            Context.ChangeTracker
                .Entries()
                .Where(e => e.Entity != null)
                .ToList()
                .ForEach(e => e.State = EntityState.Detached);

        public async Task ExportJobFitReportsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/jobfitreports/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/jobfitreports/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        public async Task Upload(IFileListEntry file)
        {
            string path = Path.Combine();
            if (file.Name == "dataCompany.xlsx")
            {
                path = Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Companies",
                    file.Name
                );

            }
            if (file.Name == "dataAssessment.xlsx")
            {
                path = Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Assessments",
                    file.Name
                );
            }
            if (file.Name == "dataEmployee.xlsx")
            {
                path = Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Employees",
                    file.Name
                );
            }
            if (file.Name == "dataReport.xlsx")
            {
                path = Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Reports",
                    file.Name
                );
            }
            var memoryStream = new MemoryStream();
            await file.Data.CopyToAsync(memoryStream);
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(fileStream);
            }
            ReadExcel(file);

            // ReadExcel();
        }

        // public async Task UploadEmployee(IFileListEntry file)
        // {
        //     var path = Path.Combine(environment.ContentRootPath, "wwwroot/Data/Employee", file.Name);
        //     var memoryStream = new MemoryStream();
        //     await file.Data.CopyToAsync(memoryStream);
        //     using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
        //     {
        //         memoryStream.WriteTo(fileStream);
        //     }
        //     // ReadExcel();
        // }
        public List<ApplicationUser> ReadExcel(IFileListEntry file)
        {
            // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";
            // string FilePath = Path.Combine(environment.WebRootPath, "wwwroot/dataCompany.xlsx");

            string FilePath =
                Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Companies", file.Name
                );
            // string FilePath = "D:/Data/data.xlsx";

            System.IO.FileInfo existingFile = new System.IO.FileInfo(FilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                int rolCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 2; row <= rowCount; row++)
                {
                    ApplicationUser company = new ApplicationUser();
                    for (int col = 1; col <= rolCount; col++)
                    {
                        if (col == 1)
                            company.CompanyId = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 2)
                            company.CompanyName = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 3)
                            company.Email = worksheet.Cells[row, col].Value.ToString();
                    }
                    companies.Add(company);
                }
            }
            return companies;
        }

        public List<JobFitReport> ReadJobFitReportExcel(IFileListEntry file)
        {
            // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/*.xlsx";
            // string FilePath = Path.Combine(environment.WebRootPath, "wwwroot/dataCompany.xlsx");

            string FilePath = Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Reports",
                    file.Name
                );
            System.IO.FileInfo existingFile = new System.IO.FileInfo(FilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                int rolCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 3; row <= rowCount; row++)
                {
                    JobFitReport jbr = new JobFitReport();
                    for (int col = 1; col <= rolCount; col++)
                    {
                        if (col == 2)
                            jbr.EmployeeId = worksheet.Cells[row, col].Value.ToString();

                        else if (col == 3)
                            jbr.Bio =
                                worksheet.Cells[row, col].Value.ToString()
                            ;
                        else if (col == 4)
                            jbr.Insight =
                                worksheet.Cells[row, col].Value.ToString()
                            ;
                        else if (col == 5)
                            jbr.CEOGeneralDirector =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 6)
                            jbr.AdministrativeStaff =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 7)
                            jbr.CreativeDesignManager =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 8)
                            jbr.FinanceStaff =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 9)
                            jbr.FinanceManager =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 10)
                            jbr.HRStaff =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 9)
                            jbr.HRManager =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 10)
                            jbr.ITStaff =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 11)
                            jbr.ITManager =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 12)
                            jbr.MarketingStaff =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 13)
                            jbr.ProductStaff =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 14)
                            jbr.ProductManager =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 15)
                            jbr.SalesStaff =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 16)
                            jbr.CustomerService =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                        else if (col == 17)
                            jbr.SalesManager =
                                Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString())
                            ;
                    }
                    jbrs.Add(jbr);
                }
            }
            return jbrs;
        }

        List<ApplicationUser> companies = new List<ApplicationUser>();



        List<JobFitReport> jbrs = new List<JobFitReport>();

        List<Employee> employees = new List<Employee>();

        public List<Employee> ReadEmployeeExcel(IFileListEntry file)
        {
            // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";

            string FilePath = Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Employees", file.Name
                );
            System.IO.FileInfo existingFile = new System.IO.FileInfo(FilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                int rolCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 3; row <= rowCount; row++)
                {
                    Employee employee = new Employee();
                    for (int col = 1; col <= rolCount; col++)
                    {
                        if (col == 1)
                            employee.EmployeeId = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 2)
                            employee.Email = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 3)
                            employee.FullName = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 4)
                            employee.Gender = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 5)
                            employee.BirthDate = Convert.ToDateTime(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 6)
                            employee.Age = Convert.ToInt32(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 7)
                            employee.AgeGroup = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 8)
                            employee.JobTitle = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 9)
                            employee.JobLevel = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 10)
                            employee.Departement = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 11)
                            employee.YearHired = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 12)
                            employee.City = worksheet.Cells[row, col].Value.ToString();
                    }
                    employees.Add(employee);
                }
            }
            return employees;
        }
        List<Assessment> assessments = new List<Assessment>();
        public List<Assessment> ReadAssessmentExcel(IFileListEntry file)
        {
            // string FilePath =
            //    "C:/Users/yahya/Documents/Project/Data/Assessments/dataAssessment.xlsx";
            string FilePath = Path.Combine(
                    environment.ContentRootPath,
                    "wwwroot/Data/Assessments", file.Name
                );
            System.IO.FileInfo existingFile = new System.IO.FileInfo(FilePath);
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            using (ExcelPackage package = new ExcelPackage(existingFile))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                int rolCount = worksheet.Dimension.End.Column;
                int rowCount = worksheet.Dimension.End.Row;

                for (int row = 3; row <= rowCount; row++)
                {
                    Assessment assessment = new Assessment();
                    for (int col = 1; col <= rolCount; col++)
                    {
                        if (col == 1)
                            assessment.EmployeeId = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 2)
                            assessment.Diplomatic = Convert.ToDecimal(worksheet.Cells[row, col].Value.ToString());

                        else if (col == 3)
                            assessment.Empathy = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 4)
                            assessment.Cooperative = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 5)
                            assessment.Trust = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 6)
                            assessment.Ambitious = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 7)
                            assessment.Competitive = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 8)
                            assessment.Direct = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 9)
                            assessment.Rational = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 10)
                            assessment.Conformity = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 11)
                            assessment.Diligent = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 12)
                            assessment.Organized = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 13)
                            assessment.Temperance = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 14)
                            assessment.Versatile = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 15)
                            assessment.Instinctive = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 16)
                            assessment.Opportunistic = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 17)
                            assessment.Unorthodox = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 18)
                            assessment.Adventurous = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 19)
                            assessment.Assertive = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 20)
                            assessment.Optimistic = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 21)
                            assessment.Gregarious = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 22)
                            assessment.Independent = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 23)
                            assessment.Introspective = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 24)
                            assessment.Restrained = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 25)
                            assessment.Ingenuity = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 26)
                            assessment.Inquisitive = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 27)
                            assessment.Intricate = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 28)
                            assessment.StrategicallyMinded = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 29)
                            assessment.Methodical = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 30)
                            assessment.PresentMinded = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 31)
                            assessment.Utilitarian = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 32)
                            assessment.Relationship_Driven = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 33)
                            assessment.ResultsDriven = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 34)
                            assessment.MotivatedByOrder = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 35)
                            assessment.MotivatedByAmbiguity = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 36)
                            assessment.Outgoing = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 37)
                            assessment.Reserved = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 38)
                            assessment.Innovative = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 39)
                            assessment.Pragmatic = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 40)
                            assessment.VisualPerception = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 41)
                            assessment.SpatialReasoning = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 42)
                            assessment.VerbalComprehension = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 43)
                            assessment.DominantLeadership = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 44)
                            assessment.CollaborativeLeadership = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 45)
                            assessment.ServantLeadership = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 46)
                            assessment.SupportingOthers = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 47)
                            assessment.CoachingOthers = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 48)
                            assessment.EnablingTeamwork = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 49)
                            assessment.BuildingConnections = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 50)
                            assessment.PersuadingOthers = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 51)
                            assessment.AdaptiveCommunication = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 52)
                            assessment.TrendIdentification = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 53)
                            assessment.DistillingInformation = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 54)
                            assessment.ApplyingExpertise = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 55)
                            assessment.ActiveLearning = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 56)
                            assessment.StrategicAgility = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 57)
                            assessment.ChampioningChange = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 58)
                            assessment.ForwardPlanning = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 59)
                            assessment.FollowingDirections = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 60)
                            assessment.QualityClientDelivery = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 61)
                            assessment.ResilienceUnderPressure = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 62)
                            assessment.LateralProblemSolving = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 63)
                            assessment.OperationalFlexibility = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 64)
                            assessment.SelfDevelopmentFocused = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 65)
                            assessment.OperationalFlexibility = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 66)
                            assessment.BusinessAcumen = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 67)
                            assessment.OptimizingPerformance = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 68)
                            assessment.LeadingAndDeciding = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 69)
                            assessment.SupportingAndCooperating = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 70)
                            assessment.InteractingAndNegotiating = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 71)
                            assessment.AnalyzeAndInterpret = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 72)
                            assessment.CreateAndConceptualize = Convert.ToDecimal(worksheet.Cells[
                                row,
                                col
                            ].Value.ToString());
                        else if (col == 73)
                            assessment.OrganizeAndExecute = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 74)
                            assessment.AdaptAndCope = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 75)
                            assessment.EnterprisingAndPerforming = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                    }
                    assessments.Add(assessment);
                }
            }

            return assessments;
        }

        public List<ApplicationUser> AddExcel(List<ApplicationUser> companies)
        {
            // var companies = ReadExcel();
            foreach (var item in companies)
            {
                var password = GenerateRandomPassword();

                item.Password = password;
                securityService.Register(
                    item.Email,
                    item.Password,
                    item.CompanyName,
                    item.CompanyId
                );
            }

            return companies;
        }

        // public List<JobFitReport> AddJobFitReportExcel(List<JobFitReport> jbs)
        // {
        //     // var companies = ReadExcel();
        //     foreach (var item in jbs)
        //     {
        //         CreateJobFitReport(item);
        //     }

        //     return jbrs;
        // }

        public List<Assessment> AddAssessmentExcel(List<Assessment> assessments)
        {
            // var companies = ReadExcel();
            foreach (var item in assessments)
            {
                CreateAssessment(item);
            }

            return assessments;
        }

        private static string GenerateRandomPassword()
        {
            var options = new PasswordOptions
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            var randomChars = new[]
            {
                "ABCDEFGHJKLMNOPQRSTUVWXYZ",
                "abcdefghijkmnopqrstuvwxyz",
                "0123456789",
                "!@$?_-"
            };

            var rand = new Random(Environment.TickCount);
            var chars = new List<char>();

            if (options.RequireUppercase)
            {
                chars.Insert(
                    rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]
                );
            }

            if (options.RequireLowercase)
            {
                chars.Insert(
                    rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]
                );
            }

            if (options.RequireDigit)
            {
                chars.Insert(
                    rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]
                );
            }

            if (options.RequireNonAlphanumeric)
            {
                chars.Insert(
                    rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]
                );
            }

            for (
                int i = chars.Count;
                i < options.RequiredLength
                    || chars.Distinct().Count() < options.RequiredUniqueChars;
                i++
            )
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count), rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        partial void OnUsersRead(ref IQueryable<WebAdmin.Models.ApplicationUser> userItems);
        public async Task<List<WebAdmin.Models.ApplicationUser>> GetAllUsers()
        {
            return context.Users.ToList();
        }
        public async Task<IQueryable<WebAdmin.Models.ApplicationUser>> GetUsers(Query query = null)
        {
            var items = Context.Users.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnUsersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPlayerCreated(WebAdmin.Models.adminPanelProject.Employee item);

        partial void OnAfterPlayerCreated(WebAdmin.Models.adminPanelProject.Employee item);

        public async Task<WebAdmin.Models.adminPanelProject.Employee> CreatePlayer(
            WebAdmin.Models.adminPanelProject.Employee employee
        )
        {
            OnPlayerCreated(employee);

            var existingItem = Context.Employees.Where(i => i.id == employee.id).FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Employees.Add(employee);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(employee).State = EntityState.Detached;
                throw;
            }

            OnAfterPlayerCreated(employee);

            return employee;
        }

        public async Task ExportJobFitReportsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/jobfitreports/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/jobfitreports/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        partial void OnJobFitReportsRead(
            ref IQueryable<WebAdmin.Models.adminPanelProject.JobFitReport> items
        );

        public async Task<
            IQueryable<WebAdmin.Models.adminPanelProject.JobFitReport>
        > GetJobFitReports(Query query = null)
        {
            var items = Context.JobFitReports.AsQueryable();


            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnJobFitReportsRead(ref items);

            foreach (var item in items)
            {
                // String.Format("Value: {0:P2}.", 0.8526);
                item.AdministrativeStaff = Math.Round(item.AdministrativeStaff * 100, 2);
                item.CEOGeneralDirector = Math.Round(item.CEOGeneralDirector * 100, 2);
                item.CreativeDesignManager = Math.Round(item.CreativeDesignManager * 100, 2);
                item.CustomerService = Math.Round(item.CustomerService * 100, 2);
                item.FinanceManager = Math.Round(item.FinanceManager * 100, 2);
                item.FinanceStaff = Math.Round(item.FinanceStaff * 100, 2);
                item.HRManager = Math.Round(item.HRManager * 100, 2);
                item.HRStaff = Math.Round(item.HRStaff * 100, 2);
                item.ITManager = Math.Round(item.ITManager * 100, 2);
                item.ITStaff = Math.Round(item.ITStaff * 100, 2);
                item.MarketingStaff = Math.Round(item.MarketingStaff * 100, 2);
                item.ProductManager = Math.Round(item.ProductManager * 100, 2);
                item.SalesManager = Math.Round(item.SalesManager * 100, 2);
                item.SalesStaff = Math.Round(item.SalesStaff * 100, 2);
                item.ProductStaff = Math.Round(item.ProductStaff * 100, 2);
            }

            return await Task.FromResult(items);
        }

        partial void OnJobFitReportGet(WebAdmin.Models.adminPanelProject.JobFitReport item);

        public WebAdmin.Models.adminPanelProject.JobFitReport GetJobFitReportById(
            string id
        )
        {
            var items = Context.JobFitReports.AsNoTracking().Where(i => i.EmployeeId == id);

            var itemToReturn = items.FirstOrDefault();

            OnJobFitReportGet(itemToReturn);

            return itemToReturn;
        }

        partial void OnJobFitReportCreated(WebAdmin.Models.adminPanelProject.JobFitReport item);

        partial void OnAfterJobFitReportCreated(
            WebAdmin.Models.adminPanelProject.JobFitReport item
        );

        public async Task<WebAdmin.Models.adminPanelProject.JobFitReport> CreateJobFitReport(
            WebAdmin.Models.adminPanelProject.JobFitReport jobfitreport
        )
        {
            OnJobFitReportCreated(jobfitreport);

            var existingItem = Context.JobFitReports
                .Where(i => i.id == jobfitreport.id)
                .FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.JobFitReports.Add(jobfitreport);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(jobfitreport).State = EntityState.Detached;
                throw;
            }

            OnAfterJobFitReportCreated(jobfitreport);

            return jobfitreport;
        }

        public async Task<WebAdmin.Models.adminPanelProject.JobFitReport> CancelJobFitReportChanges(
            WebAdmin.Models.adminPanelProject.JobFitReport item
        )
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnJobFitReportUpdated(WebAdmin.Models.adminPanelProject.JobFitReport item);

        partial void OnAfterJobFitReportUpdated(
            WebAdmin.Models.adminPanelProject.JobFitReport item
        );

        public async Task<WebAdmin.Models.adminPanelProject.JobFitReport> UpdateJobFitReport(
            string id,
            WebAdmin.Models.adminPanelProject.JobFitReport jobfitreport
        )
        {
            OnJobFitReportUpdated(jobfitreport);

            var itemToUpdate = Context.JobFitReports
                .Where(i => i.id == jobfitreport.id)
                .FirstOrDefault();

            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(jobfitreport);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterJobFitReportUpdated(jobfitreport);

            return jobfitreport;
        }

        partial void OnJobFitReportDeleted(WebAdmin.Models.adminPanelProject.JobFitReport item);

        partial void OnAfterJobFitReportDeleted(
            WebAdmin.Models.adminPanelProject.JobFitReport item
        );

        public async Task<WebAdmin.Models.adminPanelProject.JobFitReport> DeleteJobFitReport(
            string id
        )
        {
            var itemToDelete = Context.JobFitReports.Where(i => i.id == id).FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnJobFitReportDeleted(itemToDelete);

            Context.JobFitReports.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterJobFitReportDeleted(itemToDelete);

            return itemToDelete;
        }

        public async Task ExportAssessmentsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/assessments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/assessments/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        public async Task ExportAssessmentsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/assessments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/assessments/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }



        partial void OnAssessmentsRead(
            ref IQueryable<WebAdmin.Models.adminPanelProject.Assessment> items
        );

        public async Task<IQueryable<WebAdmin.Models.adminPanelProject.Assessment>> GetAssessments(
            Query query = null
        )
        {
            var items = Context.Assessments.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAssessmentsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAssessmentGet(WebAdmin.Models.adminPanelProject.Assessment item);

        public async Task<WebAdmin.Models.adminPanelProject.Assessment> GetAssessmentById(string id)
        {
            var items = Context.Assessments.AsNoTracking().Where(i => i.id == id);

            var itemToReturn = items.FirstOrDefault();

            OnAssessmentGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public List<WebAdmin.Models.adminPanelProject.TransposeData> GenerateTransposData()
        {

            // var tableNames = AssessmentDatas.FromSqlRaw($"INSERT INTO public.\"AssessmentDatas\" (assessmentName) select column_name from INFORMATION_SCHEMA.columns where TABLE_NAME = 'Assessments'").ToList();
            //Generate the target dynamic sql statemnet for transposing column to
            // var tableNames = AssessmentDatas.FromSqlRaw($"DO $$DECLARE target_table text := 'TransposeDatas'; source_table text := 'Assessments'; dynamic_sql text BEGIN EXECUTE format('DROP TABLE IF EXISTS %I', target_table);EXECUTE format('CREATE TABLE %I (column_name text, column_value text)', target_table); SELECT format('INSERT INTO %I (column_name, column_value)SELECT unnest(array[% s]) AS column_name, unnest(array[% s]) AS column_value FROM % I', target_table, string_agg(quote_literal(attname), ','), string_agg(quote_ident(attname) || '::text', ','), source_table)INTO dynamic_sql FROM pg_attribute WHERE attrelid = (SELECT oid FROM pg_class WHERE relname = source_table) AND attnum > 0 AND NOT attisdropped; EXECUTE dynamic_sql;END$$;").ToList();

            // using (var context = new ApplicationIdentityDbContext())
            // {
            var targetTable = "TransposeDatas";
            var sourceTable = "Assessments";
            var dynamicSql = $@"
                                    DO $$DECLARE
                                        target_table text := '{targetTable}';
                                        source_table text := '{sourceTable}';
                                        dynamic_sql text;
                                    BEGIN
                                        EXECUTE format('DROP TABLE IF EXISTS %I', target_table);
                                        EXECUTE format('CREATE TABLE %I (column_name text, column_value text)', target_table);
                                
                                        SELECT format(
                                            'INSERT INTO %I (column_name, column_value)
                                            SELECT unnest(array[%s]) AS column_name, unnest(array[%s]) AS column_value
                                            FROM %I',
                                            target_table,
                                            string_agg(quote_literal(attname), ','),
                                            string_agg(quote_ident(attname) || '::text', ','),
                                            source_table
                                        )
                                        INTO dynamic_sql
                                        FROM pg_attribute
                                        WHERE attrelid = (SELECT oid FROM pg_class WHERE relname = source_table)
                                            AND attnum > 0
                                            AND NOT attisdropped;
                                
                                        EXECUTE dynamic_sql;
                                    END$$;
                                ";

            context.Database.ExecuteSqlRaw(dynamicSql);

            // Query the transposed data from the target table
            var transposedData = context.TransposeDatas.ToList();

            // Return the transposed data
            return transposedData;
            // var targetTable = "transposed_data";
            // var sourceTable = "Assessments";
            // var dynamicSql = "";

            // // Drop the target table if it already exists
            // context.Database.ExecuteSqlRaw($"DROP TABLE IF EXISTS {targetTable}");

            // // Create the target table to store the transposed data
            // context.Database.ExecuteSqlRaw($"CREATE TABLE {targetTable} (column_name text, column_value tect)");

            // // Generate the dynamic SQL statement for transposing columns to rows
            // var query = context.TransposeDatas.FromSqlRaw(@"
            //     SELECT format(
            //         'INSERT INTO {0} (column_name, column_value)
            //         SELECT unnest(array[{1}]) AS column_name, unnest(array[{2}]::numeric[]) AS column_value
            //         FROM {3}',
            //         {0},
            //         string_agg(quote_literal(attname), ','),
            //         string_agg(quote_ident(attname) || '::text', ','),
            //         {4}
            //     )
            //     FROM pg_attribute
            //     WHERE attrelid = (
            //         SELECT oid FROM pg_class WHERE relname = {4}
            //     )
            //     AND attnum > 0
            //     AND NOT attisdropped",
            //     targetTable,
            //     sourceTable,
            //     sourceTable);

            // dynamicSql = query.Single().ToString();

            // // Execute the dynamic SQL statement to insert the transposed data
            // context.Database.ExecuteSqlRaw(dynamicSql);

            // // Query the transposed data from the target table
            // var transposedData = context.TransposeDatas.ToList();

            // // Return the transposed data
            // return transposedData;

            // }
            // return tableNames;
        }


        partial void OnAssessmentCreated(WebAdmin.Models.adminPanelProject.Assessment item);

        partial void OnAfterAssessmentCreated(WebAdmin.Models.adminPanelProject.Assessment item);

        public async Task<WebAdmin.Models.adminPanelProject.Assessment> CreateAssessment(
            WebAdmin.Models.adminPanelProject.Assessment assessment
        )
        {
            OnAssessmentCreated(assessment);


            var existingItem = Context.Assessments
                .Where(i => i.EmployeeId == assessment.EmployeeId)
                .FirstOrDefault();

            // foreach (var item in transposeDatas)
            // {
            //     item.EmployeeId = existingItem.EmployeeId;
            // }

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Assessments.Add(assessment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(assessment).State = EntityState.Detached;
                throw;
            }

            OnAfterAssessmentCreated(assessment);
            // var transposeDatas = context.GetTableNames();

            // foreach (var item in transposeDatas)
            // {
            //     item.EmployeeId = existingItem.EmployeeId;
            // }
            GenerateTransposData();
            // foreach (var item in columnAssessents)
            // {
            //     foreach (var item2 in dataAssessments)
            //     {
            //         // assessmentData.id = "dadasdasd";
            //         assessmentData.UserId = "2131";
            //         assessmentData.EmployeeId = "oke";
            //         assessmentData.assessmentName = item.ToString();
            //         Context.AssessmentDatas.Add(assessmentData);
            //         Context.SaveChanges();
            //     }
            //     // assessmentData.UserId = "2131";
            //     // assessmentData.assessmentName = item.Name;
            //     // Context.AssessmentDatas.Add(assessmentData);
            //     // Context.SaveChanges();

            // }
            return assessment;
        }



        partial void OnDataItemCreated(WebAdmin.Models.adminPanelProject.DataItem item);

        partial void OnAfterDataItemCreated(WebAdmin.Models.adminPanelProject.DataItem item);

        public async Task<WebAdmin.Models.adminPanelProject.DataItem> CreateDataItem(
            WebAdmin.Models.adminPanelProject.DataItem assessment
        )
        {
            OnDataItemCreated(assessment);


            var existingItem = Context.Assessments
                .Where(i => i.EmployeeId == assessment.employeeID)
                .FirstOrDefault();

            // foreach (var item in transposeDatas)
            // {
            //     item.EmployeeId = existingItem.EmployeeId;
            // }

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.dataItems.Add(assessment);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(assessment).State = EntityState.Detached;
                throw;
            }

            OnAfterDataItemCreated(assessment);

            return assessment;
        }


        public async Task<WebAdmin.Models.adminPanelProject.Assessment> CancelAssessmentChanges(
            WebAdmin.Models.adminPanelProject.Assessment item
        )
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnAssessmentUpdated(WebAdmin.Models.adminPanelProject.Assessment item);

        partial void OnAfterAssessmentUpdated(WebAdmin.Models.adminPanelProject.Assessment item);

        public async Task<WebAdmin.Models.adminPanelProject.Assessment> UpdateAssessment(
            string id,
            WebAdmin.Models.adminPanelProject.Assessment assessment
        )
        {
            OnAssessmentUpdated(assessment);

            var itemToUpdate = Context.Assessments
                .Where(i => i.id == assessment.id)
                .FirstOrDefault();

            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(assessment);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterAssessmentUpdated(assessment);

            return assessment;
        }

        partial void OnAssessmentDeleted(WebAdmin.Models.adminPanelProject.Assessment item);

        partial void OnAfterAssessmentDeleted(WebAdmin.Models.adminPanelProject.Assessment item);

        public async Task<WebAdmin.Models.adminPanelProject.Assessment> DeleteAssessment(string id)
        {
            var itemToDelete = Context.Assessments.Where(i => i.id == id).FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnAssessmentDeleted(itemToDelete);

            Context.Assessments.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAssessmentDeleted(itemToDelete);

            return itemToDelete;
        }

        public async Task ExportEmployeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/employees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        public async Task ExportEmployeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/employees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        partial void OnEmployeesRead(
            ref IQueryable<WebAdmin.Models.adminPanelProject.Employee> items
        );
        partial void OnTransposeDataRead(
            ref IQueryable<WebAdmin.Models.adminPanelProject.TransposeData> items
        );

        public async Task<IQueryable<WebAdmin.Models.adminPanelProject.TransposeData>> GetTransposeDatas(
          Query query = null
      )
        {
            var items = Context.TransposeDatas.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTransposeDataRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTransposeGet(List<WebAdmin.Models.adminPanelProject.TransposeData> item);

        public List<WebAdmin.Models.adminPanelProject.TransposeData> GetTransposeDataById(string parameterValue)
        {
            var query = Context.TransposeDatas
                  .Where(data => string.Compare(data.ColumnValue, parameterValue) >= 0)
            .Take(75);

            var result = query.ToList();

            // OnTransposeGet(result);

            return result;
        }

        public async Task<IQueryable<WebAdmin.Models.adminPanelProject.Employee>> GetEmployees(
            Query query = null
        )
        {
            var items = Context.Employees.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnEmployeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnEmployeeGet(WebAdmin.Models.adminPanelProject.Employee item);

        public async Task<WebAdmin.Models.adminPanelProject.Employee> GetEmployeeById(string id)
        {
            var items = Context.Employees.AsNoTracking().Where(i => i.id == id);

            var itemToReturn = items.FirstOrDefault();

            OnEmployeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        partial void OnEmployeeCreated(WebAdmin.Models.adminPanelProject.Employee item);

        partial void OnAfterEmployeeCreated(WebAdmin.Models.adminPanelProject.Employee item);

        public async Task<WebAdmin.Models.adminPanelProject.Employee> CreateEmployee(
            WebAdmin.Models.adminPanelProject.Employee employee
        )
        {
            OnEmployeeCreated(employee);

            var existingItem = Context.Employees.Where(i => i.id == employee.id).FirstOrDefault();

            if (existingItem != null)
            {
                throw new Exception("Item already available");
            }

            try
            {
                Context.Employees.Add(employee);
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(employee).State = EntityState.Detached;
                throw;
            }

            OnAfterEmployeeCreated(employee);

            return employee;
        }

        public async Task<WebAdmin.Models.adminPanelProject.Employee> CancelEmployeeChanges(
            WebAdmin.Models.adminPanelProject.Employee item
        )
        {
            var entityToCancel = Context.Entry(item);
            if (entityToCancel.State == EntityState.Modified)
            {
                entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
                entityToCancel.State = EntityState.Unchanged;
            }

            return item;
        }

        partial void OnEmployeeUpdated(WebAdmin.Models.adminPanelProject.Employee item);

        partial void OnAfterEmployeeUpdated(WebAdmin.Models.adminPanelProject.Employee item);

        public async Task<WebAdmin.Models.adminPanelProject.Employee> UpdateEmployee(
            string id,
            WebAdmin.Models.adminPanelProject.Employee employee
        )
        {
            OnEmployeeUpdated(employee);

            var itemToUpdate = Context.Employees.Where(i => i.id == employee.id).FirstOrDefault();

            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(employee);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterEmployeeUpdated(employee);

            return employee;
        }

        partial void OnEmployeeDeleted(WebAdmin.Models.adminPanelProject.Employee item);

        partial void OnAfterEmployeeDeleted(WebAdmin.Models.adminPanelProject.Employee item);

        public async Task<WebAdmin.Models.adminPanelProject.Employee> DeleteEmployee(string id)
        {
            var itemToDelete = Context.Employees.Where(i => i.id == id).FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnEmployeeDeleted(itemToDelete);

            Context.Employees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterEmployeeDeleted(itemToDelete);

            return itemToDelete;
        }

        public ApplicationUser GetUserById(string id)
        {
            // var user = Context.Users.Where(i => i.CompanyName.ToLower() == id.ToLower()).FirstOrDefault();
            // return user;

            return Context.Users.FirstOrDefault(x => x.CompanyName.ToLower() == id.ToLower());
        }

        public JobFitReport GetJobFitReportByID(string id)
        {
            return Context.JobFitReports.FirstOrDefault(x => x.id == id);
        }

        public async Task ExportUsersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        public async Task ExportUsersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }
        partial void OnPlayerGet(WebAdmin.Models.adminPanelProject.Employee item);

        public WebAdmin.Models.adminPanelProject.Employee GetPlayerById(string id)
        {
            var items = Context.Employees.AsNoTracking().Where(i => i.EmployeeId == id);

            // items = items.Include(i => i.ApplicationUser);

            var itemToReturn = items.FirstOrDefault();

            OnPlayerGet(itemToReturn);

            // return await Task.FromResult(itemToReturn);
            return itemToReturn;
        }

        public Employee GetPlayerByID(string id)
        {
            // var user = Context.Users.Where(i => i.CompanyName.ToLower() == id.ToLower()).FirstOrDefault();
            // return user;

            return Context.Employees.FirstOrDefault(x => x.id == id);
        }


        partial void OnPlayerUpdated(WebAdmin.Models.adminPanelProject.Employee item);

        partial void OnAfterPlayerUpdated(WebAdmin.Models.adminPanelProject.Employee item);

        public async Task<WebAdmin.Models.adminPanelProject.Employee> UpdatePlayer(
            string id,
            WebAdmin.Models.adminPanelProject.Employee employee
        )
        {
            OnPlayerUpdated(employee);

            var itemToUpdate = Context.Employees.Where(i => i.id == employee.id).FirstOrDefault();

            if (itemToUpdate == null)
            {
                throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(employee);
            entryToUpdate.State = EntityState.Modified;

            Context.SaveChanges();

            OnAfterPlayerUpdated(employee);

            return employee;
        }

        public async Task ExportPlayersToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/players/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/players/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        public async Task ExportPlayersToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(
                query != null
                    ? query.ToUrl(
                        $"export/adminpanelproject/players/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')"
                    )
                    : $"export/adminpanelproject/players/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')",
                true
            );
        }

        partial void OnPlayersRead(
            ref IQueryable<WebAdmin.Models.adminPanelProject.Employee> items
        );

        // partial void OnUsersRead(ref IQueryable<WebAdmin.Models.ApplicationUser> userItems);


        public async Task<IQueryable<WebAdmin.Models.adminPanelProject.Employee>> GetPlayers(
            Query query = null
        )
        {
            var items = Context.Employees.AsQueryable();

            // items = items.Include(i => i.ApplicationUser);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach (var p in propertiesToExpand)
                    {
                        items = items.Include(p.Trim());
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPlayersRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPlayerDeleted(WebAdmin.Models.adminPanelProject.Employee item);

        partial void OnAfterPlayerDeleted(WebAdmin.Models.adminPanelProject.Employee item);

        public async Task<WebAdmin.Models.adminPanelProject.Employee> DeletePlayer(string id)
        {
            var itemToDelete = Context.Employees.Where(i => i.id == id).FirstOrDefault();

            if (itemToDelete == null)
            {
                throw new Exception("Item no longer available");
            }

            OnPlayerDeleted(itemToDelete);

            Context.Employees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPlayerDeleted(itemToDelete);

            return itemToDelete;
        }

        public Assessment GetAssessmentByID(string id)
        {
            // var user = Context.Users.Where(i => i.CompanyName.ToLower() == id.ToLower()).FirstOrDefault();
            // return user;

            return Context.Assessments.FirstOrDefault(x => x.EmployeeId == id);
        }



        public async Task<List<ZipEntry>> ExtractFiles(Stream fileData)
        {
            await using var ms = new MemoryStream();
            await fileData.CopyToAsync(ms);

            using var archive = new ZipArchive(ms);

            var entries = new List<ZipEntry>();

            foreach (var entry in archive.Entries)
            {
                await using var fileStream = entry.Open();
                var fileBytes = await fileStream.ReadFully();
                var content = Encoding.UTF8.GetString(fileBytes);

                entries.Add(new ZipEntry { Name = entry.FullName, Content = content });
            }

            return entries;
        }

        private static readonly string SaveDirectory = "wwwroot/uploads"; // Change this path according to your requirements


        public async Task<string> SaveFile(IBrowserFile file)
        {
            var fileName = Path.GetFileName(file.Name);
            var filePath = Path.Combine(SaveDirectory, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.OpenReadStream().CopyToAsync(fileStream);
            }

            return filePath;
        }
        public Task<Stream> GetFileStream(string filePath)
        {
            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            return Task.FromResult<Stream>(fileStream);
        }

    }

    public static class NewBaseType
    {
        public static async Task<byte[]> ReadFully(this Stream input)
        {
            await using var ms = new MemoryStream();
            await input.CopyToAsync(ms);
            return ms.ToArray();
        }
    }
}
