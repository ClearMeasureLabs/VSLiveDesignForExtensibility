using System;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]
namespace ClearMeasure.Bootcamp.IntegrationTests
{
    [TestFixture, Explicit]
    public class ZDataLoader
    {
        public const string KnownExpenseReportNumber = "1A2B3";
        public const string KnownEmployeeUsername = "jpalermo";
        [Test, Category("DataLoader")]
        public void PopulateDatabase()
        {
            new DatabaseTester().Clean();
            EfCoreContext session = new DataContextFactory().GetContext();


            //Trainer1
            var jpalermo = new Employee(KnownEmployeeUsername, "Jeffrey", "Palermo", "jeffrey@clear-measure.com");
            session.Add(jpalermo);

            //Person 1
            
            //Person 2
            
            //Person 3
            var damian = new Employee("damian", "Damian", "Brady", "damian@Gmail.com");
            session.Add(damian);
            
            //Person 4
            
            //Person 5

            //Person 6
            var paul = new Employee("paul", "Paul", "Stovell", "Paul@myemail.com");
            session.Add(paul);
            
            //Person 7
            
            //Person 8
            
            //Person 9

            //Person 10

            //Person 11

            //Person 12

            //Person 13

            var hsimpson = new Employee("hsimpson", "Homer", "Simpson", "homer@simpson.com");
            session.Add(hsimpson);

            foreach (ExpenseReportStatus status in ExpenseReportStatus.GetAllItems())
            {
                var report = new ExpenseReport();
                report.Number = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                report.Submitter = jpalermo;
                report.Approver = jpalermo;
                report.Status = status;
                report.Title = "Expense report starting in status " + status;
                report.Description = "Foo, foo, foo, foo " + status;
                new DateTime(2000, 1, 1, 8, 0, 0);
                report.ChangeStatus(jpalermo, DateTime.Now, ExpenseReportStatus.Draft, ExpenseReportStatus.Draft);
                report.ChangeStatus(jpalermo, DateTime.Now, ExpenseReportStatus.Draft, ExpenseReportStatus.Submitted);
                report.ChangeStatus(jpalermo, DateTime.Now, ExpenseReportStatus.Submitted, ExpenseReportStatus.Approved);

                session.Add(report);
            }

            var report2 = new ExpenseReport();
            report2.Number = KnownExpenseReportNumber;
            report2.Submitter = jpalermo;
            report2.Approver = jpalermo;
            report2.Status = ExpenseReportStatus.Draft;
            report2.Title = "Expense report starting in Draft status ";
            report2.Description = "Foo, foo, foo, foo ";
            new DateTime(2000, 1, 1, 8, 0, 0);
            session.Add(report2);

            session.SaveChanges();
            session.Dispose();
        }
    }
}