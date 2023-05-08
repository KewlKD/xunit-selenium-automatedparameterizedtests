
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;
using Xunit.Abstractions;

namespace WeBrick.Test.UI.UserFlows
{
    public partial class LoginFromFrontPage
    {
        private string frontpageUrl = "https://webrick.pt/";
        private string loginUrl = "https://auth.webrick.pt/account/login";
        private string phoneNumber = "51949277";
        private string password = "DataDiagramAnymoreIdentityRebateCuring";
        private string cookieName = "__is_loggedin";

        // Use as console output, e.g. output.WriteLine( ... )
        private readonly ITestOutputHelper output;

        public LoginFromFrontPage(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        [Trait("Category", "UserFlow")]
        public void LoginFullscreen()
        {
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                
                Utils.LoginWithBasicAuth(driver);

                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Act
                driver.Navigate().GoToUrl(frontpageUrl);

                var menuButtonBy = By.XPath("/html/body/div[1]/div/header/div/div/div[2]/button[2]");
                var loginButtonBy = By.XPath("/html/body/div[3]/div[3]/ul/li");

                wait.Until(ExpectedConditions.ElementExists(menuButtonBy)).Click();

                //TODO: Find a better way to check for existence of loginButton
                System.Threading.Thread.Sleep(200);
                wait.Until(ExpectedConditions.ElementExists(loginButtonBy)).Click();
                wait.Until(ExpectedConditions.UrlContains(loginUrl));
                System.Threading.Thread.Sleep(200);

                driver.FindElement(By.Name("UserEnteredPhoneNumber")).SendKeys(phoneNumber);
                driver.FindElement(By.Name("Password")).SendKeys(password);
                driver.FindElement(By.Name("button")).Click();

                var cookie = wait.WaitForCookie(cookieName);

                // Assert
                Assert.Equal("__is_loggedin", cookie.Name);
                Assert.Equal("true", cookie.Value);
                Assert.Equal(frontpageUrl, driver.Url);
            }
        }
    }
}
