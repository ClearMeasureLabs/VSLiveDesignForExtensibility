using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.None)]
namespace ClearMeasure.Bootcamp.SmokeTests.Scenarios
{
    public static class ScenariosBootstrapper
    {
        private static Process _iisProcess;

        public static void Startup()
        {
            // kill off existing IIS Express instance if present
            var matchingProcess = Process.GetProcessesByName("iisexpress").FirstOrDefault();
            matchingProcess?.Kill();
            _iisProcess = new Process
                {
                    StartInfo =
                    {
                        FileName = SmokeTestPaths.GetIisExpressExecPath(),
                        Arguments = SmokeTestPaths.GetIisExpressExecArguments()
                    }
                };
            _iisProcess.Start();
        }
        
        public static void Cleanup()
        {
            // stop IIS Express
            _iisProcess?.Kill();
        }
    }
}