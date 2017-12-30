using System;
using System.Linq;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NUnit.Framework;
using Should;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    [TestFixture]
    public class AddExpenseReportFactHandlerTester
    {
        [Test]
        public void ShouldCreateExpenseReportFact()
        {
            new DatabaseTester().Clean();

            var fact = new ExpenseReportFact()
            {
                Approver = "",
                Id = Guid.NewGuid(),
                Number = "1",
                Status = "Submitter",
                Submitter = "Me",
                TimeStamp = new DateTime(2015,01,01),
                Total = 123.456m,
            };
        
            var command = new AddExpenseReportFactCommand(fact);
            var handler = new AddExpenseReportFactHandler(new StubbedDataContextFactory().GetContext());
            handler.Handle(command);

            using (EfCoreContext context = new StubbedDataContextFactory().GetContext())
            {
                context.Set<ExpenseReportFact>().Count().ShouldEqual(1);
            }
        }
    }
}