using System;
using System.Configuration;
using System.Diagnostics;
using ClearMeasure.Bootcamp.IntegrationTests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Support.UI;
using Should;

namespace ClearMeasure.Bootcamp.SmokeTests.Scenarios
{
    [TestFixture]
    public class LoginScenarios
    {
        private static IWebDriver _driver;
        private static ICommandServer _driverService;
        private static readonly string DriversPath = SmokeTestPaths.GetDriversPath();
        private static readonly string HomePage = ConfigurationManager.AppSettings["siteUrl"];

        [Test]
        public void ShouldLogInAndLogOut()
        {
            Console.WriteLine($"Homepage: {HomePage}");
            _driver.Navigate().GoToUrl(HomePage);
            _driver?.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));

            var userSelect = new SelectElement(_driver.FindElement(By.Id("UserName")));
            userSelect.SelectByValue("jpalermo");
            var login = _driver.FindElement(By.XPath("//button[contains(text(), 'Log In')]"));
            login.Click();

            _driver.Title.ShouldStartWith("Home Page");

            var logout = _driver.FindElement(By.LinkText("Logout"));
            logout.Click();

            _driver.Title.ShouldStartWith("Login");
        }

        [OneTimeSetUp]
        public static void Startup()
        {
            ScenariosBootstrapper.Startup();
            Trace.WriteLine("Loading database");
            new ZDataLoader().PopulateDatabase();
            Trace.WriteLine("Loaded database");

            var browser = ConfigurationManager.AppSettings["browser"];
            SelectBrowser(browser);
        }

        [OneTimeTearDown]
        public static void Cleanup()
        {
            _driverService?.Dispose();
            _driver?.Quit();
            _driver?.Dispose();
            ScenariosBootstrapper.Cleanup();
        }

        private static void SelectBrowser(string browser)
        {
            switch (browser)
            {
                case "Firefox":
                    _driver = new FirefoxDriver();
                    break;
                case "Chrome":
                    var chromeDriverService = ChromeDriverService.CreateDefaultService(DriversPath, "chromedriver.exe");
                    chromeDriverService.Start();
                    _driver = new RemoteWebDriver(chromeDriverService.ServiceUrl, DesiredCapabilities.Chrome());
                    _driverService = chromeDriverService;
                    break;
                case "IE":
                    _driver = new InternetExplorerDriver(DriversPath);
                    break;
                case "PhantomJS":
                    var phantomJsPath = SmokeTestPaths.GetPhantomJsPath();
                    var phantomJsDriverService = PhantomJSDriverService.CreateDefaultService(phantomJsPath);
                    _driver = new PhantomJSDriver(phantomJsDriverService);
                    _driverService = phantomJsDriverService;
                    break;
                default:
                    throw new ArgumentException("Unknown browser");
            }
        }
    }
}