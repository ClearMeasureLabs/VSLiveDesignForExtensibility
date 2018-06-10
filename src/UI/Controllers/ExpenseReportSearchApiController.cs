using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.SearchExpenseReports;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    [System.Web.Http.RoutePrefix("api/search")]
    public class ExpenseReportSearchApiController : ApiController
    {
        private Bus _bus;

        public ExpenseReportSearchApiController(Bus bus)
        {
            _bus = bus;
        }

        [System.Web.Http.HttpGet]
        public HttpResponseMessage Get(string approverName, string submitterName, string statusCode)
        {
            var submitter = _bus.Send(new EmployeeByUserNameQuery(submitterName)).Result;
            var approver = _bus.Send(new EmployeeByUserNameQuery(approverName)).Result;
            var status = !string.IsNullOrWhiteSpace(statusCode) ? ExpenseReportStatus.FromKey(statusCode) : null;

            var specification = new ExpenseReportSpecificationQuery
            {
                Approver = approver,
                Submitter = submitter,
                Status = status
            };

            ExpenseReport[] orders = _bus.Send(specification).Results;

            var responseMessage = Request.CreateResponse(HttpStatusCode.OK, orders);
            return responseMessage;
        }
    }
}