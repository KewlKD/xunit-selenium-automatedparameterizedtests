using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Diagnostics;
using WeBrick.Test.UI.Extensions;
using Xunit;
using System.Collections.Generic;
using WeBrick.Test.UI.Websites;
using System.Text.RegularExpressions;
using System.Linq;

namespace WeBrick.Test.UI.BackOffice
{
    public class UserClaimTests
    {

        [Fact]
        [Trait("Category", "UserClaimTests")]
        public void AllUserClaims()
        {
            using (IWebDriver driver = Utils.GetChromeDriver())
            {
                // Arrange
                try
                {
                    Utils.Login(driver);
                    var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                    // Act
                    driver.Navigate().GoToUrl(Configuration.UserClaimPage);
                    wait.WaitForDocumentReadyStateComplete();
                    Utils.Wait();
                    // Assert

                    var userClaimPage = new UserClaimPage(driver);
                    Assert.Contains(Configuration.UserClaimPage.Split('/').Last(), driver.Url);
                    string allInfo = userClaimPage.UserTable.GetInnerText();
                    Assert.Contains("email	ghainian@gmail.com", allInfo);
                    Assert.Contains("family_name	ghainian", allInfo);
                    Assert.Contains("given_name	mehran", allInfo);
                    Assert.Contains("phone_number	+4523391178", allInfo);
                }
                finally
                {
                    Utils.Logout(driver);
                }

            }
        }


    }
}


