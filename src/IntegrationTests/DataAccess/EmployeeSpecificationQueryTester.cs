using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using NUnit.Framework;
using StructureMap;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    [TestFixture]
    public class EmployeeSpecificationQueryTester
    {
        [Test]
        public void ShouldGetAllEmployees()
        {
            new DatabaseTester().Clean();

            var one = new Employee("1", "first1", "last1", "email1");
            var two = new Employee("2", "first2", "last2", "email2");
            var three = new Employee("3", "first3", "last3", "email3");

            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Add(one);
                context.Add(two);
                context.Add(three);
                context.SaveChanges();
            }

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();
            Employee[] employees = bus.Send(EmployeeSpecificationQuery.All).Results;

            Assert.That(employees.Length, Is.EqualTo(3));
            Assert.That(employees[0].UserName, Is.EqualTo("1"));
            Assert.That(employees[0].FirstName, Is.EqualTo("first1"));
            Assert.That(employees[0].LastName, Is.EqualTo("last1"));
            Assert.That(employees[0].EmailAddress, Is.EqualTo("email1"));
        }
    }
}