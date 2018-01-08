using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace ClearMeasure.Bootcamp.Core.Services.Impl
{
    public class KeyTransaction : IKeyTransaction
    {
        private static readonly TelemetryClient Telemetry = new TelemetryClient();

        public void Track(string eventName, IDictionary<string, string> properties = null)
        {
            Telemetry.TrackEvent(eventName, properties);
        }
    }
}
