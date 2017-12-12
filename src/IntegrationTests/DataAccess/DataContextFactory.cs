using System.Configuration;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    public class DataContextFactory
    {
        private readonly EfCoreContext _context;

        public DataContextFactory()
        {
            _context = new EfCoreContext(
                new DataConfigurationStub(
                    ConfigurationManager.ConnectionStrings["Bootcamp"].ConnectionString));
        }

        public EfCoreContext GetContext()
        {
            return _context;
        }
        
        public class DataConfigurationStub : IDataConfiguration
        {
            private string _connectionString;

            public DataConfigurationStub(string connectionString)
            {
                _connectionString = connectionString;
            }

            public string GetConnectionString()
            {
                return _connectionString;
            }
        }
    }
}