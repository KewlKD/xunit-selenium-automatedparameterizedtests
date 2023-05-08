using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;

namespace WeBrick.Test.UI
{
    public static class SeleniumIdExtensions
    {
        internal static bool Exists(this By element, IWebDriver driver)
        {
            var wait = new WebDriverWait(driver, TimeSpan.FromMilliseconds(Utils.WaitTime));
            IWebElement res = wait.Until(ExpectedConditions.ElementExists(element));
            return res != null;
        }
    }
}



