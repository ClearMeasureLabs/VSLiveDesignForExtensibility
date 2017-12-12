using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportSaveCommandHandler : IRequestHandler<ExpenseReportSaveCommand, SingleResult<ExpenseReport>
    >
    {
        private readonly EfCoreContext _context;

        public ExpenseReportSaveCommandHandler(EfCoreContext context)
        {
            _context = context;
        }

        public SingleResult<ExpenseReport> Handle(ExpenseReportSaveCommand request)
        {
            _context.Update(request.ExpenseReport);
            _context.SaveChanges();

            return new SingleResult<ExpenseReport>(request.ExpenseReport);
        }
    }
}