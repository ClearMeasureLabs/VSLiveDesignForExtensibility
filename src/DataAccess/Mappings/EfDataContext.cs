using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
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
            Console.WriteLine($"%%%Connection string is: {connectionString}");
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

    public class MyDbConnection : DbConnection
    {
        private readonly IDbConnection _connection;
        private DbConnection _dbConnectionImplementation;

        public MyDbConnection(IDbConnection connection)
        {
            _connection = connection;
        }

        protected override DbTransaction BeginDbTransaction(IsolationLevel isolationLevel)
        {
            return _dbConnectionImplementation.BeginTransaction(isolationLevel);
        }

        public override void Close()
        {
            _dbConnectionImplementation.Close();
        }

        public override void ChangeDatabase(string databaseName)
        {
            _dbConnectionImplementation.ChangeDatabase(databaseName);
        }

        public override void Open()
        {
            _dbConnectionImplementation.Open();
        }

        public override string ConnectionString
        {
            get { return _dbConnectionImplementation.ConnectionString; }
            set { _dbConnectionImplementation.ConnectionString = value; }
        }

        public override string Database
        {
            get { return _dbConnectionImplementation.Database; }
        }

        public override ConnectionState State
        {
            get { return _dbConnectionImplementation.State; }
        }

        public override string DataSource
        {
            get { return _dbConnectionImplementation.DataSource; }
        }

        public override string ServerVersion
        {
            get { return _dbConnectionImplementation.ServerVersion; }
        }

        protected override DbCommand CreateDbCommand()
        {
            return _dbConnectionImplementation.CreateCommand();
        }
    }
}