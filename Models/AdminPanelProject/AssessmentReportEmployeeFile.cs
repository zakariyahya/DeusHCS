using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAdmin.Models.adminPanelProject
{
    public partial class AssessmentReportEmployeeFile
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }
        [Required]
        public string UserId { get; set; }
        public string? employeeID { get; set; }
        public string? employeeName { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
    }
}