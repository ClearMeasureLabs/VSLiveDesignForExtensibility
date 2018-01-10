using System;
using System.Configuration;
using System.Diagnostics;
using ClearMeasure.Bootcamp.IntegrationTests;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using Should;

namespace ClearMeasure.Bootcamp.AcceptanceTests
{
	[TestFixture]
	public class LoginPageShould
	{
		private readonly string _siteUrl;
		private IWebDriver _driver;
		private bool IsLocalDevelopment { get; } = ConfigurationManager.AppSettings["environment"] == "#{Environment}";

		public LoginPageShould()
	    {
		    _siteUrl = IsLocalDevelopment ? ConfigurationManager.AppSettings["localSiteUrl"]: ConfigurationManager.AppSettings["siteUrl"];
	    }

		[OneTimeSetUp]
		public void Startup()
		{
			if (IsLocalDevelopment)
				LocalIisInstance.Startup();

			Trace.WriteLine("Loading database");
			new ZDataLoader().PopulateDatabase();
			Trace.WriteLine("Loaded database");

			var browser = ConfigurationManager.AppSettings["browser"];
			SelectBrowser(browser);
		}

		[OneTimeTearDown]
		public void Cleanup()
		{
			_driver?.Quit();
			_driver?.Dispose();

			if (IsLocalDevelopment)
				LocalIisInstance.Cleanup();
		}

		private void SelectBrowser(string browser)
		{
			switch (browser)
			{
				case "Firefox":
					_driver = new FirefoxDriver();
					break;
				case "Chrome":
					_driver = new ChromeDriver();
					break;
				default:
					throw new ArgumentException("Unsupported browser, currently only Firefox and Chrome supported");
			}
		}

		[Test]
	    public void Should_exist()
		{
			_driver.Navigate().GoToUrl(_siteUrl);
			_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
			var userSelect = new SelectElement(_driver.FindElement(By.Id("UserName")));
			userSelect.SelectByValue("jpalermo");
			var login = _driver.FindElement(By.XPath("//button[contains(text(), 'Log In')]"));
			login.Click();

			_driver.Title.ShouldStartWith("Home Page");

			var logout = _driver.FindElement(By.LinkText("Logout"));
			logout.Click();

			_driver.Title.ShouldStartWith("Login");
		}
	}
}
