using System;
using System.Data;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class EfDataContext : DbContext
    {
        private string _connectionString = "server=localhost\\SQLEXPRESS2014;database=ClearMeasure.Bootcamp;Integrated Security=true;";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString =
                _connectionString;
//                DataContextFactory.BuildConfiguration().GetProperty("connection.connection_string"); //ConfigurationManager.ConnectionStrings["Bootcamp"].ConnectionString;
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
                dbConnection.ConnectionString = _connectionString;
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