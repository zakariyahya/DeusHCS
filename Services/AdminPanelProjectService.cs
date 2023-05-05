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

        public adminPanelProjectService(
            ApplicationIdentityDbContext context,
            NavigationManager navigationManager,
            IWebHostEnvironment environment,
            SecurityService securityService,
            UserManager<ApplicationUser> userManager,
            IConfiguration configuration
        )
        {
            this.context = context;
            this.navigationManager = navigationManager;
            this.environment = environment;
            this.securityService = securityService;
            this.userManager = userManager;
            this.configuration = configuration;
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
            var path = Path.Combine(environment.ContentRootPath, "wwwroot/Data", file.Name);
            var memoryStream = new MemoryStream();
            await file.Data.CopyToAsync(memoryStream);
            using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                memoryStream.WriteTo(fileStream);
            }
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
        public List<ApplicationUser> ReadExcel()
        {
            // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";
            // string FilePath = Path.Combine(environment.WebRootPath, "wwwroot/dataCompany.xlsx");

            string FilePath =
                "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/dataCompany.xlsx";
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

        public List<JobFitReport> ReadJobFitReportExcel()
        {
            // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";
            // string FilePath = Path.Combine(environment.WebRootPath, "wwwroot/dataCompany.xlsx");

            string FilePath =
                "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/dataJobFitReports.xlsx";
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
                    JobFitReport jbr = new JobFitReport();
                    for (int col = 1; col <= rolCount; col++)
                    {
                        if (col == 2)
                            jbr.EmployeeId = worksheet.Cells[row, col].Value.ToString();
                        else if (col == 3)
                            jbr.CEOGeneralDirector = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 4)
                            jbr.AdministrativeStaff = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 5)
                            jbr.CreativeDesignManager = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 6)
                            jbr.FinanceStaff = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 7)
                            jbr.FinanceManager = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 8)
                            jbr.HRStaff = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 9)
                            jbr.HRManager = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 10)
                            jbr.ITStaff = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 11)
                            jbr.ITManager = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 12)
                            jbr.MarketingStaff = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 13)
                            jbr.ProductStaff = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 14)
                            jbr.ProductManager = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 15)
                            jbr.SalesStaff = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 16)
                            jbr.CustomerService = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                        else if (col == 17)
                            jbr.SalesManager = Convert.ToDecimal(
                                worksheet.Cells[row, col].Value.ToString()
                            );
                    }
                    jbrs.Add(jbr);
                }
            }
            return jbrs;
        }

        List<ApplicationUser> companies = new List<ApplicationUser>();
        List<JobFitReport> jbrs = new List<JobFitReport>();

        List<Employee> employees = new List<Employee>();

        public List<Employee> ReadEmployeeExcel()
        {
            // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";
            string FilePath =
                "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/dataEmployee.xlsx";

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

        public List<Assessment> ReadAssessmentExcel()
        {
            List<Assessment> assessments = new List<Assessment>();
            // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";
            string FilePath =
                "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/dataAssessment.xlsx";

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
                        // else if (col == 2)
                        //     assessment.Diplomatic = Convert.ToInt32(
                        //         worksheet.Cells[row, col].Value.ToString()
                        //     );
                        // else if (col == 3)
                        //     assessment.Empathy = Convert.ToInt32(
                        //         worksheet.Cells[row, col].Value.ToString()
                        //     );
                        // else if (col == 4)
                        //     assessment.Cooperative = Convert.ToInt32(
                        //         worksheet.Cells[row, col].Value.ToString()
                        //     );
                        // else if (col == 5)
                        //     assessment.Trust = Convert.ToInt32(
                        //         worksheet.Cells[row, col].Value.ToString()
                        //     );
                        // else if (col == 6) assessment.Ambitious = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 7) assessment.Competitive = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 8) assessment.Direct = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 9) assessment.Rational = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 10) assessment.Conformity = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 11) assessment.Diligent = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 12) assessment.Organized = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 13) assessment.Temperance = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 14) assessment.Versatile = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 15) assessment.Instinctive = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 16) assessment.Opportunistic = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 17) assessment.Unorthodox = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 18) assessment.Adventurous = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 19) assessment.Assertive = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 20) assessment.Optimistic = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 21) assessment.Gregarious = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 22) assessment.Independent = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 23) assessment.Introspective = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 24) assessment.Restrained = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 25) assessment.Ingenuity = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 26) assessment.Inquisitive = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 27) assessment.Intricate = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 28) assessment.StrategicallyMinded = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 29) assessment.Methodical = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 30) assessment.PresentMinded = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 31) assessment.Utilitarian = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 32) assessment.Relationship_Driven = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 33) assessment.ResultsDriven = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 34) assessment.MotivatedByOrder = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 35) assessment.MotivatedByAmbiguity = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 36) assessment.Outgoing = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 37) assessment.Reserved = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 38) assessment.Innovative = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 39) assessment.Pragmatic = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 40) assessment.VisualPerception = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 41) assessment.SpatialReasoning = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 42) assessment.VerbalComprehension = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 43) assessment.DominantLeadership = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 44) assessment.CollaborativeLeadership = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 45) assessment.ServantLeadership = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 46) assessment.SupportingOthers = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 47) assessment.CoachingOthers = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 48) assessment.EnablingTeamwork = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 49) assessment.BuildingConnections = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 50) assessment.PersuadingOthers = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 51) assessment.AdaptiveCommunication = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 52) assessment.TrendIdentification = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 53) assessment.DistillingInformation = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 54) assessment.ApplyingExpertise = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 55) assessment.ActiveLearning = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 56) assessment.StrategicAgility = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 57) assessment.ChampioningChange = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 58) assessment.ForwardPlanning = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 59) assessment.FollowingDirections = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 60) assessment.QualityClientDelivery = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 61) assessment.ResilienceUnderPressure = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 62) assessment.LateralProblemSolving = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 63) assessment.OperationalFlexibility = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 64) assessment.SelfDevelopmentFocused = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 65) assessment.OperationalFlexibility = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 66) assessment.BusinessAcumen = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 67) assessment.OptimizingPerformance = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 68) assessment.LeadingAndDeciding = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 69) assessment.SupportingAndCooperating = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 70) assessment.InteractingAndNegotiating = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 71) assessment.AnalyzeAndInterpret = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 72) assessment.CreateAndConceptualize = worksheet.Cells[row, col].Value.ToString();
                        // else if (col == 73) assessment.OrganizeAndExecute = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 74) assessment.AdaptAndCope = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                        // else if (col == 75) assessment.EnterprisingAndPerforming = Convert.ToInt32(worksheet.Cells[row, col].Value.ToString());
                    }
                    assessments.Add(assessment);
                }
            }

            // AddExcel(companies);

            // foreach (var item in companies)
            // {

            //     MailClass mailClass = this.GetMailObject(item);
            //     SendMail(mailClass);
            //     // sendEmail(item.email, item.password);
            // }

            return assessments;
        }

        public List<ApplicationUser> AddExcel(List<ApplicationUser> companies)
        {
            // var companies = ReadExcel();
            foreach (var item in companies)
            {
                var password = GenerateRandomPassword();

                item.Password = password;
                securityService.Register(item.Email, item.Password, item.CompanyName, item.CompanyId);
            }

            return companies;
        }

        public List<JobFitReport> AddJobFitReportExcel(List<JobFitReport> jbs)
        {
            // var companies = ReadExcel();
            foreach (var item in jbs)
            {
                CreateJobFitReport(item);
            }

            return jbrs;
        }

        public List<Employee> AddEmployeeExcel(List<Employee> employees)
        {
            // var companies = ReadExcel();
            foreach (var item in employees)
            {
                item.UserId = "UserId";
                item.createdTime = DateTime.Now;
                item.PhoneNumber = "0888";
                item.createdBy = "System";
                CreatePlayer(item);
            }

            return employees;
        }

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

        public async Task<IQueryable<WebAdmin.Models.ApplicationUser>> GetUsers(Query query = null)
        {
            var items = Context.Users.AsQueryable();

            // items = items.Include(i => i.UserName);

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

            return await Task.FromResult(items);
        }

        partial void OnJobFitReportGet(WebAdmin.Models.adminPanelProject.JobFitReport item);

        public async Task<WebAdmin.Models.adminPanelProject.JobFitReport> GetJobFitReportById(
            string id
        )
        {
            var items = Context.JobFitReports.AsNoTracking().Where(i => i.id == id);

            var itemToReturn = items.FirstOrDefault();

            OnJobFitReportGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
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

        partial void OnAssessmentCreated(WebAdmin.Models.adminPanelProject.Assessment item);

        partial void OnAfterAssessmentCreated(WebAdmin.Models.adminPanelProject.Assessment item);

        public async Task<WebAdmin.Models.adminPanelProject.Assessment> CreateAssessment(
            WebAdmin.Models.adminPanelProject.Assessment assessment
        )
        {
            OnAssessmentCreated(assessment);

            var existingItem = Context.Assessments
                .Where(i => i.id == assessment.id)
                .FirstOrDefault();

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

        public async Task<WebAdmin.Models.adminPanelProject.Employee> GetPlayerById(string id)
        {
            var items = Context.Employees.AsNoTracking().Where(i => i.id == id);

            // items = items.Include(i => i.ApplicationUser);

            var itemToReturn = items.FirstOrDefault();

            OnPlayerGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
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

            return Context.Assessments.FirstOrDefault(x => x.id == id);
        }
    }
}
