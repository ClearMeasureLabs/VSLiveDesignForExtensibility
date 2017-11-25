using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using ClearMeasure.Bootcamp.Core.Services.Impl;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using StructureMap;

namespace ClearMeasure.Bootcamp.IntegrationTests.Core.Features.Workflow
{
    [TestFixture]
    public class MostRecentExpenseReportFactViewIntegratedTester
    { 
        [Test]
        public void ShouldReproduceOwnedInstanceBug()
        {
            new DatabaseTester().Clean();
            var employee = new Employee("jpalermo", "Jeffrey", "Palermo", "jeffrey @ clear dash measure.com");
            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Update(employee);
                context.SaveChanges();
            }

            var report = new ExpenseReport();
            report.Number = "123";
            report.Status = ExpenseReportStatus.Draft;
            report.Submitter = employee;
            report.Approver = employee;
            report.Total = 34;

            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Update(report);
                context.SaveChanges();
            }

            new DraftToSubmittedCommand().Execute(
                new ExecuteTransitionCommand(report, "Submit", employee, DateTime.Now));

            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Update(report);
                context.SaveChanges();
            }

            using (EfDataContext context = DataContextFactory.GetContext())
            {
                Dictionary<string, int> statuses = new Dictionary<string, int>();
                context.ExecuteSql(
                    "select count(1), status from MostRecentExpenseReportFactView group by status"
                    , reader => statuses.Add(reader.GetString(1), reader.GetInt32(0)));

                Assert.That(statuses["Submitted"], Is.EqualTo(1));
            }
        }

        [Test]
        public void ShouldReturnOnlyMostRecentExpenseReportFacts()
        {
            new DatabaseTester().Clean();

            Setup();

            using (EfDataContext context = DataContextFactory.GetContext())
            {
                Dictionary<string, int> statuses = new Dictionary<string, int>();
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText =
                        "select count(1), status from MostRecentExpenseReportFactView group by status";

                    DbDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        statuses.Add(reader.GetString(1), reader.GetInt32(0));
                    }
                }
                
                Assert.That(statuses["Approved"], Is.EqualTo(50));
                Assert.That(statuses["Drafting"], Is.EqualTo(25));
                Assert.That(statuses["Submitted"], Is.EqualTo(25));
            }

        }

        private static void Setup()
        {
            var employee = new Employee("jpalermo", "Jeffrey", "Palermo", "jeffrey @ clear dash measure.com");
            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Update(employee);
                context.SaveChanges();
            }

            var startingDate = new DateTime(1974, 8, 4);
            for (int i = 0; i < 25; i++)
            {
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13*i, startingDate.AddMinutes(i), "Save Draft");
            }
            for (int i = 0; i < 25; i++)
            {
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13*i, startingDate.AddMinutes(i), "Save Draft", "Submit");
            }
            for (int i = 0; i < 25; i++)
            {
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13*i, startingDate.AddMinutes(i), "Save Draft", "Submit",
                    "Approve");
            }
            for (int i = 0; i < 25; i++)
            {
                RunToDraft(new NumberGenerator().GenerateNumber(), employee, 13*i, startingDate.AddMinutes(i), "Save Draft", "Submit",
                    "Approve");
            }
        }

        private static void RunToDraft(string number, Employee employee, int total, DateTime startingDate, params string[] commandsToRun)
        {
            var report = new ExpenseReport();
            report.Number = number;
            report.Status = ExpenseReportStatus.Draft;
            report.Submitter = employee;
            report.Approver = employee;
            report.Total = total;

            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Update(report);
                context.SaveChanges();
            }

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();

            for (int j = 0; j < commandsToRun.Length; j++)
            {
                DateTime timestamp = startingDate.AddSeconds(j);
                var command = new ExecuteTransitionCommand(report, commandsToRun[j], employee, timestamp);
                bus.Send(command);
            }
        }
    }
}