using Newtonsoft.Json.Linq;
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
    public class SearchAgetTests : IDisposable
    {

        public void Dispose()
        {
            Utils.MakeSureChromeDriveDoesNotExist();
        }


        [Fact]
        [Trait("Category", "Search")]
        public void SearchAgnetGeneralFunctionalityTest()
        {
            if (Configuration.TestEnvironment != "qa")
            {
                return;
            }
            using (IWebDriver driver = Utils.GetChromeDriverNotMaximized())
            {
                Utils.Login(driver);
                Utils.DeleteAllSearchAgents();
                List<JToken> homes = ApiUtil.SearchHome(null, 5, null, null, null, null, null, null, null, null, null, null, null, null, null, SaleStatus.ACTIVE).ToList();
                foreach (JToken home in homes)
                {
                    JToken responseJson = ApiUtil.GetHomeBySlug(home["home"]["slug"].ToString());
                    long popularity = ((long)((JValue)responseJson["data"]["homeBySlug"]["popularity"]).Value);
                    Assert.Equal(0, popularity);
                }
                // Arrange
                var searchPage = new SearchPage(driver);
                // Act
                Utils.GoToUrl(driver, searchPage.Url);

                Utils.Wait();

                searchPage.AdvancedSearchSliderSpan.Click();
                Utils.Wait();
                IWebElement createSearchAgentBtn = driver.FindElement(By.XPath("//*[@id=\"__next\"]/div[1]/div[1]/div[1]/div[3]/div[2]/div/div/div/div/div[2]/button[1]"));
                createSearchAgentBtn.Click(3);
                IWebElement searchAgentNameTxt = driver.FindElement(By.XPath("/html/body/div[3]/div[3]/div/div[2]/div/div/input"));
                searchAgentNameTxt.SendKeys("AllHomesSearchAgnent");

                IWebElement saveSearchAgentBtn = driver.FindElement(By.XPath("/html/body/div[3]/div[3]/div/div[3]/button[2]"));
                while (!saveSearchAgentBtn.Click(3)) ;
                Utils.Wait();
                saveSearchAgentBtn.Click(3);
                Utils.Wait();
                saveSearchAgentBtn.Click(3);
                foreach (JToken home in homes)
                {
                    JToken responseJson = ApiUtil.GetHomeBySlug(home["home"]["slug"].ToString());
                    long popularity = ((long)((JValue)responseJson["data"]["homeBySlug"]["popularity"]).Value);
                    if (popularity > 0)
                    {

                    }
                    //Assert.Equal(1, popularity);//after creating Search agent all active homes must showup there
                }
            }
        }

    }
}
