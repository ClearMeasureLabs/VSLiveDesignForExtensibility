using System;
using NUnit.Framework;
using OpenQA.Selenium;
using Should;

namespace ClearMeasure.Bootcamp.AcceptanceTests
{
    [TestFixture]
    public class LoginPageTester : SeleniumTesterBase
    {
        [Test]
        public void ShouldLoginAndLogOut()
        {
            Driver.Navigate().GoToUrl(AppUrl);
            var login = Driver.FindElement(By.XPath("//button[contains(text(), 'Log In')]"));
            login.Click();

            Driver.Title.ShouldStartWith("Home Page");

            var logout = Driver.FindElement(By.LinkText("Logout"));
            logout.Click();

            Driver.Title.ShouldStartWith("Login");
        }
    }
}