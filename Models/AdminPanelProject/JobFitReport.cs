using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdmin.Models.adminPanelProject
{
    [Table("JobFitReports", Schema = "public")]
    public partial class JobFitReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string EmployeeId { get; set; }

        [Required]
        public decimal CEOGeneralDirector { get; set; }

        [Required]
        public decimal AdministrativeStaff { get; set; }

        [Required]
        public decimal CreativeDesignManager { get; set; }

        [Required]
        public decimal FinanceStaff { get; set; }

        [Required]
        public decimal FinanceManager { get; set; }

        [Required]
        public decimal HRStaff { get; set; }

        [Required]
        public decimal HRManager { get; set; }

        [Required]
        public decimal ITStaff { get; set; }

        [Required]
        public decimal ITManager { get; set; }

        [Required]
        public decimal MarketingStaff { get; set; }

        [Required]
        public decimal ProductStaff { get; set; }

        [Required]
        public decimal ProductManager { get; set; }

        [Required]
        public decimal SalesStaff { get; set; }

        [Required]
        public decimal CustomerService { get; set; }

        [Required]
        public decimal SalesManager { get; set; }

    }
}