using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using FluentNHibernate.Mapping;
using Microsoft.EntityFrameworkCore;

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

        public void Map(ModelBuilder modelBuilder)
        {
            
        }
    }
}