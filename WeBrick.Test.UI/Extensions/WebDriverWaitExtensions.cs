using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace WeBrick.Test.UI.Extensions
{
    public static class WebDriverWaitExtensions
    {
        public static bool WaitForDocumentReadyStateComplete(this WebDriverWait wait)
        {
            return wait.Until(expectedCondition => ((IJavaScriptExecutor)expectedCondition).ExecuteScript("return document.readyState").Equals("complete"));
        }

        public static Cookie WaitForCookie(this WebDriverWait wait, string cookieName)
        {
            return wait.Until(driver => driver.Manage().Cookies.GetCookieNamed(cookieName));
        }

        public static bool WaitForCookieAbsence(this WebDriverWait wait, string cookieName)
        {
            return wait.Until(driver => driver.Manage().Cookies.GetCookieNamed(cookieName) == null);
        }
    }
}
