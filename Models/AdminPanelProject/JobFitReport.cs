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
        public string? Bio { get; set; }
        public string? Insight { get; set; }

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
        
        [Required]
        public string X1 { get; set; }

        [Required]
        public string X2 { get; set; }

        [Required]
        public string X3 { get; set; }

        [Required]
        public string X4 { get; set; }

        [Required]
        public string X5 { get; set; }

        [Required]
        public string X6 { get; set; }

        [Required]
        public string X7 { get; set; }

        [Required]
        public string X8 { get; set; }

        [Required]
        public string X9 { get; set; }

        [Required]
        public string X10 { get; set; }

        [Required]
        public string X11 { get; set; }

        [Required]
        public string X12 { get; set; }

        [Required]
        public string X13 { get; set; }

        [Required]
        public string X14 { get; set; }

        [Required]
        public string X15 { get; set; }

        [Required]
        public string X16 { get; set; }
        
        [Required]
        public string X17 { get; set; }

    }
}