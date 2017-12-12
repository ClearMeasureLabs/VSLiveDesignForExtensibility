using ClearMeasure.Bootcamp.UI.DependencyResolution;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    public class DatabaseTester
    {
        public void Clean()
        {
            DependencyRegistrarModule.Reset();

            new DatabaseEmptier().DeleteAllData();
        }
    }
}