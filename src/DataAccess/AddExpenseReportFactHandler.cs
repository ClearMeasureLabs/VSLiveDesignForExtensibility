using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class AddExpenseReportFactHandler : IRequestHandler<AddExpenseReportFactCommand, AddExpenseReportFactResult>
    {
        public AddExpenseReportFactResult Handle(AddExpenseReportFactCommand command)
        {
            using (EfDataContext dbContext = DataContextFactory.GetContext())
            {
                dbContext.Add(command.ExpenseReportFact);
                dbContext.SaveChanges();
            }

            return new AddExpenseReportFactResult();
        }
    }
}