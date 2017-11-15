using FluentNHibernate.Cfg;
using NHibernate;
using NHibernate.Cfg;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class DataContextFactory
    {
        private static ISessionFactory _sessionFactory;
        private static bool _startupComplete;

        private static readonly object _locker =
            new object();

        public static IDbContext GetContext()
        {
            EnsureStartup();
            IDbContext dbContext = new DbContextWrapper(_sessionFactory.OpenSession());
            dbContext.BeginTransaction();
            return dbContext;
        }

        public static EfDataContext GetEfContext()
        {
            EnsureStartup();
            return new EfDataContext();
        }

        public static void EnsureStartup()
        {
            if (!_startupComplete)
            {
                lock (_locker)
                {
                    if (!_startupComplete)
                    {
                        PerformStartup();
                        _startupComplete = true;
                    }
                }
            }
        }

        private static void PerformStartup()
        {
            InitializeSessionFactory();
        }

        private static void InitializeSessionFactory()
        {
            Configuration configuration =
                BuildConfiguration();
            _sessionFactory =
                configuration.BuildSessionFactory();
        }

        public static Configuration BuildConfiguration()
        {
            return
                Fluently.Configure(
                    new Configuration().Configure())
                    .Mappings(cfg =>
                        cfg.FluentMappings
                            .AddFromAssembly(
                                typeof (DataContextFactory)
                                    .Assembly))
                    .BuildConfiguration();
        }
    }
}