using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;

namespace WeBrick.Test.UI.SmokeTests
{
    public class PropertyPageShould
    {
        private const string propertyPageUrl = "https://webrick.pt/property/bag-slotsgraven-51-4400-kalundborg";
        private const string propertyPageTitle = "Bag Slotsgraven 51, 4400 Kalundborg";

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
                driver.Navigate().GoToUrl(propertyPageUrl);
                wait.WaitForDocumentReadyStateComplete();

                // Assert
                Assert.Equal(propertyPageTitle, driver.Title);
                Assert.Equal(propertyPageUrl, driver.Url);
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
                driver.Navigate().GoToUrl(propertyPageUrl);
                driver.Navigate().Refresh();

                wait.WaitForDocumentReadyStateComplete();

                // Assert
                Assert.Equal(propertyPageUrl, driver.Url);
            }
        }

        [Fact]
        [Trait("Category","Smoke")]
        public void CheckFooter()
        {
            GenericChecks.CheckFooter(propertyPageUrl);
        }
    }
}