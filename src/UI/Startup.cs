using ClearMeasure.Bootcamp.UI;
using Microsoft.Owin;
using Owin;
using System.Diagnostics;
using System.IO;
using System.Text;

[assembly: OwinStartup(typeof(Startup))]
namespace ClearMeasure.Bootcamp.UI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            Trace.Listeners.Add(new TextWriterTraceListener(new Log4NetTextWriter()));
        }
    }

    internal class Log4NetTextWriter : TextWriter
    {
        public override Encoding Encoding => new ASCIIEncoding();

        public override void WriteLine(string value)
        {
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType).Debug(value + " trace");
        }
    }
}
