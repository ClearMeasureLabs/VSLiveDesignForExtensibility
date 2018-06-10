using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;

namespace ClearMeasure.Bootcamp.AcceptanceTests
{
	public class LocalIisInstance
	{
		private static Process _iisProcess;

        private LocalIisInstance()
        {
        }

        public static void Startup()
		{
			// kill off existing IIS Express instance if present
			var matchingProcess = Process.GetProcessesByName("iisexpress").FirstOrDefault();
			matchingProcess?.Kill();
			_iisProcess = new Process
			{
				StartInfo =
				{
					FileName = GetIisExpressExecPath(),
					Arguments = GetIisExpressExecArguments()
				}
			};
			_iisProcess.Start();
		}

		public static void Cleanup()
		{
			// stop IIS Express
			if (!_iisProcess.HasExited)
				_iisProcess.Kill();
		}

		private static string GetIisExpressExecArguments()
		{
			// TODO: Think of a better, more robust way of finding the website root
			var path = AppDomain.CurrentDomain.BaseDirectory;
			// for running within VS
			path = path.Replace(@"AcceptanceTests\bin\Debug", "UI");
			// for running from the command line
			path = path.Replace(@"build\test", @"src\UI");
			var port = ConfigurationManager.AppSettings["localPort"];
			var arguments = $"/path:{path} /port:{port} /clr:v4.0";
			return arguments;
		}

		private static string GetIisExpressExecPath()
		{
			var path = ConfigurationManager.AppSettings["iisExpressPath"];
			return path;
		}
	}
}
