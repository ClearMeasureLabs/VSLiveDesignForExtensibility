using System;
using System.Linq;
using ClearMeasure.Bootcamp.Core;
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
    public class ExpenseReportSaveCommandHandlerTester
    {
        [Test]
        public void ShouldSave()
        {
            new DatabaseTester().Clean();

            var creator = new Employee("1", "1", "1", "1");
            var assignee = new Employee("2", "2", "2", "2");
            var report = new ExpenseReport();
            report.Submitter = creator;
            report.Approver = assignee;
            report.Title = "foo";
            report.Description = "bar";
            report.ChangeStatus(ExpenseReportStatus.Approved);
            report.Number = "123";

            using(EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Add(creator);
                context.Add(assignee);
                context.SaveChanges();
            }

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            bus.Send(new ExpenseReportSaveCommand {ExpenseReport = report});

            ExpenseReport rehydratedReport;
            using(EfCoreContext context = new DataContextFactory().GetContext())
            {
                rehydratedReport = context.Find<ExpenseReport>(report.Id);
                context.Entry(rehydratedReport).Reference(x=>x.Submitter).Load();
                context.Entry(rehydratedReport).Reference(x=>x.Approver).Load();
            }

            rehydratedReport.Id.ShouldEqual(report.Id);
            rehydratedReport.Submitter.Id.ShouldEqual(report.Submitter.Id);
            rehydratedReport.Approver.Id.ShouldEqual(report.Approver.Id);
            rehydratedReport.Title.ShouldEqual(report.Title);
            rehydratedReport.Description.ShouldEqual(report.Description);
            rehydratedReport.Status.ShouldEqual(report.Status);
            
            rehydratedReport.Number.ShouldEqual(report.Number);
        }
        
        [Test]
        public void ShouldSaveAuditEntries()
        {
            new DatabaseTester().Clean();

            var creator = new Employee("1", "1", "1", "1");
            var assignee = new Employee("2", "2", "2", "2");
            var report = new ExpenseReport();
            report.Submitter = creator;
            report.Approver = assignee;
            report.Title = "foo";
            report.Description = "bar";
            report.ChangeStatus(ExpenseReportStatus.Approved);
            report.Number = "123";
            report.AddAuditEntry(new AuditEntry(creator, DateTime.Now,ExpenseReportStatus.Submitted,
                                                  ExpenseReportStatus.Approved, report));

            using(EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Add(creator);
                context.Add(assignee);
                context.SaveChanges();   
            }

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            bus.Send(new ExpenseReportSaveCommand { ExpenseReport = report });

            ExpenseReport rehydratedReport;
            using(EfCoreContext context = new DataContextFactory().GetContext())
            {
                rehydratedReport = context.Find<ExpenseReport>(report.Id);
                context.Entry(rehydratedReport).Collection(r=>r.AuditEntries).Load();
            }

            var x = report.AuditEntries.ToArray()[0];
            var y = rehydratedReport.AuditEntries.ToArray()[0];
            y.EndStatus.ShouldEqual(x.EndStatus);
            y.BeginStatus.ShouldEqual(x.BeginStatus);
            y.EmployeeName.ShouldEqual(x.EmployeeName);
        }

    }
}