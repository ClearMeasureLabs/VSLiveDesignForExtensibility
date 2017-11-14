using System;
using ClearMeasure.Bootcamp.Core.Model;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class EmployeeMap : ClassMap<Employee>, IEntityFrameworkMapping
    {
        public EmployeeMap()
        {
            Not.LazyLoad();
            Access.CamelCaseField(CamelCasePrefix.Underscore);
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.UserName).Not.Nullable().Length(50);
            Map(x => x.FirstName).Not.Nullable().Length(25);
            Map(x => x.LastName).Not.Nullable().Length(25);
            Map(x => x.EmailAddress).Not.Nullable().Length(100);
            DiscriminateSubClassesOnColumn("Type");
        }

        public EntityTypeBuilder Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<Employee>();
            mapping.UsePropertyAccessMode(PropertyAccessMode.Field);
            mapping.HasKey(x => x.Id);
            mapping.Property(x => x.Id).HasValueGenerator<SequentialGuidValueGenerator>().ValueGeneratedOnAdd();
            mapping.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            mapping.Property(x => x.FirstName).IsRequired().HasMaxLength(25);
            mapping.Property(x => x.LastName).IsRequired().HasMaxLength(25);
            mapping.Property(x => x.EmailAddress).IsRequired().HasMaxLength(100);
            mapping.HasDiscriminator<string>("Type").HasValue(typeof(Employee).FullName);

            return mapping;
        }
    }
}