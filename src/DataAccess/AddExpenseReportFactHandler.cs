using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NHibernate;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class AddExpenseReportFactHandler : IRequestHandler<AddExpenseReportFactCommand, AddExpenseReportFactResult>
    {
        public AddExpenseReportFactResult Handle(AddExpenseReportFactCommand command)
        {
            using (IDbContext dbContext = DataContextFactory.GetContext())
            {
                dbContext.Save(command.ExpenseReportFact);
                dbContext.Transaction.Commit();
            }

            return new AddExpenseReportFactResult
            {
            };
        }
    }
}