using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;

namespace WeBrick.Test.UI.SmokeTests
{
    public class PropertyPageShould
    {
        private const string propertyPageUrl = "https://webrick.dk/property/sankt-thomas-alle-1-st-tv-1824-frederiksberg-c";
        private const string propertyPageTitle = "Sankt Thomas Alle 1, st. tv, 1824 Frederiksberg C";

        [Fact]
        [Trait("Category", "Smoke")]
        public void Load()
        {
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var titleBy = By.TagName("title");

                // Act
                Utils.GoToUrl(driver, propertyPageUrl);
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
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                // Act
                Utils.GoToUrl(driver, propertyPageUrl);
                driver.Navigate().Refresh();

                wait.WaitForDocumentReadyStateComplete();

                // Assert
                Assert.Equal(propertyPageUrl, driver.Url);
            }
        }
    }
}