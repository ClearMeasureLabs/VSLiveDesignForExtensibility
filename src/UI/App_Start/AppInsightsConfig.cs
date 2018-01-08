using System;
using System.Configuration;
using Microsoft.ApplicationInsights.Extensibility;

namespace ClearMeasure.Bootcamp.UI
{
    public class AppInsightsConfig
    {
        public static void Configure()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
        }
    }
}