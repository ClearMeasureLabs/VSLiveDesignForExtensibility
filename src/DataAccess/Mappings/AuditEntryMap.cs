using System;
using ClearMeasure.Bootcamp.Core.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class AuditEntryMap : IEntityFrameworkMapping
    {
        public EntityTypeBuilder Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<AuditEntry>().ToTable("EfAuditEntry");
            mapping.UsePropertyAccessMode(PropertyAccessMode.Field);
            mapping.HasKey(x => x.Id);
            mapping.Property(x => x.Id).IsRequired()
                .HasValueGenerator<SequentialGuidValueGenerator>()
                .ValueGeneratedOnAdd()
                .HasDefaultValue(Guid.Empty);
            mapping.Property<string>("BeginStatusCode").HasColumnName("BeginStatus").HasColumnType("nchar(3)"); 
            mapping.Property<string>("EndStatusCode").HasColumnName("EndStatus").HasColumnType("nchar(3)");
            mapping.Property(x => x.Date);
            mapping.HasOne(x => x.Employee);
            mapping.Ignore(x => x.BeginStatus);
            mapping.Ignore(x => x.EndStatus);

            return mapping;
        }
    }
}