// using System;
// using System.Data;
// using System.Linq;
// using System.Linq.Dynamic.Core;
// using System.Collections.Generic;
// using System.Threading.Tasks;
// using System.Text.Encodings.Web;
// using Microsoft.AspNetCore.Components;
// using Microsoft.EntityFrameworkCore;
// using Radzen;
// using AdminPanel.Data;
// using BlazorInputFile;
// using WebAdmin.Models;
// using OfficeOpenXml;
// using System.Text;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.AspNetCore.Mvc;

// namespace WebAdmin
// {
//     public partial class adminPanelProjectService : Controller
//     {
//         ApplicationIdentityDbContext Context
//         {
//             get
//             {
//                 return this.context;
//             }
//         }

//         private readonly ApplicationIdentityDbContext context;
//         private readonly SecurityService securityService;
//         private readonly NavigationManager navigationManager;
//         private readonly IWebHostEnvironment environment;
//         private readonly UserManager<ApplicationUser> userManager;
//         private readonly IConfiguration configuration;

//         public adminPanelProjectService(ApplicationIdentityDbContext context, NavigationManager navigationManager, IWebHostEnvironment environment, SecurityService securityService
//         , UserManager<ApplicationUser> userManager, IConfiguration configuration)
//         {
//             this.context = context;
//             this.navigationManager = navigationManager;
//             this.environment = environment;
//             this.securityService = securityService;
//             this.userManager = userManager;
//             this.configuration = configuration;
//         }

//         public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);


//         public async Task ExportPlayersToExcel(Query query = null, string fileName = null)
//         {
//             navigationManager.NavigateTo(query != null ? query.ToUrl($"export/adminpanelproject/players/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/adminpanelproject/players/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
//         }

//         public async Task ExportPlayersToCSV(Query query = null, string fileName = null)
//         {
//             navigationManager.NavigateTo(query != null ? query.ToUrl($"export/adminpanelproject/players/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/adminpanelproject/players/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
//         }



//         partial void OnPlayersRead(ref IQueryable<WebAdmin.Models.adminPanelProject.Employee> items);
//         partial void OnUsersRead(ref IQueryable<WebAdmin.Models.ApplicationUser> userItems);


//         public async Task<IQueryable<WebAdmin.Models.adminPanelProject.Employee>> GetPlayers(Query query = null)
//         {
//             var items = Context.Employees.AsQueryable();

//             // items = items.Include(i => i.ApplicationUser);

//             if (query != null)
//             {
//                 if (!string.IsNullOrEmpty(query.Expand))
//                 {
//                     var propertiesToExpand = query.Expand.Split(',');
//                     foreach (var p in propertiesToExpand)
//                     {
//                         items = items.Include(p.Trim());
//                     }
//                 }

//                 if (!string.IsNullOrEmpty(query.Filter))
//                 {
//                     if (query.FilterParameters != null)
//                     {
//                         items = items.Where(query.Filter, query.FilterParameters);
//                     }
//                     else
//                     {
//                         items = items.Where(query.Filter);
//                     }
//                 }

//                 if (!string.IsNullOrEmpty(query.OrderBy))
//                 {
//                     items = items.OrderBy(query.OrderBy);
//                 }

//                 if (query.Skip.HasValue)
//                 {
//                     items = items.Skip(query.Skip.Value);
//                 }

//                 if (query.Top.HasValue)
//                 {
//                     items = items.Take(query.Top.Value);
//                 }
//             }

//             OnPlayersRead(ref items);

//             return await Task.FromResult(items);
//         }
//         public async Task ExportUsersToCSV(Query query = null, string fileName = null)
//         {
//             navigationManager.NavigateTo(query != null ? query.ToUrl($"export/adminpanelproject/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/adminpanelproject/users/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
//         }
//         public async Task ExportUsersToExcel(Query query = null, string fileName = null)
//         {
//             navigationManager.NavigateTo(query != null ? query.ToUrl($"export/adminpanelproject/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/adminpanelproject/users/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
//         }
//         public async Task<IQueryable<WebAdmin.Models.ApplicationUser>> GetUsers(Query query = null)
//         {
//             var items = Context.Users.AsQueryable();

//             // items = items.Include(i => i.UserName);

//             if (query != null)
//             {
//                 if (!string.IsNullOrEmpty(query.Expand))
//                 {
//                     var propertiesToExpand = query.Expand.Split(',');
//                     foreach (var p in propertiesToExpand)
//                     {
//                         items = items.Include(p.Trim());
//                     }
//                 }

//                 if (!string.IsNullOrEmpty(query.Filter))
//                 {
//                     if (query.FilterParameters != null)
//                     {
//                         items = items.Where(query.Filter, query.FilterParameters);
//                     }
//                     else
//                     {
//                         items = items.Where(query.Filter);
//                     }
//                 }

//                 if (!string.IsNullOrEmpty(query.OrderBy))
//                 {
//                     items = items.OrderBy(query.OrderBy);
//                 }

//                 if (query.Skip.HasValue)
//                 {
//                     items = items.Skip(query.Skip.Value);
//                 }

//                 if (query.Top.HasValue)
//                 {
//                     items = items.Take(query.Top.Value);
//                 }
//             }

//             OnUsersRead(ref items);

//             return await Task.FromResult(items);
//         }

//         partial void OnPlayerGet(WebAdmin.Models.adminPanelProject.Employee item);

//         public async Task<WebAdmin.Models.adminPanelProject.Employee> GetPlayerById(string id)
//         {
//             var items = Context.Employees
//                               .AsNoTracking()
//                               .Where(i => i.id == id);

//             // items = items.Include(i => i.ApplicationUser);

//             var itemToReturn = items.FirstOrDefault();

//             OnPlayerGet(itemToReturn);

//             return await Task.FromResult(itemToReturn);
//         }

//         partial void OnPlayerCreated(WebAdmin.Models.adminPanelProject.Employee item);
//         partial void OnAfterPlayerCreated(WebAdmin.Models.adminPanelProject.Employee item);

//         public async Task<WebAdmin.Models.adminPanelProject.Employee> CreatePlayer(WebAdmin.Models.adminPanelProject.Employee employee)
//         {
//             OnPlayerCreated(employee);

//             var existingItem = Context.Employees
//                               .Where(i => i.id == employee.id)
//                               .FirstOrDefault();

//             if (existingItem != null)
//             {
//                 throw new Exception("Item already available");
//             }

//             try
//             {
//                 Context.Employees.Add(employee);
//                 Context.SaveChanges();
//             }
//             catch
//             {
//                 Context.Entry(employee).State = EntityState.Detached;
//                 throw;
//             }

//             OnAfterPlayerCreated(employee);

//             return employee;
//         }

//         public async Task<WebAdmin.Models.adminPanelProject.Employee> CancelPlayerChanges(WebAdmin.Models.adminPanelProject.Employee item)
//         {
//             var entityToCancel = Context.Entry(item);
//             if (entityToCancel.State == EntityState.Modified)
//             {
//                 entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
//                 entityToCancel.State = EntityState.Unchanged;
//             }

//             return item;
//         }

//         partial void OnPlayerUpdated(WebAdmin.Models.adminPanelProject.Employee item);
//         partial void OnAfterPlayerUpdated(WebAdmin.Models.adminPanelProject.Employee item);

//         public async Task<WebAdmin.Models.adminPanelProject.Employee> UpdatePlayer(string id, WebAdmin.Models.adminPanelProject.Employee employee)
//         {
//             OnPlayerUpdated(employee);

//             var itemToUpdate = Context.Employees
//                               .Where(i => i.id == employee.id)
//                               .FirstOrDefault();

//             if (itemToUpdate == null)
//             {
//                 throw new Exception("Item no longer available");
//             }

//             var entryToUpdate = Context.Entry(itemToUpdate);
//             entryToUpdate.CurrentValues.SetValues(employee);
//             entryToUpdate.State = EntityState.Modified;

//             Context.SaveChanges();

//             OnAfterPlayerUpdated(employee);

//             return employee;
//         }

//         partial void OnPlayerDeleted(WebAdmin.Models.adminPanelProject.Employee item);
//         partial void OnAfterPlayerDeleted(WebAdmin.Models.adminPanelProject.Employee item);

//         public async Task<WebAdmin.Models.adminPanelProject.Employee> DeletePlayer(string id)
//         {
//             var itemToDelete = Context.Employees
//                               .Where(i => i.id == id)
//                               .FirstOrDefault();

//             if (itemToDelete == null)
//             {
//                 throw new Exception("Item no longer available");
//             }

//             OnPlayerDeleted(itemToDelete);


//             Context.Employees.Remove(itemToDelete);

//             try
//             {
//                 Context.SaveChanges();
//             }
//             catch
//             {
//                 Context.Entry(itemToDelete).State = EntityState.Unchanged;
//                 throw;
//             }

//             OnAfterPlayerDeleted(itemToDelete);

//             return itemToDelete;
//         }

//         // public async Task ExportCompaniesToExcel(Query query = null, string fileName = null)
//         // {
//         //     navigationManager.NavigateTo(query != null ? query.ToUrl($"export/adminpanelproject/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/adminpanelproject/companies/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
//         // }



//         // partial void OnCompaniesRead(ref IQueryable<WebAdmin.Models.adminPanelProject.Company> items);

//         // public async Task<IQueryable<WebAdmin.Models.adminPanelProject.Company>> GetCompanies(Query query = null)
//         // {
//         //     var items = Context.Companies.AsQueryable();


//         //     if (query != null)
//         //     {
//         //         if (!string.IsNullOrEmpty(query.Expand))
//         //         {
//         //             var propertiesToExpand = query.Expand.Split(',');
//         //             foreach (var p in propertiesToExpand)
//         //             {
//         //                 items = items.Include(p.Trim());
//         //             }
//         //         }

//         //         if (!string.IsNullOrEmpty(query.Filter))
//         //         {
//         //             if (query.FilterParameters != null)
//         //             {
//         //                 items = items.Where(query.Filter, query.FilterParameters);
//         //             }
//         //             else
//         //             {
//         //                 items = items.Where(query.Filter);
//         //             }
//         //         }

//         //         if (!string.IsNullOrEmpty(query.OrderBy))
//         //         {
//         //             items = items.OrderBy(query.OrderBy);
//         //         }

//         //         if (query.Skip.HasValue)
//         //         {
//         //             items = items.Skip(query.Skip.Value);
//         //         }

//         //         if (query.Top.HasValue)
//         //         {
//         //             items = items.Take(query.Top.Value);
//         //         }
//         //     }

//         //     OnCompaniesRead(ref items);

//         //     return await Task.FromResult(items);
//         // }

//         // partial void OnCompanyGet(WebAdmin.Models.adminPanelProject.Company item);

//         // public async Task<WebAdmin.Models.adminPanelProject.Company> GetCompanyById(string id)
//         // {
//         //     var items = Context.Companies
//         //                       .AsNoTracking()
//         //                       .Where(i => i.id == id);


//         //     var itemToReturn = items.FirstOrDefault();

//         //     OnCompanyGet(itemToReturn);

//         //     return await Task.FromResult(itemToReturn);
//         // }

//         // partial void OnCompanyCreated(WebAdmin.Models.adminPanelProject.Company item);
//         // partial void OnAfterCompanyCreated(WebAdmin.Models.adminPanelProject.Company item);

//         // public async Task<WebAdmin.Models.adminPanelProject.Company> CreateCompany(WebAdmin.Models.adminPanelProject.Company company)
//         // {
//         //     OnCompanyCreated(company);

//         //     var existingItem = Context.Companies
//         //                       .Where(i => i.id == company.id)
//         //                       .FirstOrDefault();

//         //     if (existingItem != null)
//         //     {
//         //         throw new Exception("Item already available");
//         //     }

//         //     try
//         //     {
//         //         Context.Companies.Add(company);
//         //         Context.SaveChanges();
//         //     }
//         //     catch
//         //     {
//         //         Context.Entry(company).State = EntityState.Detached;
//         //         throw;
//         //     }

//         //     OnAfterCompanyCreated(company);

//         //     return company;
//         // }

//         // public async Task<WebAdmin.Models.adminPanelProject.Company> CancelCompanyChanges(WebAdmin.Models.adminPanelProject.Company item)
//         // {
//         //     var entityToCancel = Context.Entry(item);
//         //     if (entityToCancel.State == EntityState.Modified)
//         //     {
//         //         entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
//         //         entityToCancel.State = EntityState.Unchanged;
//         //     }

//         //     return item;
//         // }

//         // partial void OnCompanyUpdated(WebAdmin.Models.adminPanelProject.Company item);
//         // partial void OnAfterCompanyUpdated(WebAdmin.Models.adminPanelProject.Company item);

//         // public async Task<WebAdmin.Models.adminPanelProject.Company> UpdateCompany(string id, WebAdmin.Models.adminPanelProject.Company company)
//         // {
//         //     OnCompanyUpdated(company);

//         //     var itemToUpdate = Context.Companies
//         //                       .Where(i => i.id == company.id)
//         //                       .FirstOrDefault();

//         //     if (itemToUpdate == null)
//         //     {
//         //         throw new Exception("Item no longer available");
//         //     }

//         //     var entryToUpdate = Context.Entry(itemToUpdate);
//         //     entryToUpdate.CurrentValues.SetValues(company);
//         //     entryToUpdate.State = EntityState.Modified;

//         //     Context.SaveChanges();

//         //     OnAfterCompanyUpdated(company);

//         //     return company;
//         // }

//         // partial void OnCompanyDeleted(WebAdmin.Models.adminPanelProject.Company item);
//         // partial void OnAfterCompanyDeleted(WebAdmin.Models.adminPanelProject.Company item);

//         // public async Task<WebAdmin.Models.adminPanelProject.Company> DeleteCompany(string id)
//         // {
//         //     var itemToDelete = Context.Companies
//         //                       .Where(i => i.id == id)
//         //                       .FirstOrDefault();

//         //     if (itemToDelete == null)
//         //     {
//         //         throw new Exception("Item no longer available");
//         //     }

//         //     OnCompanyDeleted(itemToDelete);


//         //     Context.Companies.Remove(itemToDelete);

//         //     try
//         //     {
//         //         Context.SaveChanges();
//         //     }
//         //     catch
//         //     {
//         //         Context.Entry(itemToDelete).State = EntityState.Unchanged;
//         //         throw;
//         //     }

//         //     OnAfterCompanyDeleted(itemToDelete);

//         //     return itemToDelete;
//         // }
//         public async Task Upload(IFileListEntry file)
//         {
//             var path = Path.Combine(environment.ContentRootPath, "wwwroot/Data", file.Name);
//             var memoryStream = new MemoryStream();
//             await file.Data.CopyToAsync(memoryStream);
//             using (FileStream fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
//             {
//                 memoryStream.WriteTo(fileStream);
//             }
//             // ReadExcel();
//         }

//         public List<ApplicationUser> ReadExcel()
//         {
//             List<ApplicationUser> companies = new List<ApplicationUser>();
//             // string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";
//             string FilePath = "C:/Users/Yahya/Documents/Project/AdminPanel/AdminPanel/wwwroot/Data/data.xlsx";

//             System.IO.FileInfo existingFile = new System.IO.FileInfo(FilePath);
//             ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
//             using (ExcelPackage package = new ExcelPackage(existingFile))
//             {
//                 ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
//                 int rolCount = worksheet.Dimension.End.Column;
//                 int rowCount = worksheet.Dimension.End.Row;

//                 for (int row = 2; row <= rowCount; row++)
//                 {
//                     ApplicationUser company = new ApplicationUser();
//                     for (int col = 1; col <= rolCount; col++)
//                     {
//                         if (col == 1) company.Id = worksheet.Cells[row, col].Value.ToString();
//                         else if (col == 2) company.CompanyName = worksheet.Cells[row, col].Value.ToString();
//                         else if (col == 3) company.Email = worksheet.Cells[row, col].Value.ToString();

//                     }
//                     companies.Add(company);
//                 }
//             }

//             // AddExcel(companies);

//             // foreach (var item in companies)
//             // {

//             //     MailClass mailClass = this.GetMailObject(item);
//             //     SendMail(mailClass);
//             //     // sendEmail(item.email, item.password);
//             // }

//             return companies;
//         }

//         private static string GenerateRandomPassword()
//         {
//             var options = new PasswordOptions
//             {
//                 RequiredLength = 8,
//                 RequiredUniqueChars = 4,
//                 RequireDigit = true,
//                 RequireLowercase = true,
//                 RequireNonAlphanumeric = true,
//                 RequireUppercase = true
//             };

//             var randomChars = new[] {
//                 "ABCDEFGHJKLMNOPQRSTUVWXYZ",
//                 "abcdefghijkmnopqrstuvwxyz",
//                 "0123456789",
//                 "!@$?_-"
//             };

//             var rand = new Random(Environment.TickCount);
//             var chars = new List<char>();

//             if (options.RequireUppercase)
//             {
//                 chars.Insert(rand.Next(0, chars.Count), randomChars[0][rand.Next(0, randomChars[0].Length)]);
//             }

//             if (options.RequireLowercase)
//             {
//                 chars.Insert(rand.Next(0, chars.Count), randomChars[1][rand.Next(0, randomChars[1].Length)]);
//             }

//             if (options.RequireDigit)
//             {
//                 chars.Insert(rand.Next(0, chars.Count), randomChars[2][rand.Next(0, randomChars[2].Length)]);
//             }

//             if (options.RequireNonAlphanumeric)
//             {
//                 chars.Insert(rand.Next(0, chars.Count), randomChars[3][rand.Next(0, randomChars[3].Length)]);
//             }

//             for (int i = chars.Count; i < options.RequiredLength || chars.Distinct().Count() < options.RequiredUniqueChars; i++)
//             {
//                 string rcs = randomChars[rand.Next(0, randomChars.Length)];
//                 chars.Insert(rand.Next(0, chars.Count), rcs[rand.Next(0, rcs.Length)]);
//             }

//             return new string(chars.ToArray());
//         }

//         public List<ApplicationUser> AddExcel(List<ApplicationUser> companies)
//         {

//             // var companies = ReadExcel();
//             foreach (var item in companies)
//             {
//                 var password = GenerateRandomPassword();

//                 item.Password = password;
//                 securityService.Register(item.Email, item.Password, item.CompanyName);
//             }

//             return companies;
//         }


//         public ApplicationUser GetUserById(string id)
//         {
//             // var user = Context.Users.Where(i => i.CompanyName.ToLower() == id.ToLower()).FirstOrDefault();
//             // return user;

//             return Context.Users.FirstOrDefault(x => x.CompanyName.ToLower() == id.ToLower());
//         }

//     }
// }