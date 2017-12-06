using System;
using System.Collections.Generic;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Services.Impl;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using NUnit.Framework;

namespace ClearMeasure.Bootcamp.IntegrationTests.Core.Features.Workflow
{
    [TestFixture]
    public class MostRecentExpenseReportFactViewIntegratedTester
    {
        private static void Setup()
        {
            var employee = new Employee("jpalermo", "Jeffrey", "Palermo", "jeffrey @ clear dash measure.com");

            var container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var context = container.GetInstance<EfCoreContext>();
            context.Update(employee);
            context.SaveChanges();


            var startingDate = new DateTime(1974, 8, 4);
            for (var i = 0; i < 25; i++)
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13 * i, startingDate.AddMinutes(i),
                    "Save Draft");
            for (var i = 0; i < 25; i++)
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13 * i, startingDate.AddMinutes(i),
                    "Save Draft", "Submit");
            for (var i = 0; i < 25; i++)
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13 * i, startingDate.AddMinutes(i),
                    "Save Draft", "Submit",
                    "Approve");
            for (var i = 0; i < 25; i++)
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13 * i, startingDate.AddMinutes(i),
                    "Save Draft", "Submit",
                    "Approve");
        }

        private static void RunToDraft(string number, Employee employee, int total, DateTime startingDate,
            params string[] commandsToRun)
        {
            var report = new ExpenseReport();
            report.Number = number;
            report.Status = ExpenseReportStatus.Draft;
            report.Submitter = employee;
            report.Approver = employee;
            report.Total = total;


            var container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var context = container.GetInstance<EfCoreContext>();
            context.Update(report);
            context.SaveChanges();


            var bus = container.GetInstance<Bus>();

            for (var j = 0; j < commandsToRun.Length; j++)
            {
                var timestamp = startingDate.AddSeconds(j);
                var command = new ExecuteTransitionCommand(report, commandsToRun[j], employee, timestamp);
                bus.Send(command);
            }
        }

        [Test]
        public void ShouldReproduceOwnedInstanceBug()
        {
            new DatabaseTester().Clean();
            var employee = new Employee("jpalermo", "Jeffrey", "Palermo", "jeffrey @ clear dash measure.com");
            using (var context = new DataContextFactory().GetContext())
            {
                context.Add(employee);
                context.SaveChanges();
            }

            var report = new ExpenseReport();
            report.Number = "123";
            report.Status = ExpenseReportStatus.Draft;
            report.Submitter = employee;
            report.Approver = employee;
            report.Total = 34;

            using (var context = new DataContextFactory().GetContext())
            {
                context.Update(report);
                context.SaveChanges();
            }

            var submitted = ExpenseReportStatus.Submitted;
            report.AddAuditEntry(
                new AuditEntry(employee, DateTime.Now, ExpenseReportStatus.Draft,
                    submitted, report));
            report.Status = submitted.Clone();

//            new DraftToSubmittedCommand().Execute(
//                new ExecuteTransitionCommand(report, "Submit", employee, DateTime.Now));

            using (var context = new DataContextFactory().GetContext())
            {
                context.Update(report);
                context.SaveChanges();
            }

            using (var context = new DataContextFactory().GetContext())
            {
                var statuses = new Dictionary<string, int>();
                context.ExecuteSql(
                    "select count(1), status from ExpenseReport group by status"
                    , reader => statuses.Add(reader.GetString(1), reader.GetInt32(0)));

                Assert.That(statuses["SBM"], Is.EqualTo(1));
            }
        }

        [Test, Explicit]
        public void ShouldReturnOnlyMostRecentExpenseReportFacts()
        {
            new DatabaseTester().Clean();

            Setup();

            var container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var context = container.GetInstance<EfCoreContext>();
            var statuses = new Dictionary<string, int>();
            context.ExecuteSql(
                "select count(1), status from MostRecentExpenseReportFactView group by status"
                , reader => statuses.Add(reader.GetString(1), reader.GetInt32(0)));

            Assert.That(statuses["Approved"], Is.EqualTo(50));
            Assert.That(statuses["Drafting"], Is.EqualTo(25));
            Assert.That(statuses["Submitted"], Is.EqualTo(25));
        }
    }
}