using ClearMeasure.Bootcamp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class EfDataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(DataContextFactory.GetContext().Connection.ConnectionString);

            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            new EmployeeMap().Map(modelBuilder);
            new ExpenseReportFactMap().Map(modelBuilder);
            new ExpenseReportMap().Map(modelBuilder);
            new ManagerMap().Map(modelBuilder);
        }
    }
}