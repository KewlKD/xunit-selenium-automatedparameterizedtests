using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using WeBrick.Test.UI.Extensions;
using Xunit;

namespace WeBrick.Test.UI.SmokeTests
{
    public class FrontPageShould
    {
        private const string frontpageTitle = "WeBrick";

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
                Utils.GoToUrl(driver, Configuration.Website);
                wait.Until(e => ExpectedConditions.TextToBePresentInElement(e.FindElement(titleBy), "frontPageTitle"));

                // Assert
                Assert.Equal(frontpageTitle, driver.Title);
                Assert.Equal(Configuration.Website, driver.Url);
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
                Utils.GoToUrl(driver, Configuration.Website);
                driver.Navigate().Refresh();

                wait.WaitForDocumentReadyStateComplete();

                // Assert
                Assert.Equal(Configuration.Website, driver.Url);
            }
        }

        [Fact]
        [Trait("Category", "Smoke")]
        public void CheckFooter()
        {
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {

                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var home = new Websites.HomePage(driver);

                // Act

                Utils.GoToUrl(driver, home.Url);
                wait.WaitForDocumentReadyStateComplete();
                // Assert
                string contactInfoText = home.FooterContactInfoElement.GetInnerText();
                Enumerable.Range(0, 10).ToList().ForEach(i =>
                {
                    Utils.ScrollDown(driver, 400);
                });
                Assert.Contains(Configuration.FooterText.Replace("\r", "").Replace("\n", ""), contactInfoText.Replace("\r", "").Replace("\n", ""));


                Assert.Equal(Configuration.FooterElementZero, home.FooterAboutWebrickLinkElements[0].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.FooterElementOne, home.FooterAboutWebrickLinkElements[1].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.FooterElementTwo, home.FooterAboutWebrickLinkElements[2].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.FooterElementThree, home.FooterAboutWebrickLinkElements[3].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.FooterElementFour, home.FooterAboutWebrickLinkElements[4].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.FooterElementFive, home.FooterAboutWebrickLinkElements[5].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.SocialMediaElementZero, home.SocialMediaLinkElements[0].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.SocialMediaElementOne, home.SocialMediaLinkElements[1].GetAttribute("href").ToString().ToLower());
                Assert.Equal(Configuration.SocialMediaElementTwo, home.SocialMediaLinkElements[2].GetAttribute("href").ToString().ToLower());
            }
        }
    }
}
