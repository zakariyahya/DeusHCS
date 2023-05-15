// using System;
// using System.Linq;
// using System.Collections.Generic;
// using Microsoft.EntityFrameworkCore;

// using WebAdmin.Models.adminPanelProject;

// namespace WebAdmin.Data
// {
//     public partial class adminPanelProjectContext : DbContext
//     {
//         public adminPanelProjectContext()
//         {
//         }

//         public adminPanelProjectContext(DbContextOptions<adminPanelProjectContext> options) : base(options)
//         {
//         }

//         partial void OnModelBuilding(ModelBuilder builder);

//         protected override void OnModelCreating(ModelBuilder builder)
//         {
//             base.OnModelCreating(builder);

//             builder.Entity<WebAdmin.Models.adminPanelProject.Employee>()
//               .Property(p => p.EmployeeId)
//               .HasDefaultValueSql(@"''::text");
//             this.OnModelBuilding(builder);
//         }

//         public DbSet<WebAdmin.Models.adminPanelProject.JobFitReport> JobFitReports { get; set; }

//         public DbSet<WebAdmin.Models.adminPanelProject.Assessment> Assessments { get; set; }

//         public DbSet<WebAdmin.Models.adminPanelProject.Employee> Employees { get; set; }
//     }
// }