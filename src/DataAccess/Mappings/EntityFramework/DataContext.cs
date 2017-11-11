using ClearMeasure.Bootcamp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings.EntityFramework
{
    public class DataContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(Mappings.DataContext.GetTransactedSession().Connection.ConnectionString);
            

            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var employeeMap = modelBuilder.Entity<Employee>();
            employeeMap.UsePropertyAccessMode(PropertyAccessMode.Field);
            employeeMap.HasKey(x => x.Id);
            employeeMap.Property(x => x.Id).HasValueGenerator<SequentialGuidValueGenerator>().ValueGeneratedOnAdd();
            employeeMap.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            employeeMap.Property(x => x.FirstName).IsRequired().HasMaxLength(25);
            employeeMap.Property(x => x.LastName).IsRequired().HasMaxLength(25);
            employeeMap.Property(x => x.EmailAddress).IsRequired().HasMaxLength(100);
            employeeMap.HasDiscriminator<string>("Type").HasValue(typeof(Employee).FullName);
        }
    }
}