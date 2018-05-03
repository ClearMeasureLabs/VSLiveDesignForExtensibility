using System.Web.Mvc;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.SearchExpenseReports;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.Core.Services;
using ClearMeasure.Bootcamp.UI.Helpers.ActionFilters;
using ClearMeasure.Bootcamp.UI.Models;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    [AddUserMetaDataToViewData]
    [Authorize]
    public class ExpenseReportSearchController : Controller
    {
        private readonly IExpenseReportRepository _repository;
        private readonly Bus _bus;

        public ExpenseReportSearchController(IExpenseReportRepository repository, Bus bus)
        {
            _repository = repository;
            _bus = bus;
        }

        public ActionResult Index(ExpenseReportSearchModel.SearchFilters filters)
        {
            var model = new ExpenseReportSearchModel();
            if (filters != null)
                model.Filters = filters;


            var submitter = _bus.Send(new EmployeeByUserNameQuery(model.Filters.Submitter)).Result;
            var approver = _bus.Send(new EmployeeByUserNameQuery(model.Filters.Approver)).Result;
            var status = !string.IsNullOrWhiteSpace(model.Filters.Status) ? ExpenseReportStatus.FromKey(model.Filters.Status) : null;

            var specification = new SearchSpecification();
            specification.MatchApprover(approver);
            specification.MatchSubmitter(submitter);
            specification.MatchStatus(status);

            ExpenseReport[] orders = _repository.GetMany(specification);

            model.Results = orders;

            return View(model);
        }

    }
}
