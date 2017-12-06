using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class EmployeeByUserNameQueryHandler : IRequestHandler<EmployeeByUserNameQuery, SingleResult<Employee>>
    {
        private readonly EfCoreContext _context;

        public EmployeeByUserNameQueryHandler(EfCoreContext context)
        {
            _context = context;
        }

        public SingleResult<Employee> Handle(EmployeeByUserNameQuery specification)
        {
            var employee = _context.Set<Employee>().SingleOrDefault(e => e.UserName == specification.UserName);
            return new SingleResult<Employee>(employee);
        }
    }
}