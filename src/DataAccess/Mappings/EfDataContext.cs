using System;
using System.Configuration;
using System.Diagnostics;
using ClearMeasure.Bootcamp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Microsoft.Extensions.Logging;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class EfDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = DataContextFactory.BuildConfiguration().GetProperty("connection.connection_string"); //ConfigurationManager.ConnectionStrings["Bootcamp"].ConnectionString;
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
    }
}