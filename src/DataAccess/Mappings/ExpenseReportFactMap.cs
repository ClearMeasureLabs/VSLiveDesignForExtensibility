using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class ExpenseReportFactMap : ClassMap<ExpenseReportFact>, IEntityFrameworkMapping
    {
        public ExpenseReportFactMap()
        {
            Not.LazyLoad();
            Id(x => x.Id).GeneratedBy.GuidComb();
            Map(x => x.Number).Length(5);
            Map(x => x.TimeStamp);
            Map(x => x.Total);
            Map(x => x.Status);
            Map(x => x.Submitter);
            Map(x => x.Approver);

            
        }

        public EntityTypeBuilder Map(ModelBuilder modelBuilder)
        {
            var mapping = modelBuilder.Entity<ExpenseReportFact>();
            mapping.UsePropertyAccessMode(PropertyAccessMode.Field);
            mapping.HasKey(x => x.Id);
            mapping.Property(x => x.Id).HasValueGenerator<SequentialGuidValueGenerator>().ValueGeneratedOnAdd();
            mapping.Property(x => x.Number).HasMaxLength(5);
            mapping.Property(x => x.TimeStamp);
            mapping.Property(x => x.Total);
            mapping.Property(x => x.Status);
            mapping.Property(x => x.Submitter);
            mapping.Property(x => x.Approver);
            mapping.Ignore(x => x.ExpenseReportId);
            return mapping;
        }
    }
}