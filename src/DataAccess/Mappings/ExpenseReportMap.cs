using System.Collections.Generic;
using ClearMeasure.Bootcamp.Core.Model;
using FluentNHibernate;
using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class ExpenseReportMap : ClassMap<ExpenseReport>, IEntityFrameworkMapping
    {
        public ExpenseReportMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Number).Not.Nullable().Length(5);
            Map(x => x.Title).Not.Nullable().Length(200);
            Map(x => x.Description).Not.Nullable().Length(4000);
            Map(x => x.Status).Not.Nullable().CustomType<ExpenseReportStatusType>();
            // New property mappings
            Map(x => x.MilesDriven).Column("MilesDriven");
            Map(x => x.Created).Column("Created");
            Map(x => x.FirstSubmitted).Column("FirstSubmitted");
            Map(x => x.LastSubmitted).Column("LastSubmitted");
            Map(x => x.LastWithdrawn).Column("LastWithdrawn");
            Map(x => x.LastCancelled).Column("LastCancelled");
            Map(x => x.LastApproved).Column("LastApproved");
            Map(x => x.LastDeclined).Column("LastDeclined");
            Map(x => x.Total).Column("Total");

            References(x => x.Submitter).Column("SubmitterId");
            References(x => x.Approver).Column("ApproverId");

            HasMany(Reveal.Member<ExpenseReport, IEnumerable<AuditEntry>>("_auditEntries"))
                .AsList(part =>
                {
                    part.Column("Sequence");
                    part.Type<int>();
                })
                .Table("AuditEntry")
                .Cascade.AllDeleteOrphan()
                .KeyColumn("ExpenseReportId")
                .Component(part =>
                {
                    part.References(x => x.Employee).Column("EmployeeId");
                    part.Map(x => x.EmployeeName).Length(200);
                    part.Map(x => x.Date);
                    part.Map(x => x.EndStatus).CustomType<ExpenseReportStatusType>();
                    part.Map(x => x.BeginStatus).CustomType<ExpenseReportStatusType>();
                })
                .Access.CamelCaseField()
                .Not.LazyLoad();
        }

        public EntityTypeBuilder Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<ExpenseReport>();
            mapping.UsePropertyAccessMode(PropertyAccessMode.Field);
            mapping.HasKey(x => x.Id);
            mapping.Property(x => x.Id).HasValueGenerator<SequentialGuidValueGenerator>().ValueGeneratedOnAdd();
            mapping.Property(x => x.Number).IsRequired().HasMaxLength(5);
            mapping.Property(x => x.Title).IsRequired().HasMaxLength(200);
            mapping.Property(x => x.Description).IsRequired().HasMaxLength(4000);
            mapping.OwnsOne(x => x.Status, builder =>
            {
                builder.Property<string>(x => x.Code).HasColumnName("Status").HasColumnType("nchar(3)");
                builder.Ignore(x => x.FriendlyName).Ignore(x => x.Key).Ignore(x => x.SortBy);
            });

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


            return mapping;
        }
    }
}