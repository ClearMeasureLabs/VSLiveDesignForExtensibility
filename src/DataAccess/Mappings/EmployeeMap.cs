﻿using System;
using ClearMeasure.Bootcamp.Core.Model;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore;
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

        public void Map(ModelBuilder modelBuilder)
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