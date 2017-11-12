using System;
using System.Collections.Generic;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NHibernate;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class EmployeeSpecificationQueryHandler : IRequestHandler<EmployeeSpecificationQuery, MultipleResult<Employee>>
    {
        public MultipleResult<Employee> Handle(EmployeeSpecificationQuery request)
        {
            using (IDbContext dbContext = DataContextFactory.GetContext())
            {
                ICriteria criteria = dbContext.CreateCriteria(typeof(Employee));
                criteria.SetCacheable(true);
                
                IList<Employee> list = criteria.List<Employee>();
                Employee[] employees = new List<Employee>(list).ToArray();
                Array.Sort(employees);
                return new MultipleResult<Employee> {Results = employees};
            }
        }
    }
}