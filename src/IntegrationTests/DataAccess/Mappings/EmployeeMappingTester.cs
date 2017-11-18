using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NUnit.Framework;
using Should;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{
    [TestFixture]
    public class EmployeeMappingTester
    {
        [Test]
        public void ShouldPersist()
        {
            new DatabaseTester().Clean();

            var one = new Employee("1", "first1", "last1", "email1");
            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Add(one);
                context.SaveChanges();
            }

            Employee rehydratedEmployee;
            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                rehydratedEmployee = context.Find<Employee>(one.Id);
            }

            rehydratedEmployee.UserName.ShouldEqual(one.UserName);
            rehydratedEmployee.FirstName.ShouldEqual(one.FirstName);
            rehydratedEmployee.LastName.ShouldEqual(one.LastName);
            rehydratedEmployee.EmailAddress.ShouldEqual(one.EmailAddress);
        }

        [Test]
        public void EfCoreMappingShouldPersist()
        {
            new DatabaseTester().Clean();

            var one = new Employee("1", "first1", "last1", "email1");
            using (EfDataContext context = new EfDataContext())
            {
                context.Add(one);
                context.SaveChanges();
            }

            Employee rehydratedEmployee;
            using (EfDataContext context = new EfDataContext())
            {
                rehydratedEmployee = context.Find<Employee>(one.Id);
            }

            rehydratedEmployee.UserName.ShouldEqual(one.UserName);
            rehydratedEmployee.FirstName.ShouldEqual(one.FirstName);
            rehydratedEmployee.LastName.ShouldEqual(one.LastName);
            rehydratedEmployee.EmailAddress.ShouldEqual(one.EmailAddress);
        }
    }
}