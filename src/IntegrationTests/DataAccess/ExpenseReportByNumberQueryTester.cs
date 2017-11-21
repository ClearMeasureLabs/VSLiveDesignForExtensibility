using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using NHibernate;
using NUnit.Framework;
using StructureMap;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    [TestFixture]
    public class ExpenseReportByNumberQueryTester
    {
        [Test]
        public void ShouldGetByNumber()
        {
            new DatabaseTester().Clean();

            var employee = new Employee("1", "1", "1", "1");
            var report1 = new ExpenseReport();
            report1.Submitter = employee;
            report1.Number = "123";
            var report2 = new ExpenseReport();
            report2.Submitter = employee;
            report2.Number = "456";

            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Add(employee);
                context.Add(report1);
                context.Add(report2);
                context.SaveChanges();
            }

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();

            ExpenseReport report123 = bus.Send(new ExpenseReportByNumberQuery {ExpenseReportNumber = "123"}).Result;
            ExpenseReport report456 = bus.Send(new ExpenseReportByNumberQuery {ExpenseReportNumber = "456"}).Result;

            Assert.That(report123.Id, Is.EqualTo(report1.Id));
            Assert.That(report456.Id, Is.EqualTo(report2.Id));
        }

    }
}