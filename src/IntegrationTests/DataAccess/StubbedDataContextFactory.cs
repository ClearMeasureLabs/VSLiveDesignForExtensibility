using System;
using System.Configuration;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    public class StubbedDataContextFactory : IDisposable
    {
        private readonly EfCoreContext _context;
        private readonly DataConfigurationStub _config;

        public StubbedDataContextFactory()
        {
            _config = new DataConfigurationStub(
                ConfigurationManager.ConnectionStrings["Bootcamp"].ConnectionString);
            _context = new EfCoreContext(_config);
        }

        public DataConfigurationStub Config
        {
            get { return _config; }
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

        public void Dispose()
        {
            if(_context != null) _context.Dispose();
        }
    }
}