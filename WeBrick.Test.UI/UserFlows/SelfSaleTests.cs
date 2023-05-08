using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using WeBrick.Test.UI.Models;
using WeBrick.Test.UI.Websites;
using Xunit;

namespace WeBrick.Test.UI.UserFlows
{
    public class SelfSaleTests : IDisposable
    {

        public SelfSaleTests()
        {
        }

        public void Dispose()
        {
            Utils.MakeSureChromeDriveDoesNotExist();
        }

        [Fact]
        [Trait("Category", "SelfSale")]
        public void PressEscapeAfterSearchingSomething()
        {
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                // Arrange
                var selfSalePage = new SelfSalePage(driver);
                Utils.GoToUrl(driver, selfSalePage.Url);
                Utils.Wait();
                selfSalePage.SearchtextBox.SendKeys("sankt thomas");
                Utils.Wait();
                try
                {
                    selfSalePage.SearchtextBox.SendKeys(Keys.Escape);
                    selfSalePage.SearchtextBox.SendKeys(Keys.Escape);
                    selfSalePage.SearchtextBox.SendKeys(Keys.Escape);
                    selfSalePage.SearchtextBox.SendKeys(Keys.Escape);
                }
                catch (Exception)
                {
                    //Ignore sending keys error
                }
                Utils.Wait();
                string bodyText = selfSalePage.Body.GetInnerText().ToLower();
                Assert.False(bodyText.Contains("fejl"));

            }
        }
    }
}
