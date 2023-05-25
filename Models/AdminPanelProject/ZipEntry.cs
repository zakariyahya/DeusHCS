

using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebAdmin.Models.adminPanelProject
{
    // [Table("TransposeDatas", Schema = "public")]
    [Keyless]
    public record ZipEntry
    {
        public string Name { get; init; }
        public string Content { get; init; }
    }
}
