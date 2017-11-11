using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NHibernate;
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
            using (ISession session = DataContext.GetTransactedSession())
            {
                session.Save(one);
                session.Transaction.Commit();
            }

            Employee rehydratedEmployee;
            using (ISession session = DataContext.GetTransactedSession())
            {
                rehydratedEmployee = session.Load<Employee>(one.Id);
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