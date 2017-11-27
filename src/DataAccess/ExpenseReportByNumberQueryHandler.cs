using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportByNumberQueryHandler : IRequestHandler<ExpenseReportByNumberQuery, SingleResult<ExpenseReport>>
    {
        public SingleResult<ExpenseReport> Handle(ExpenseReportByNumberQuery request)
        {
            using (EfDataContext dbContext = DataContextFactory.GetContext())
            {
                ExpenseReport report = dbContext.Set<ExpenseReport>()
                    .Include(r => r.AuditEntries).ThenInclude(a=>a.Employee)
                    .Include(r => r.Approver)
                    .Include(r=>r.Submitter)    
                    .Single(r => r.Number == request.ExpenseReportNumber);
                return new SingleResult<ExpenseReport>(report);
            }
        }
    }
}