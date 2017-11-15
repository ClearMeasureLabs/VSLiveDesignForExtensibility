using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    public class DatabaseTester
    {
        public void Clean()
        {
            new DatabaseEmptier(DataContextFactory.GetContext().SessionFactory).DeleteAllData();
        }
    }
}