using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using WeBrick.Test.UI.Websites;
using Xunit;

namespace WeBrick.Test.UI.ScreenTests
{
    public class HomeScreenTests : IDisposable
    {
        public void Dispose()
        {
            Utils.MakeSureChromeDriveDoesNotExist();
        }

        [Fact]
        [Trait("Category", "ScreenTest")]
        [Trait("Category", "Home")]
        public static void CheckVideoIsPlaying()
        {
            if (Configuration.RunHeadless)
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                // Arrange
                var home = new HomePage(driver);
                Utils.GoToUrl(driver, home.Url);
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                wait.Until(ExpectedConditions.UrlContains(home.Url));
                Utils.ScrollToElement(driver, home.Video);

                // Act
                Utils.Wait();
                string firstImage = @$".\{Guid.NewGuid().ToString("N")}.jpg";
                Utils.TakeScreenshot(firstImage);
                Utils.Wait();
                Utils.Wait();
                string secondImage = @$".\{Guid.NewGuid().ToString("N")}.jpg";
                Utils.TakeScreenshot(secondImage);

                float similarity = Utils.CompareImages(firstImage, secondImage);
                // Assert
                Assert.True(similarity < 0.999);
            }
        }

    }
}
