namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class DataContextFactory
    {
        private static bool _startupComplete;

        private static readonly object _locker =
            new object();

        public static EfDataContext GetContext()
        {
            return new EfDataContext();
        }

        public static EfDataContext GetEfContext()
        {
//            EnsureStartup();
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
        }
    }
}