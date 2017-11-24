using System.Collections.Generic;
using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.SearchExpenseReports;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportSpecificationQueryHandler : IRequestHandler<ExpenseReportSpecificationQuery, MultipleResult<ExpenseReport>>
    {
        public MultipleResult<ExpenseReport> Handle(ExpenseReportSpecificationQuery command)
        {
            using (EfDataContext dbContext = DataContextFactory.GetContext())
            {
                IQueryable<ExpenseReport> reports = dbContext.Set<ExpenseReport>().AsQueryable();

                if (command.Approver != null)
                {
                    reports = reports.Where(r => r.Approver == command.Approver);
                }

                if (command.Submitter != null)
                {
                    reports = reports.Where(r => r.Submitter == command.Submitter);
                }

                if (command.Status != null)
                {
                    reports = reports.Where(r => r.Status.Equals(command.Status));
                }

                IList<ExpenseReport> list = reports.ToList();
                return new MultipleResult<ExpenseReport> {Results = new List<ExpenseReport>(list).ToArray()};
            }


        }
    }
}