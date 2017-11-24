using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class EmployeeByUserNameQueryHandler : IRequestHandler<EmployeeByUserNameQuery, SingleResult<Employee>>
    {
        public SingleResult<Employee> Handle(EmployeeByUserNameQuery specification)
        {
            using (EfDataContext dbContext = DataContextFactory.GetContext())
            {
                var employee = dbContext.Set<Employee>().SingleOrDefault(e=>e.UserName == specification.UserName);
                return new SingleResult<Employee>(employee);
            }
        }
    }
}