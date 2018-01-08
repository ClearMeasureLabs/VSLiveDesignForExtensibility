using System.Collections.Generic;

namespace ClearMeasure.Bootcamp.Core.Services
{
    public interface IKeyTransaction
    {
        void Track(string eventName, IDictionary<string, string> properties = null);
    }
}
