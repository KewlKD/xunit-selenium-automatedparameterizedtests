using OpenQA.Selenium.Support.UI;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;


// Tests that should be true everywhere on the WeBrick

namespace WeBrick.Test.UI
{
    public static class GenericChecks
    {
        public static void CheckFooter(string url)
        {
            using (var driver = Utils.GetChromeDriver())
            {

                Utils.LoginWithBasicAuth(driver);
            
                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var footerBy = OpenQA.Selenium.By.TagName("footer");


                // Act
                driver.Navigate().GoToUrl(url);
                wait.WaitForDocumentReadyStateComplete();

                var footer = driver.FindElement(footerBy);
                var footerContactInfoElement = footer.FindElement(OpenQA.Selenium.By.XPath(".//div/div/div[1]"));
                var footerAboutWebrickElement = footer.FindElement(OpenQA.Selenium.By.XPath(".//div/div/div[2]"));
                var footerAboutWebrickLinkElements = footerAboutWebrickElement.FindElements(OpenQA.Selenium.By.TagName("a"));
                var socialMediaElement = footer.FindElement(OpenQA.Selenium.By.XPath(".//div/div/div[6]"));
                var socialMediaLinkElements = socialMediaElement.FindElements(OpenQA.Selenium.By.TagName("a"));
                

                // Assert
                Assert.Equal("WeBrick A/S\nSankt Thomas Alle 1, st. tv\n1824 Frederiksberg C\nCVR: 40753710\nBRUG FOR HJÆLP?", 
                             footerContactInfoElement.Text);
                
                Assert.Equal("https://webrick.pt/sider/om-webrick", footerAboutWebrickLinkElements[0].GetAttribute("href"));
                Assert.Equal("https://webrick.pt/sider/kontakt_os", footerAboutWebrickLinkElements[1].GetAttribute("href"));
                Assert.Equal("https://webrick.pt/my-properties/find", footerAboutWebrickLinkElements[2].GetAttribute("href"));
                Assert.Equal("https://webrick.pt/sider/kob-din-bolig-pa-webrick",   footerAboutWebrickLinkElements[3].GetAttribute("href"));
                Assert.Equal("https://webrick.pt/sider/sporgsmal-svar", footerAboutWebrickLinkElements[4].GetAttribute("href"));
                Assert.Equal("https://webrick.pt/presse", footerAboutWebrickLinkElements[5].GetAttribute("href"));
                Assert.Equal("https://webrick.pt/sider/cookie-og-persondata", footerAboutWebrickLinkElements[6].GetAttribute("href"));
                Assert.Equal("https://www.facebook.com/WeBrick-112231790361536", socialMediaLinkElements[0].GetAttribute("href"));
                Assert.Equal("https://www.instagram.com/webrick_dk/?hl=da", socialMediaLinkElements[1].GetAttribute("href"));
                Assert.Equal("https://www.linkedin.com/company/webrick", socialMediaLinkElements[2].GetAttribute("href"));
            }
        }   
    }
}