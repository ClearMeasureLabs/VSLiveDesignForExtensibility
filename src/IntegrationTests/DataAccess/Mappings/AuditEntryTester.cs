using System;
using System.Linq;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Should;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{
    [TestFixture]
    public class AuditEntryTester
    {
        [Test]
        public void ShouldPersitAuditEntry()
        {
            // Clean the database
            new DatabaseTester().Clean();
            // Make employees
            var employee = new Employee("1", "1", "1", "1");
            DateTime testTime = new DateTime(2015, 1, 1);
            // popluate ExpenseReport
            var report = new ExpenseReport
            {
                Submitter = employee,
                Title = "TestExpenseReport",
                Description = "This is an expense report test",
                Number = "123",
                Total = 100.25m
            };
            var entry = new AuditEntry(employee, testTime, ExpenseReportStatus.Approved, ExpenseReportStatus.Cancelled, report);
            entry.ExpenseReport = report;

            
            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                context.Add(employee);
                context.Add(report);
                context.Add(entry);
                context.SaveChanges();
            }

            AuditEntry rehydratedEntry;
            using (EfCoreContext context = new DataContextFactory().GetContext())
            {
                rehydratedEntry = context.Set<AuditEntry>().Include(x => x.ExpenseReport)
                    .Include(x => x.Employee).Single(x => x.Id == entry.Id);
            }

            rehydratedEntry.Employee.ShouldEqual(employee);
            rehydratedEntry.ExpenseReport.ShouldEqual(report);
            rehydratedEntry.BeginStatus.ShouldEqual(ExpenseReportStatus.Approved);
            rehydratedEntry.EndStatus.ShouldEqual(ExpenseReportStatus.Cancelled);
            rehydratedEntry.Date.ShouldEqual(testTime);
        }
    }
}