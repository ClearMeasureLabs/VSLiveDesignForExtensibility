using System;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class ExpenseReportFactMap : IEntityFrameworkMapping
    {
        public EntityTypeBuilder Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<ExpenseReportFact>();
            mapping.UsePropertyAccessMode(PropertyAccessMode.Field);
            mapping.HasKey(x => x.Id);
            mapping.Property(x => x.Id).IsRequired()
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd()
                .HasDefaultValue(Guid.Empty);
            mapping.Property(x => x.Number).HasMaxLength(5);
            mapping.Property(x => x.TimeStamp);
            mapping.Property(x => x.Total);
            mapping.Property(x => x.Status);
            mapping.Property(x => x.Submitter);
            mapping.Property(x => x.Approver);
            return mapping;
        }
    }
}