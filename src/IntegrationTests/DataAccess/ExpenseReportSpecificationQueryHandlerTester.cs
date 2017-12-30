using System;
using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.SearchExpenseReports;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using NUnit.Framework;
using Should;
using StructureMap;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    [TestFixture]
    public class ExpenseReportSpecificationQueryHandlerTester
    {
        [Test]
        public void ShouldSearchBySpecificationWithAssignee()
        {
            new DatabaseTester().Clean();

            var employee1 = new Employee("1", "1", "1", "1");
            var employee2 = new Employee("2", "2", "2", "2");
            var order1 = new ExpenseReport();
            order1.Submitter = employee2;
            order1.Approver = employee1;
            order1.Number = "123";
            var order2 = new ExpenseReport();
            order2.Submitter = employee1;
            order2.Approver = employee2;
            order2.Number = "456";

            using (EfCoreContext dbContext = new StubbedDataContextFactory().GetContext())
            {
                dbContext.Add(employee1);
                dbContext.Add(employee2);
                dbContext.Add(order1);
                dbContext.Add(order2);
                dbContext.SaveChanges();
            }

            var specification = new ExpenseReportSpecificationQuery {Approver = employee1};

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            MultipleResult<ExpenseReport> result = bus.Send(specification);
            ExpenseReport[] reports = result.Results;

            Assert.That(reports.Length, Is.EqualTo(1));
            Assert.That(reports[0].Id, Is.EqualTo(order1.Id));
        }

        [Test]
        public void ShouldSearchBySpecificationWithCreator()
        {
            new DatabaseTester().Clean();

            var creator1 = new Employee("1", "1", "1", "1");
            var creator2 = new Employee("2", "2", "2", "2");
            var order1 = new ExpenseReport();
            order1.Submitter = creator1;
            order1.Number = "123";
            var order2 = new ExpenseReport();
            order2.Submitter = creator2;
            order2.Number = "456";

            using (EfCoreContext dbContext = new StubbedDataContextFactory().GetContext())
            {
                dbContext.Add(creator1);
                dbContext.Add(creator2);
                dbContext.Add(order1);
                dbContext.Add(order2);
                dbContext.SaveChanges();
            }

            var specification = new ExpenseReportSpecificationQuery{Submitter = creator1};

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            MultipleResult<ExpenseReport> result = bus.Send(specification);
            ExpenseReport[] reports = result.Results;

            Assert.That(reports.Length, Is.EqualTo(1));
            Assert.That(reports[0].Id, Is.EqualTo(order1.Id));
        }

        [Test]
        public void ShouldSearchBySpecificationWithFullSpecification()
        {
            new DatabaseTester().Clean();

            var employee1 = new Employee("1", "1", "1", "1");
            var employee2 = new Employee("2", "2", "2", "2");
            var order1 = new ExpenseReport();
            order1.Submitter = employee2;
            order1.Approver = employee1;
            order1.Number = "123";
            order1.Status = ExpenseReportStatus.Submitted;
            var order2 = new ExpenseReport();
            order2.Submitter = employee1;
            order2.Approver = employee2;
            order2.Number = "456";
            order2.Status = ExpenseReportStatus.Draft;

            using(EfCoreContext dbContext = new StubbedDataContextFactory().GetContext())
            {

                dbContext.Add(employee1);
                dbContext.Add(employee2);
                dbContext.Add(order1);
                dbContext.Add(order2);
                dbContext.SaveChanges();
            }

            var specification = new ExpenseReportSpecificationQuery()
            {
                Submitter = employee2,
                Approver = employee1,
                Status = ExpenseReportStatus.Submitted
            };

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            MultipleResult<ExpenseReport> result = bus.Send(specification);
            ExpenseReport[] reports = result.Results;

            Assert.That(reports.Length, Is.EqualTo(1));
            Assert.That(reports[0].Id, Is.EqualTo(order1.Id));
        }

        [Test]
        public void ShouldSearchBySpecificationWithStatus()
        {
            new DatabaseTester().Clean();

            var employee1 = new Employee("1", "1", "1", "1");
            var employee2 = new Employee("2", "2", "2", "2");
            var order1 = new ExpenseReport();
            order1.Submitter = employee2;
            order1.Approver = employee1;
            order1.Number = "123";
            order1.Status = ExpenseReportStatus.Submitted;
            var order2 = new ExpenseReport();
            order2.Submitter = employee1;
            order2.Approver = employee2;
            order2.Number = "456";
            order2.Status = ExpenseReportStatus.Draft;

            using (EfCoreContext dbContext = new StubbedDataContextFactory().GetContext())
            {

                dbContext.Add(employee1);
                dbContext.Add(employee2);
                dbContext.Add(order1);
                dbContext.Add(order2);
                dbContext.SaveChanges();
            }

            var specification = new ExpenseReportSpecificationQuery()
            {
                Status = ExpenseReportStatus.Submitted
            };

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            MultipleResult<ExpenseReport> result = bus.Send(specification);
            ExpenseReport[] reports = result.Results;

            Assert.That(reports.Length, Is.EqualTo(1));
            Assert.That(reports[0].Id, Is.EqualTo(order1.Id));
        }

        [Test]
        public void ShouldEagerFetchAssociations()
        {
            new DatabaseTester().Clean();

            var employee1 = new Employee("1", "1", "1", "1");
            var employee2 = new Employee("2", "2", "2", "2");
            var report = new ExpenseReport();
            report.Submitter = employee1;
            report.Approver = employee1;
            report.Number = "123";
            report.ChangeStatus(employee2, DateTime.Now, ExpenseReportStatus.Draft, ExpenseReportStatus.Submitted);;

            using (EfCoreContext dbContext = new StubbedDataContextFactory().GetContext())
            {

                dbContext.Add(employee1);
                dbContext.Add(report);
                dbContext.SaveChanges();
            }

            var specification = new ExpenseReportSpecificationQuery()
            {
                Status = ExpenseReportStatus.Submitted
            };

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            MultipleResult<ExpenseReport> result = bus.Send(specification);
            ExpenseReport[] reports = result.Results;

            Assert.That(reports.Length, Is.EqualTo(1));
            Assert.That(reports[0].Id, Is.EqualTo(report.Id));
            reports[0].Submitter.ShouldEqual(employee1);
            reports[0].Approver.ShouldEqual(employee1);
            reports[0].AuditEntries.ToArray()[0].Employee.ShouldEqual(employee2);
        }
    }
}