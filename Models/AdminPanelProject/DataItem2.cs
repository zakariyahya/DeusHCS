using Microsoft.EntityFrameworkCore;

namespace WebAdmin.Models.adminPanelProject
{
    [Keyless]

    public partial class DataItem2
    {
        public string? employeeID { get; set; }
        // public List<string> Quarter { get; set; }
        // public List<double> Revenue { get; set; }
        public string? Quarter { get; set; }
        public decimal? Revenue { get; set; }
    }
}