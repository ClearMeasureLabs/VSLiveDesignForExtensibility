using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Services;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportRepository : IExpenseReportRepository
    {
        private readonly EfCoreContext _context;

        public ExpenseReportRepository(EfCoreContext context)
        {
            _context = context;
        }

        public ExpenseReport[] GetMany(SearchSpecification specification)
        {
            var reports = _context.Set<ExpenseReport>()
                .Include(r => r.AuditEntries).ThenInclude(a => a.Employee)
                .Include(r => r.Submitter)
                .Include(r => r.Approver)
                .AsQueryable();

            if (specification.Approver != null)
                reports = reports.Where(r => r.Approver == specification.Approver);

            if (specification.Submitter != null)
                reports = reports.Where(r => r.Submitter == specification.Submitter);

            if (specification.Status != null)
                reports = reports.Where(r => r.StatusCode == specification.Status.Code);

            IList<ExpenseReport> list = reports.ToList();
            return list.ToArray();
        }

        public ExpenseReport GetSingle(string number)
        {
            var report = _context.Set<ExpenseReport>()
                .Include(r => r.AuditEntries).ThenInclude(a => a.Employee)
                .Include(r => r.Approver)
                .Include(r => r.Submitter)
                .Single(r => r.Number == number);
            return report;
        }

        public void Save(ExpenseReport expenseReport)
        {
            _context.Update(expenseReport);
            _context.SaveChanges();
        }
    }
}
