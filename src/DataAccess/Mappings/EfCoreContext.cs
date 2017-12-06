using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Memory;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class EfCoreContext : DbContext
    {
        private readonly IDataConfiguration _config;

        public EfCoreContext(IDataConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            var connectionString = _config.GetConnectionString();
            optionsBuilder
                .UseSqlServer(connectionString)
                .ConfigureWarnings(warnings => warnings.Throw(RelationalEventId.QueryClientEvaluationWarning));

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new EmployeeMap().Map(modelBuilder);
            new ExpenseReportFactMap().Map(modelBuilder);
            new ExpenseReportMap().Map(modelBuilder);
            new ManagerMap().Map(modelBuilder);
            new AuditEntryMap().Map(modelBuilder);
            new ExpenseReportFactMap().Map(modelBuilder);
        }

        public void ExecuteSql(string commandText, Action<DbDataReader> readerAction)
        {
            using (var dbConnection = Database.GetDbConnection())
            {
                dbConnection.ConnectionString = _config.GetConnectionString();
                dbConnection.Open();
                using (var command = dbConnection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        commandText;
                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        readerAction(reader);
                    }
                }
                dbConnection.Close();
            }
        }
    }
}