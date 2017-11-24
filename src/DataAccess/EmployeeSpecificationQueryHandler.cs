using System;
using System.Linq;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class EmployeeSpecificationQueryHandler : IRequestHandler<EmployeeSpecificationQuery, MultipleResult<Employee>>
    {
        public MultipleResult<Employee> Handle(EmployeeSpecificationQuery request)
        {
            using (EfDataContext dbContext = DataContextFactory.GetContext())
            {
                Employee[] employees = dbContext.Set<Employee>().ToArray();
                Array.Sort(employees);
                return new MultipleResult<Employee> {Results = employees};
            }
        }
    }
}