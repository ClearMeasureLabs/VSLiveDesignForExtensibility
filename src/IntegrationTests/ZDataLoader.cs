using System;
using System.IO;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]
namespace ClearMeasure.Bootcamp.IntegrationTests
{
    [TestFixture]
    public class ZDataLoader
    {
        public const string KnownExpenseReportNumber = "1A2B3";
        public const string KnownEmployeeUsername = "jpalermo";

        [Test, Category("DataLoader")]
        public void PopulateDatabase()
        {
            new DatabaseTester().Clean();
            EfCoreContext session = new StubbedDataContextFactory().GetContext();

            //Trainer1
            var jpalermo = new Employee(KnownEmployeeUsername, "Jeffrey", "Palermo", "jeffrey@clear-measure.com");
            session.Add(jpalermo);

            //Person 1
            var newPerson = new Employee("myUserName", "First", "Last", "somefakeemail@gmail.com");
            session.Add(newPerson);

            var coolGuy = new Employee("CoolestGuy", "Corey", "Kobayashi", "Coolguysrus@gmail.com");
            session.Add(coolGuy);

            var isaiah = new Employee("tabach", "Isaiah", "Tabach", "isaiah.tabach@gmail.com");
            session.Add(isaiah);

            var malachi = new Employee("Malachi", "Malachi", "Mayfield", "mal@gmail.com");
            session.Add(malachi);

            var truckertom = new Employee("Trucker Tom", "Trucker", "Tom", "hiNathaniel@gmail.com");
            session.Add(truckertom);

            var carson = new Employee("carson", "Carson", "Williams", "Randomemail@gmail.com");
            session.Add(carson);

            session.Add(new Employee("mitch", "mitchell", "garrett", "garrett@gmail.com"));

            var thing1 = new Employee("Thing1", "Emma", "Driscoll", "Thing1@gmail.com");
            session.Add(thing1);

            var bettertwin = new Employee("Bettertwin","Lily","Driscoll", "blubber@gmail.com");
            session.Add(bettertwin);

            var austin = new Employee("Austin", "austin", "hale", "austin@gmail.com");
            session.Add(austin);

            var knuckles = new Employee("Knuckles", "Da", "Wae", "Uganda@gmail.com");
            session.Add(knuckles);

            var gibson = new Employee("Gibson", "Miles", "Gibson", "doyouknowdawey@gmail.com");
            session.Add(gibson);

            var jb = new Employee("jb", "john", "burshnick", "wristersl@ltisdschools.org");
            session.Add(jb);

            var grandmasterflash = new Employee("grandmasterC", "Cody", "Miesse", "rappersdelight@gmail.com");
            session.Add(grandmasterflash);

            var jonb1 = new Employee("jonb1", "Jon", "Ballard", "miesse.cody@ltisdschools.net");
            session.Add(jonb1);

            var elle = new Employee("ellemhansen", "elle", "hansen", "elle@gmail.com");
            session.Add(elle);

            var carter = new Employee("carter", "carter", "nokes", "carter@gmail.com");
            session.Add(carter);

            var sd = new Employee("sd", "stefan", "deitrich", "emaill@gmail.com");
            session.Add(sd);

            var jluetz = new Employee("jluetz", "john", "luetzelschwab", "luetzalt@gmail.com");
            session.Add(jluetz);

            var pitifulPlatapus99 = new Employee("PitifulPlatapus99", "Ian", "Trowbridge", "iakt99@gmail.com");
            session.Add(pitifulPlatapus99);

            var didi = new Employee("didizhou", "didi", "zhou", "email1@gmail.com");
            session.Add(didi);

            session.Add(new Employee("theRealTruckerTom", "Nathaniel", "Plaxton", "truckerTom@gmail.com"));

            var seth = new Employee("seth", "Seth", "Forman", "gmail@seth.com");
            session.Add(seth);

            var tp = new Employee("TP", "Thomas", "Plaxton", "plaxton.thomas@ltisdschools.net");
            session.Add(tp);
        //Person 2

            //Person 3


            //etc

            var damian = new Employee("damian", "Damian", "Brady", "damian@Gmail.com");
            session.Add(damian);

            var hsimpson = new Employee("hsimpson", "Homer", "Simpson", "homer@simpson.com");
            session.Add(hsimpson);

            var paul = new Employee("paul", "Paul", "Stovell", "Paul@myemail.com");
            session.Add(paul);

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