﻿using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.DataAccess
{
    public class ExpenseReportSaveCommandHandler : IRequestHandler<ExpenseReportSaveCommand, SingleResult<ExpenseReport>>
    {
        public SingleResult<ExpenseReport> Handle(ExpenseReportSaveCommand request)
        {
            using (EfDataContext context = DataContextFactory.GetEfContext())
            {
                context.Attach(request.ExpenseReport.Submitter);
                context.Attach(request.ExpenseReport.Approver);
                context.Update(request.ExpenseReport);
                context.SaveChanges();
            }

            return new SingleResult<ExpenseReport>(request.ExpenseReport);
        }
    }
}