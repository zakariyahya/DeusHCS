using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebAdmin.Models.adminPanelProject
{
    [Table("Employees", Schema = "public")]
    public partial class Employee
    {
        [Key]
         [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string id { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public string JobTitle { get; set; }

        [Required]
        public string JobLevel { get; set; }

        [Required]
        public string Departement { get; set; }

        [Required]
        public string YearHired { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public DateTime ApplyDate { get; set; }

        [Required]
        public DateTime TesetDate { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public string AgeGroup { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string createdBy { get; set; }

        public DateTime? createdTime { get; set; }

        public DateTime? lastModifiedTime { get; set; }

        public string EmployeeId { get; set; }

    }
}