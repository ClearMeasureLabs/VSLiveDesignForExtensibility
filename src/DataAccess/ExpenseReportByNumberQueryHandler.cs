using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportByNumberQueryHandler : IRequestHandler<ExpenseReportByNumberQuery,
        SingleResult<ExpenseReport>>
    {
        private readonly EfCoreContext _context;

        public ExpenseReportByNumberQueryHandler(EfCoreContext context)
        {
            _context = context;
        }

        public SingleResult<ExpenseReport> Handle(ExpenseReportByNumberQuery request)
        {
            var report = _context.Set<ExpenseReport>()
                .Include(r => r.AuditEntries).ThenInclude(a => a.Employee)
                .Include(r => r.Approver)
                .Include(r => r.Submitter)
                .Single(r => r.Number == request.ExpenseReportNumber);
            return new SingleResult<ExpenseReport>(report);
        }
    }
}