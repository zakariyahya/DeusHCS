using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebAdmin.Models;

namespace AdminPanel.Data
{
    public partial class ApplicationIdentityDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        public ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options) : base(options)
        {
        }

        public ApplicationIdentityDbContext()
        {
        }

        partial void OnModelBuilding(ModelBuilder builder);
        public DbSet<WebAdmin.Models.adminPanelProject.Employee> Employees { get; set; }
        public DbSet<WebAdmin.Models.adminPanelProject.Assessment> Assessments { get; set; }
        public DbSet<WebAdmin.Models.adminPanelProject.JobFitReport> JobFitReports { get; set; }

    }
}