using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using WebAdmin.Models;
using Microsoft.EntityFrameworkCore.Metadata;

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
        // public DbSet<WebAdmin.Models.adminPanelProject.AssessmentData> AssessmentDatas { get; set; }
        public DbSet<WebAdmin.Models.adminPanelProject.TransposeData> TransposeDatas { get; set; }
        public DbSet<WebAdmin.Models.adminPanelProject.DataItem> dataItems { get; set; }
        public DbSet<WebAdmin.Models.adminPanelProject.AssessmentReportEmployeeFile> assessmentReportEmployeeFiles { get; set; }




        public List<WebAdmin.Models.adminPanelProject.TransposeData> GetTableNames()
        {

            // var tableNames = AssessmentDatas.FromSqlRaw($"INSERT INTO public.\"AssessmentDatas\" (assessmentName) select column_name from INFORMATION_SCHEMA.columns where TABLE_NAME = 'Assessments'").ToList();
            //Generate the target dynamic sql statemnet for transposing column to
            // var tableNames = AssessmentDatas.FromSqlRaw($"DO $$DECLARE target_table text := 'TransposeDatas'; source_table text := 'Assessments'; dynamic_sql text BEGIN EXECUTE format('DROP TABLE IF EXISTS %I', target_table);EXECUTE format('CREATE TABLE %I (column_name text, column_value text)', target_table); SELECT format('INSERT INTO %I (column_name, column_value)SELECT unnest(array[% s]) AS column_name, unnest(array[% s]) AS column_value FROM % I', target_table, string_agg(quote_literal(attname), ','), string_agg(quote_ident(attname) || '::text', ','), source_table)INTO dynamic_sql FROM pg_attribute WHERE attrelid = (SELECT oid FROM pg_class WHERE relname = source_table) AND attnum > 0 AND NOT attisdropped; EXECUTE dynamic_sql;END$$;").ToList();

            using (var context = new ApplicationIdentityDbContext())
            {
                var targetTable = "TransposeDatas";
                var sourceTable = "Assessments";
                var dynamicSql = $@"
                                    DO $$DECLARE
                                        target_table text := '{targetTable}';
                                        source_table text := '{sourceTable}';
                                        dynamic_sql text;
                                    BEGIN
                                        EXECUTE format('DROP TABLE IF EXISTS %I', target_table);
                                        EXECUTE format('CREATE TABLE %I (ColumnName text, ColumnValue text)', target_table);
                                
                                        SELECT format(
                                            'INSERT INTO %I (column_name, column_value)
                                            SELECT unnest(array[%s]) AS ColumnName, unnest(array[%s]) AS ColumnValue
                                            FROM %I',
                                            target_table,
                                            string_agg(quote_literal(attname), ','),
                                            string_agg(quote_ident(attname) || '::text', ','),
                                            source_table
                                        )
                                        INTO dynamic_sql
                                        FROM pg_attribute
                                        WHERE attrelid = (SELECT oid FROM pg_class WHERE relname = source_table)
                                            AND attnum > 0
                                            AND NOT attisdropped;
                                
                                        EXECUTE dynamic_sql;
                                    END$$;
                                ";

                context.Database.ExecuteSqlRaw(dynamicSql);
                
                // Query the transposed data from the target table
                var transposedData = context.TransposeDatas.ToList();

                // Return the transposed data
                return transposedData;
                // var targetTable = "transposed_data";
                // var sourceTable = "Assessments";
                // var dynamicSql = "";

                // // Drop the target table if it already exists
                // context.Database.ExecuteSqlRaw($"DROP TABLE IF EXISTS {targetTable}");

                // // Create the target table to store the transposed data
                // context.Database.ExecuteSqlRaw($"CREATE TABLE {targetTable} (column_name text, column_value tect)");

                // // Generate the dynamic SQL statement for transposing columns to rows
                // var query = context.TransposeDatas.FromSqlRaw(@"
                //     SELECT format(
                //         'INSERT INTO {0} (column_name, column_value)
                //         SELECT unnest(array[{1}]) AS column_name, unnest(array[{2}]::numeric[]) AS column_value
                //         FROM {3}',
                //         {0},
                //         string_agg(quote_literal(attname), ','),
                //         string_agg(quote_ident(attname) || '::text', ','),
                //         {4}
                //     )
                //     FROM pg_attribute
                //     WHERE attrelid = (
                //         SELECT oid FROM pg_class WHERE relname = {4}
                //     )
                //     AND attnum > 0
                //     AND NOT attisdropped",
                //     targetTable,
                //     sourceTable,
                //     sourceTable);

                // dynamicSql = query.Single().ToString();

                // // Execute the dynamic SQL statement to insert the transposed data
                // context.Database.ExecuteSqlRaw(dynamicSql);

                // // Query the transposed data from the target table
                // var transposedData = context.TransposeDatas.ToList();

                // // Return the transposed data
                // return transposedData;

            }
            // return tableNames;
        }
    }
}