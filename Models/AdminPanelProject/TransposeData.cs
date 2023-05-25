

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAdmin.Models.adminPanelProject
{
    [Table("TransposeDatas", Schema = "public")]
    [Keyless]
    public partial class TransposeData
    {
        // public string EmployeeId {get; set; }
        public string ColumnName { get; set; }
        public string ColumnValue { get; set; }
    }
}
