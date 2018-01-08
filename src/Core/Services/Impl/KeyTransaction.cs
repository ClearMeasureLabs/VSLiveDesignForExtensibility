using System.Collections.Generic;
using Microsoft.ApplicationInsights;

namespace ClearMeasure.Bootcamp.Core.Services.Impl
{
    public class KeyTransaction : IKeyTransaction
    {
        private readonly TelemetryClient _telemetryClient = new TelemetryClient();

        public void Track(string eventName, IDictionary<string, string> properties = null)
        {
            _telemetryClient.TrackEvent(eventName, properties);
        }
    }
}
