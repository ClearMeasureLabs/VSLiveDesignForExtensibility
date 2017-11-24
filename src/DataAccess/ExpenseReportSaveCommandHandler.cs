using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportSaveCommandHandler : IRequestHandler<ExpenseReportSaveCommand, SingleResult<ExpenseReport>>
    {
        public SingleResult<ExpenseReport> Handle(ExpenseReportSaveCommand request)
        {
            using (Mappings.EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Update(request.ExpenseReport);
                context.SaveChanges();
            }

            return new SingleResult<ExpenseReport>(request.ExpenseReport);
        }
    }
}