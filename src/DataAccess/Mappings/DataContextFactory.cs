using System;

namespace ClearMeasure.Bootcamp.DataAccess.Mappings
{
    public class DataContextFactory
    {
        private static Func<DataContextFactory> _instanceBuilder = () => new DataContextFactory();
        private readonly EfDataContext _context;

        public DataContextFactory()
        {
            _context = new EfDataContext();
        }

        public static EfDataContext GetContext()
        {
            var context = GetUsableContext();
            return context;
        }

        public static EfDataContext GetEfContext()
        {
            return _instanceBuilder()._context;
        }

        private static EfDataContext GetUsableContext()
        {
            var context = _instanceBuilder()._context;
            if (context == null || context.IsDisposed)
                return new EfDataContext();

            return context;
        }

        public static void SetInstanceBuilder(Func<DataContextFactory> instanceBuilder)
        {
            _instanceBuilder = instanceBuilder;
        }
    }
}