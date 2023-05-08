using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace WeBrick.Test.UI.UserFlows
{
    public partial class LogoutFromFrontPage
    {
        private const string frontpageUrl = "https://webrick.pt/";
        private const string cookieName = "__is_loggedin";

        // Use as console output, e.g. output.WriteLine( ... )
        private readonly ITestOutputHelper output;

        public LogoutFromFrontPage(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        [Trait("Category", "UserFlow")]
        public void LogoutFullscreen()
        {
            using (IWebDriver driver = Utils.GetChromeDriver())
            {

                Utils.LoginWithBasicAuth(driver);
                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var menuButtonBy = By.XPath("/html/body/div[1]/div/header/div/div/div[2]/button[2]");
                var logoutButtonBy = By.XPath("/html/body/div[3]/div[3]/ul/li");

                //We need to be logged in to log out
                Utils.Login(driver);

                // Act
                wait.Until(ExpectedConditions.ElementExists(menuButtonBy)).Click();
                System.Threading.Thread.Sleep(200);
                wait.Until(ExpectedConditions.ElementExists(logoutButtonBy)).Click();
                wait.Until(ExpectedConditions.UrlToBe(frontpageUrl));

                // Assert
                Assert.True(wait.WaitForCookieAbsence(cookieName));
            }
        }
    }
}
