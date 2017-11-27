using System;
using System.Diagnostics;
using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using NUnit.Framework;
using Should;
using StructureMap;

namespace ClearMeasure.Bootcamp.IntegrationTests.Core.Features.Workflow
{
    [TestFixture]
    public class ExecuteTransitionCommandHandlerIntegratedTester
    {
        [Test]
        public void ShouldExecuteDraftTransition()
        {
            new DatabaseTester().Clean();

            var report = new ExpenseReport();
            report.Number = "123";
            report.Status = ExpenseReportStatus.Draft;
            var employee = new Employee("jpalermo", "Jeffrey", "Palermo", "jeffrey @ clear dash measure.com");
            report.Submitter = employee;
            report.Approver = employee;

            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.UpdateRange(employee, report);
                context.SaveChanges();
            }

            var command = new ExecuteTransitionCommand(report, "Save Draft", employee, new DateTime(2001, 1, 1));

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();

            ExecuteTransitionResult result = bus.Send(command);
            result.NewStatus.ShouldEqual("Drafting");
        }

        [Test]
        public void ShouldSaveWithMultipleInstancesOfEmployee()
        {
            new ZDataLoader().PopulateDatabase();
            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();

            Employee approver = bus.Send(new EmployeeByUserNameQuery(ZDataLoader.KnownEmployeeUsername)).Result;
            Employee submitter = bus.Send(new EmployeeByUserNameQuery(ZDataLoader.KnownEmployeeUsername)).Result;

            ExpenseReport expenseReport = new ExpenseReport();
            expenseReport.Number = ZDataLoader.KnownExpenseReportNumber;
            expenseReport.Submitter = submitter;
            expenseReport.Approver = approver;
            expenseReport.Title = "some title";
            expenseReport.Description = "some descriptioni";
            expenseReport.Total = 34;

            using (var context = DataContextFactory.GetEfContext())
            {
                context.Update(expenseReport);
                context.SaveChanges();
            }
        }

        [Test]
        public void ShouldPersistExportReportFact()
        {
            new DatabaseTester().Clean();
            var employee = new Employee("somethingelse", "Jeffrey", "Palermo", "jeffrey @ clear dash measure.com");
            employee.Id = Guid.NewGuid();
            var report = new ExpenseReport
            {
                Number = "123",
                Status = ExpenseReportStatus.Draft,
                Submitter = employee
            };

            DateTime setDate = new DateTime(2015, 1, 1);
            ExpenseReportFact expenseReportFact = new ExpenseReportFact(report, setDate);

            var command = new AddExpenseReportFactCommand(expenseReportFact);

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();

            bus.Send(command);

            ExpenseReportFact reHydratedExpenseReportFact;

            using (EfDataContext session = DataContextFactory.GetContext())
            {
                reHydratedExpenseReportFact = session.Find<ExpenseReportFact>(expenseReportFact.Id);
            }

            reHydratedExpenseReportFact.Approver.ShouldEqual(expenseReportFact.Approver);
            reHydratedExpenseReportFact.Number.ShouldEqual(expenseReportFact.Number);
            reHydratedExpenseReportFact.Status.ShouldEqual(expenseReportFact.Status);
            reHydratedExpenseReportFact.Submitter.ShouldEqual(expenseReportFact.Submitter);
            reHydratedExpenseReportFact.TimeStamp.ShouldEqual(expenseReportFact.TimeStamp);
            reHydratedExpenseReportFact.Total.ShouldEqual(expenseReportFact.Total);
        }

        [Test, Explicit]
        public void sample()
        {
            var matchingProcess = Process.GetProcessesByName("iisexpress").FirstOrDefault();
            if (matchingProcess != null && matchingProcess.StartInfo.Arguments.Contains("43507"))
            {
                matchingProcess.Kill();
            }
        }
}
}