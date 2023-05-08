using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;

namespace WeBrick.Test.UI.SmokeTests
{
    public class FrontPageShould
    {
        private const string frontpageUrl = "https://webrick.pt/";
        private const string frontpageTitle = "WeBrick";

        [Fact]
        [Trait("Category", "Smoke")]
        public void Load()
        {
            using (var driver = Utils.GetChromeDriver())
            {
                Utils.LoginWithBasicAuth(driver);

                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var titleBy = OpenQA.Selenium.By.TagName("title");

                // Act
                driver.Navigate().GoToUrl(frontpageUrl);
                wait.Until(e => ExpectedConditions.TextToBePresentInElement(e.FindElement(titleBy), "frontPageTitle"));

                // Assert
                Assert.Equal(frontpageTitle, driver.Title);
                Assert.Equal(frontpageUrl, driver.Url);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void Reload()
        {
            using (var driver = Utils.GetChromeDriver())
            {
                Utils.LoginWithBasicAuth(driver);

                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Act
                driver.Navigate().GoToUrl(frontpageUrl);
                driver.Navigate().Refresh();

                wait.WaitForDocumentReadyStateComplete();

                // Assert
                Assert.Equal(frontpageUrl, driver.Url);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void CheckFooter()
        {
            GenericChecks.CheckFooter(frontpageUrl);
        }
    }
}