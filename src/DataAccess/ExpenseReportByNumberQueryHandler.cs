using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportByNumberQueryHandler : IRequestHandler<ExpenseReportByNumberQuery, SingleResult<ExpenseReport>>
    {
        public SingleResult<ExpenseReport> Handle(ExpenseReportByNumberQuery request)
        {
            using (EfDataContext dbContext = DataContextFactory.GetContext())
            {
                var reports = dbContext.Set<ExpenseReport>().AsQueryable();
                reports = reports.Where(r => r.Number == request.ExpenseReportNumber);
                var report = reports.Single();
                dbContext.Entry(report).Collection(r=>r.AuditEntries).Load();
                return new SingleResult<ExpenseReport>(report);
            }
        }
    }
}