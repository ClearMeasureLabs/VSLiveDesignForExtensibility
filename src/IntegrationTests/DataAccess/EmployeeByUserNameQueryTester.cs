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
    public class EmployeeByUserNameQueryTester
    {
        [Test]
        public void ShouldFindMatchedEmployee()
        {
            new DatabaseTester().Clean();

            var one = new Employee("1", "first1", "last1", "email1");
            var two = new Employee("2", "first2", "last2", "email2");
            var three = new Employee("3", "first3", "last3", "email3");

            using (EfCoreContext context = new StubbedDataContextFactory().GetContext())
            {
                context.Add(one);
                context.Add(two);
                context.Add(three);
                context.SaveChanges();
            }

            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var bus = container.GetInstance<Bus>();

            SingleResult<Employee> result = bus.Send(new EmployeeByUserNameQuery("1"));

            result.Result.ShouldEqual(one);
        }
    }
}