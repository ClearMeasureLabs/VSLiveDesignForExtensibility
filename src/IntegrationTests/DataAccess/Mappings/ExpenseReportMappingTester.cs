using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using FluentNHibernate.Utils;
using Microsoft.EntityFrameworkCore;
using NHibernate;
using NUnit.Framework;
using Should;

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
            report.AddAuditEntry(new AuditEntry(submitter, DateTime.Now, ExpenseReportStatus.Submitted,
                                                  ExpenseReportStatus.Approved));
            
            using (ISession session = DataContextFactory.GetContext())
            {
                session.SaveOrUpdate(submitter);
                session.SaveOrUpdate(approver);
                session.SaveOrUpdate(report);
                session.Transaction.Commit();
            }

            ExpenseReport rehydratedExpenseReport;
            using (EfDataContext context = DataContextFactory.GetEfContext())
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
            report.AddAuditEntry(new AuditEntry(creator, DateTime.Now, ExpenseReportStatus.Submitted,
                ExpenseReportStatus.Approved));

            using (ISession session = DataContextFactory.GetContext())
            {
                session.SaveOrUpdate(creator);
                session.SaveOrUpdate(assignee);
                session.SaveOrUpdate(report);
                session.Transaction.Commit();
            }

            ExpenseReport rehydratedExpenseReport;
            using (ISession session2 = DataContextFactory.GetContext())
            {
                rehydratedExpenseReport = session2.Load<ExpenseReport>(report.Id);
            }

            var x = report.GetAuditEntries()[0];
            var y = rehydratedExpenseReport.GetAuditEntries()[0];
            Assert.That(x.EndStatus, Is.EqualTo(y.EndStatus));
        }
    }
}