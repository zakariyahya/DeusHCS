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
        public string CEOGeneralDirector { get; set; }

        [Required]
        public string AdministrativeStaff { get; set; }

        [Required]
        public string CreativeDesignManager { get; set; }

        [Required]
        public string FinanceStaff { get; set; }

        [Required]
        public string FinanceManager { get; set; }

        [Required]
        public string HRStaff { get; set; }

        [Required]
        public string HRManager { get; set; }

        [Required]
        public string ITStaff { get; set; }

        [Required]
        public string ITManager { get; set; }

        [Required]
        public string MarketingStaff { get; set; }

        [Required]
        public string ProductStaff { get; set; }

        [Required]
        public string ProductManager { get; set; }

        [Required]
        public string SalesStaff { get; set; }

        [Required]
        public string CustomerService { get; set; }

        [Required]
        public string SalesManager { get; set; }

    }
}