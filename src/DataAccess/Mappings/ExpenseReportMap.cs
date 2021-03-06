﻿using System;
using System.Collections.Generic;
using ClearMeasure.Bootcamp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class ExpenseReportMap : IEntityFrameworkMapping
    {
        public EntityTypeBuilder Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<ExpenseReport>();
            mapping.UsePropertyAccessMode(PropertyAccessMode.Field);
            mapping.HasKey(x => x.Id);
            mapping.Property(x => x.Id).IsRequired()
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd()
                .HasDefaultValue(Guid.Empty);
            mapping.Property(x => x.Number).IsRequired().HasMaxLength(5);
            mapping.Property(x => x.Title).IsRequired().HasMaxLength(200);
            mapping.Property(x => x.Description).IsRequired().HasMaxLength(4000);
            mapping.Property<string>("StatusCode").HasColumnName("Status").HasColumnType("nchar(3)");
            mapping.Ignore(x => x.Status);
            mapping.Property(x => x.MilesDriven);
            mapping.Property(x => x.Created);
            mapping.Property(x => x.FirstSubmitted);
            mapping.Property(x => x.LastSubmitted);
            mapping.Property(x => x.LastWithdrawn);
            mapping.Property(x => x.LastCancelled);
            mapping.Property(x => x.LastApproved);
            mapping.Property(x => x.LastDeclined);
            mapping.Property(x => x.Total);

            mapping.HasOne(x => x.Submitter);
            mapping.HasOne(x => x.Approver);

            mapping.HasMany(x => x.AuditEntries)
                .WithOne(x=>x.ExpenseReport).IsRequired().OnDelete(DeleteBehavior.Cascade);
            
            return mapping;
        }
    }
}