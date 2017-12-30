using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

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
    }
}