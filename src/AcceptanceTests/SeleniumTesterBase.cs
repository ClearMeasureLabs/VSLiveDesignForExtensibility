using System.Configuration;
using System.Diagnostics;
using ClearMeasure.Bootcamp.IntegrationTests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;

namespace ClearMeasure.Bootcamp.AcceptanceTests
{
    public class SeleniumTesterBase
    {
        public IWebDriver Driver { get; private set; }
        public string AppUrl { get; private set; }

        [OneTimeSetUp]
        public void SetupTest()
        {
            AppUrl = ConfigurationManager.AppSettings["appUrl"];

            string browser = ConfigurationManager.AppSettings["browser"];
            switch (browser)
            {
                case "Chrome":
                    Driver = new ChromeDriver();
                    break;
                case "Firefox":
                    Driver = new FirefoxDriver();
                    break;
                case "IE":
                    Driver = new InternetExplorerDriver();
                    break;
                default:
                    Driver = new ChromeDriver();
                    break;
            }

            ProveDriverWorking();

//            LocalIisInstance.Startup();

            Trace.WriteLine("Loading database");
            new ZDataLoader().PopulateDatabase();
            Trace.WriteLine("Loaded database");

        }

        public void ProveDriverWorking()
        {
            Driver.Navigate().GoToUrl("https://www.bing.com/");
            Driver.FindElement(By.Id("sb_form_q")).SendKeys("VSTS");
            Driver.FindElement(By.Id("sb_form_go")).Click();
        }

        [OneTimeTearDown()]
        public void MyTestCleanup()
        {
            Driver.Quit();
//            LocalIisInstance.Cleanup();
        }
    }
}