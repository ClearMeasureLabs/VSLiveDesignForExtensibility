using System;
using System.Web.Mvc;
using ClearMeasure.Bootcamp.Core.Services;
using Microsoft.ApplicationInsights.DataContracts;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    public class DemoController : Controller
    {
        private readonly IKeyTransaction _keyTransaction;
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public DemoController(IKeyTransaction keyTransaction)
        {
            _keyTransaction = keyTransaction;
        }

        public ActionResult ThrowException()
        {
            throw new Exception("DEMOEXCEPTION - Throwing an exception for demo purposes.");
        }

        public ActionResult KeyTransaction()
        {
            _keyTransaction.Track("DemoExpenseReportRejected");

            return new EmptyResult();
        }

        public ActionResult CustomEvents()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackEvent("DemoExpenseReportSubmitted");

            return new EmptyResult();
        }

        public ActionResult CustomMetrics()
        {
            var sample = new MetricTelemetry();
            sample.Name = "demo - metric name";
            sample.Sum = 42.3;
           
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackMetric(sample);

            return new EmptyResult();
        }

        public ActionResult LogException()
        {
            var ex = new ArgumentException("Invalid argument");
            Log.Error("Demo - logging an exception", ex);

            return new EmptyResult();
        }
    }
}
