using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class AddExpenseReportFactHandler : IRequestHandler<AddExpenseReportFactCommand, AddExpenseReportFactResult>
    {
        private readonly EfCoreContext _context;

        public AddExpenseReportFactHandler(EfCoreContext context)
        {
            _context = context;
        }

        public AddExpenseReportFactResult Handle(AddExpenseReportFactCommand command)
        {
            _context.Add(command.ExpenseReportFact);
            _context.SaveChanges();

            return new AddExpenseReportFactResult();
        }
    }
}