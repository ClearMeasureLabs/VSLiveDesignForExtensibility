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
            mapping.OwnsOne(x => x.BeginStatus, builder =>
            {
                builder.Property<string>(x => x.Code).HasColumnName("BeginStatus").HasColumnType("nchar(3)");
                builder.Ignore(x => x.FriendlyName).Ignore(x => x.Key).Ignore(x => x.SortBy);
                builder.Property(typeof(Guid), "AuditEntryId").HasDefaultValue(Guid.Empty); 
            });
            mapping.OwnsOne(x => x.EndStatus, builder =>
            {
                builder.Property<string>(x => x.Code).HasColumnName("EndStatus").HasColumnType("nchar(3)");
                builder.Ignore(x => x.FriendlyName).Ignore(x => x.Key).Ignore(x => x.SortBy);
                builder.Property(typeof(Guid), "AuditEntryId").HasDefaultValue(Guid.Empty); 
            });
            mapping.Property(x => x.Date);
            mapping.HasOne(x => x.Employee);

            return mapping;
        }
    }
}