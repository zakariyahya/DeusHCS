using AdminPanel.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using WebAdmin.Models.adminPanelProject;

namespace WebAdmin
{
    public class ContextHelper
    {
        private readonly ILogger<ContextHelper> logger;
        private readonly ApplicationIdentityDbContext context;

        // private static Dictionary<Type, string> tableNames = new Dictionary<Type, string>(30);

        // private Dictionary<Type, Dictionary<string, string>> columnNames = new Dictionary<Type, Dictionary<string, string>>(30);

        public ContextHelper(ILogger<ContextHelper> logger, ApplicationIdentityDbContext context)
        {
            this.logger = logger;
            this.context = context;

            // PopulateTableNames();
            // PopulateColumnNames();
        }
     

        public List<ColumnInfo> GetAllColumns(Assessment assessmentData)
        {
            var entityType = context.Model.FindEntityType(assessmentData.ToString());
            var properties = entityType.GetProperties();
            List<ColumnInfo> columns = new List<ColumnInfo>(properties.Count());
            foreach (var property in properties)
                columns.Add(new ColumnInfo(property.GetColumnName(StoreObjectIdentifier.SqlQuery(entityType)), property.GetColumnType()));

            return columns;
        }

        public class ColumnInfo
        {
            public string Name;
            public string Type;

            public ColumnInfo(string Name, string Type)
            {
                this.Name = Name;
                this.Type = Type;
            }
        }
    }
}