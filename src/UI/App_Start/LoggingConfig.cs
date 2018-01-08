using System.IO;
using System.Web;
using log4net.Config;

namespace ClearMeasure.Bootcamp.UI
{
    public class LoggingConfig
    {
        public static void Configure(HttpServerUtility server)
        {
            XmlConfigurator.ConfigureAndWatch(new FileInfo(server.MapPath("~/log4net.config")));
        }
    }
}