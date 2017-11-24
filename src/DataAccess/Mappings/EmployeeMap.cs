using System;
using ClearMeasure.Bootcamp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class EmployeeMap : IEntityFrameworkMapping
    {
        public EntityTypeBuilder Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<Employee>();
            mapping.UsePropertyAccessMode(PropertyAccessMode.Field);
            mapping.HasKey(x => x.Id);
            mapping.Property(x => x.Id).IsRequired().HasValueGenerator<SequentialGuidValueGenerator>().ValueGeneratedOnAdd();
            mapping.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            mapping.Property(x => x.FirstName).IsRequired().HasMaxLength(25);
            mapping.Property(x => x.LastName).IsRequired().HasMaxLength(25);
            mapping.Property(x => x.EmailAddress).IsRequired().HasMaxLength(100);
            mapping.HasDiscriminator<string>("Type").HasValue(typeof(Employee).FullName);

            return mapping;
        }
    }
}