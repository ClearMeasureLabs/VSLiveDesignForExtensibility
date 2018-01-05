using System.Configuration;
using System.IO;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net.Config;
using Microsoft.ApplicationInsights.Extensibility;

namespace ClearMeasure.Bootcamp.UI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly log4net.ILog Log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected void Application_Start()
        {
            ConfigureAppInsightsAndLogging();

            DemoCustomEvent();

            Log.Info("bootcamp app started"); // log will show up in AppInsights -> Analytics -> traces if AiAppender is configured correctly

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private void ConfigureAppInsightsAndLogging()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Server.MapPath("~/log4net.config")));
        }

        private void DemoCustomEvent()
        {
            var telemetry = new Microsoft.ApplicationInsights.TelemetryClient();
            telemetry.TrackEvent("AppStarted");
        }
    }
}
