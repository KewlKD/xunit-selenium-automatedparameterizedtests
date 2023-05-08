using OpenQA.Selenium;
using System;

namespace WeBrick.Test.UI
{
    public static class WebElementExtensions
    {
        public static string GetInnerHtml(this IWebElement element)
        {
            WebElement webElement = (WebElement)element;
            var javaScriptExecutor = (IJavaScriptExecutor)webElement.WrappedDriver;
            var innerHtml = javaScriptExecutor.ExecuteScript("return arguments[0].innerHTML;", element).ToString();
            return innerHtml;
        }

        public static bool Click(this IWebElement element, int retryCount = 3)
        {
            bool clicksFine = true;

            for (int i = 0; i < retryCount; i++)
            {
                clicksFine = true;
                try
                {
                    element.Click();
                }
                catch (Exception)
                {
                    clicksFine = false;
                    //ignore exception
                }
                if (clicksFine)
                {
                    return clicksFine;
                }
            }
            return clicksFine;
        }
        public static string GetInnerText(this IWebElement element, int retryCount = 1)
        {
            Exception ex = null;
            for (int i = 0; i < retryCount; i++)
            {
                try
                {

                    WebElement webElement = (WebElement)element;
                    var javaScriptExecutor = (IJavaScriptExecutor)webElement.WrappedDriver;
                    var innerText = javaScriptExecutor.ExecuteScript("return arguments[0].innerText;", element).ToString();
                    return innerText;
                }
                catch (Exception e)
                {
                    ex = e;
                }
            }
            throw ex;
        }
    }
}
