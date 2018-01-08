using System;
using System.Configuration;
using ClearMeasure.Bootcamp.Core.Services;
using ClearMeasure.Bootcamp.Core.Services.Impl;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;

namespace ClearMeasure.Bootcamp.UI
{
    public class AppInsightsConfig
    {
        public static void Configure()
        {
            TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings["APPINSIGHTS_INSTRUMENTATIONKEY"];
            TelemetryConfiguration.Active.TelemetryInitializers.Add(new TelemetryInitializer());
            TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = Boolean.Parse(ConfigurationManager.AppSettings["AppInsightsDeveloperMode"]);
        }

        public class TelemetryInitializer : ITelemetryInitializer
        {
            public void Initialize(ITelemetry telemetry)
            {
                IApplicationInformation appInfo = new ApplicationInformation();
                // Component.Version maps to application_Version
                telemetry.Context.Component.Version = appInfo.ProductVersion;
                // Environment will be mapped to customDimensions
                telemetry.Context.Properties.Add("Environment", ConfigurationManager.AppSettings["SiteEnvironment"]);
            }
        }
    }
}