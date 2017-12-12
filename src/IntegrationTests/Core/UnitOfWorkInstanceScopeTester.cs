using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using ClearMeasure.Bootcamp.UI.DependencyResolution;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Should;
using StructureMap;
using UI.DependencyResolution;

namespace ClearMeasure.Bootcamp.IntegrationTests.Core
{
    [TestFixture()]
    public class UnitOfWorkInstanceScopeTester
    {
        [Test]
        public void ShouldUseSameInstanceOfDataContextFactory()
        {
            IContainer container = DependencyRegistrarModule.EnsureDependenciesRegistered();
            var factory1 = container.GetInstance<EfCoreContext>();
            var factory2 = container.GetInstance<EfCoreContext>();
            object.ReferenceEquals(factory1, factory2).ShouldBeTrue();
        }
    }
}