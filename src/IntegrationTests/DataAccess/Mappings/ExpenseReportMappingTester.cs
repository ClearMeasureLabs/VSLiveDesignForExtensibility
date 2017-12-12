using System;
using System.Linq;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Should;
using StructureMap;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{
    [TestFixture]
    public class ExpenseReportMappingTester
    {
        [Test]
        public void ShouldSaveExpenseReportWithNewProperties()
        {
            // Clean the database
            new DatabaseTester().Clean();
            // Make employees
            var submitter = new Employee("1", "1", "1", "1");
            var approver = new Employee("2", "2", "2", "2");
            DateTime testTime = new DateTime(2015, 1, 1);
            // popluate ExpenseReport
            var report = new ExpenseReport
            {
                Submitter = submitter,
                Approver = approver,
                Title = "TestExpenseReport",
                Description = "This is an expense report test",
                Number = "123",
                MilesDriven = 100,
                Created = testTime,
                FirstSubmitted = testTime,
                LastSubmitted = testTime,
                LastWithdrawn = testTime,
                LastCancelled = testTime,
                LastApproved = testTime,
                LastDeclined = testTime,
                Total = 100.25m
            };

            report.ChangeStatus(ExpenseReportStatus.Approved);
            var auditEntry = new AuditEntry(submitter, DateTime.Now, ExpenseReportStatus.Submitted,
                ExpenseReportStatus.Approved, report);
            report.AddAuditEntry(auditEntry);
            
            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Add(submitter);
                context.Add(approver);
                context.Add(auditEntry);
                context.Add(report);
                context.SaveChanges();
            }

            ExpenseReport rehydratedExpenseReport;
            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                rehydratedExpenseReport = context.Set<ExpenseReport>().Include(x => x.Approver)
                    .Include(x => x.Submitter).Single(x => x.Id == report.Id);
            }

            rehydratedExpenseReport.Approver.ShouldEqual(approver);
            rehydratedExpenseReport.Submitter.ShouldEqual(submitter);
            rehydratedExpenseReport.Title.ShouldEqual("TestExpenseReport");
            rehydratedExpenseReport.Description.ShouldEqual("This is an expense report test");
            rehydratedExpenseReport.Number.ShouldEqual("123");
            rehydratedExpenseReport.Status.ShouldEqual(ExpenseReportStatus.Approved);
            Assert.That(rehydratedExpenseReport.MilesDriven, Is.EqualTo(report.MilesDriven));
            Assert.That(rehydratedExpenseReport.Created, Is.EqualTo(report.Created));
            Assert.That(rehydratedExpenseReport.FirstSubmitted, Is.EqualTo(report.FirstSubmitted));
            Assert.That(rehydratedExpenseReport.LastSubmitted, Is.EqualTo(report.LastSubmitted));
            Assert.That(rehydratedExpenseReport.LastWithdrawn, Is.EqualTo(report.LastWithdrawn));
            Assert.That(rehydratedExpenseReport.LastCancelled, Is.EqualTo(report.LastCancelled));
            Assert.That(rehydratedExpenseReport.LastApproved, Is.EqualTo(report.LastApproved));
            Assert.That(rehydratedExpenseReport.LastDeclined, Is.EqualTo(report.LastDeclined));
            Assert.That(rehydratedExpenseReport.Total, Is.EqualTo(report.Total));
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
            var auditEntry = new AuditEntry(creator, DateTime.Now, ExpenseReportStatus.Submitted,
                ExpenseReportStatus.Approved, report);
            report.AddAuditEntry(auditEntry);

            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Add(creator);
                context.Add(assignee);
                context.Add(auditEntry);
                context.Add(report);
                context.SaveChanges();
            }

            ExpenseReport rehydratedExpenseReport;
            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                rehydratedExpenseReport = context.Set<ExpenseReport>()
                    .Single(s => s.Id == report.Id);
                context.Entry<ExpenseReport>(rehydratedExpenseReport).Collection(x=>x.AuditEntries).Load();
            }

            var x1 = report.AuditEntries.ToArray()[0];
            var y1 = rehydratedExpenseReport.AuditEntries.ToArray()[0];
            Assert.That(y1.EndStatus, Is.EqualTo(x1.EndStatus));
        }

        [Test]
        public void ShouldCascadeDeleteAuditEntries()
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
            var auditEntry = new AuditEntry(creator, DateTime.Now, ExpenseReportStatus.Submitted,
                ExpenseReportStatus.Approved, report);
            report.AddAuditEntry(auditEntry);

            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Add(creator);
                context.Add(assignee);
                context.Add(auditEntry);
                context.Add(report);
                context.SaveChanges();
            }

            ExpenseReport rehydratedExpenseReport;
            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                rehydratedExpenseReport = context.Set<ExpenseReport>()
                    .Single(s => s.Id == report.Id);
                context.Entry(rehydratedExpenseReport).Collection(x => x.AuditEntries).Load();
            }

            rehydratedExpenseReport.AuditEntries.ToArray().Length.ShouldEqual(1);
            var entryId = rehydratedExpenseReport.AuditEntries.ToArray()[0].Id;

            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Remove(rehydratedExpenseReport);
                context.SaveChanges();
            }

            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Set<AuditEntry>().Count(entry => entry.Id == entryId).ShouldEqual(0);
                context.SaveChanges();
            }
        }

        [Test]
        public void ShouldPersistMultipleInstancesOfSameEmployee()
        {
            new DatabaseTester().Clean();
            var employee = new Employee("1", "1", "1", "1");
            var report = new ExpenseReport
            {
                Submitter = employee,
                Approver = employee,
                Title = "TestExpenseReport",
                Description = "This is an expense report test",
                Number = "123",
                Total = 100.25m
            };

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();

            EfCoreContext context = container.GetInstance<EfCoreContext>();
                context.Add(report);
                context.SaveChanges();

            Employee approver = container.GetInstance<EfCoreContext>().Find<Employee>(employee.Id);
            Employee submitter = container.GetInstance<EfCoreContext>().Find<Employee>(employee.Id);

            report.Approver = approver;
            report.Submitter = submitter;

            EfCoreContext context2 = container.GetInstance<EfCoreContext>();

            context2.Update(report);
                context2.SaveChanges();
        }
    }
}