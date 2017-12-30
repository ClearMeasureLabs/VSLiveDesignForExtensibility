using System.Linq;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using Should;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{
    [TestFixture]
    public class ManagerMappingTester
    {
        [Test]
        public void ShouldPersist()
        {
            new DatabaseTester().Clean();

            var one = new Manager("username", "Endurance", "Idehen", "Email");
            Employee adminAssistant = new Employee("Assistant", "Someone", "Else", "Email2");
            one.AdminAssistant = adminAssistant;
            using (EfCoreContext context = new StubbedDataContextFactory().GetContext())
            {
                context.Add(one);
                context.Add(adminAssistant);
                context.SaveChanges();
            }

            Manager rehydratedEmployee;
            using (EfCoreContext context = new StubbedDataContextFactory().GetContext())
            {
                rehydratedEmployee = context.Set<Manager>().Include(x => x.AdminAssistant).Single(x => x.Id == one.Id);
            }

            rehydratedEmployee.UserName.ShouldEqual(one.UserName);
            rehydratedEmployee.FirstName.ShouldEqual(one.FirstName);
            rehydratedEmployee.LastName.ShouldEqual(one.LastName);
            rehydratedEmployee.EmailAddress.ShouldEqual(one.EmailAddress);
            rehydratedEmployee.AdminAssistant.ShouldEqual(adminAssistant);

        }
    }
}