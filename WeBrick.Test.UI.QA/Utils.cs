using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using WeBrick.Test.UI.Extensions;
using System;
using System.Collections.Generic;

namespace WeBrick.Test.UI
{
    public static class Utils
    {
        public static IWebDriver GetChromeDriver(bool headless = true)
        {
            var chromeOptionsHeadless = new ChromeOptions();
            chromeOptionsHeadless.AddArguments(new List<string>() { "headless","--window-size=1980,1080})"});

            var chromeOptions = new ChromeOptions();
            chromeOptions.AddArguments(new List<string>() { "--window-size=1980,1080"});


            var chromeDriverService = ChromeDriverService.CreateDefaultService();

            var driver = headless
                ? new ChromeDriver(chromeDriverService, chromeOptionsHeadless)
                : new ChromeDriver(chromeDriverService, chromeOptions);

            //driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

            return driver;
        }

        public static void LoginWithBasicAuth(IWebDriver driver)
        {
            driver.Navigate().GoToUrl("https://webrick:test123@webrick.pt/");
        }

        public static void Login(IWebDriver driver) 
        {
            LoginWithBasicAuth(driver);

            var loginUrl = "https://auth.webrick.pt/account/login";
            var frontpageUrl = "https://webrick.pt/";
            var phoneNumber = "51949277";
            var password = "DataDiagramAnymoreIdentityRebateCuring";
            var cookieName = "__is_loggedin";


                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

                driver.Navigate().GoToUrl(frontpageUrl);

                var menuButtonBy = By.XPath("/html/body/div[1]/div/header/div/div/div[2]/button[2]");
                var loginButtonBy = By.XPath("/html/body/div[3]/div[3]/ul/li");

                wait.Until(ExpectedConditions.ElementExists(menuButtonBy)).Click();

                System.Threading.Thread.Sleep(200);
                wait.Until(ExpectedConditions.ElementExists(loginButtonBy)).Click();
                wait.Until(ExpectedConditions.UrlContains(loginUrl));
                System.Threading.Thread.Sleep(200);

                driver.FindElement(By.Name("UserEnteredPhoneNumber")).SendKeys(phoneNumber);
                driver.FindElement(By.Name("Password")).SendKeys(password);
                driver.FindElement(By.Name("button")).Click();

                var cookie = wait.WaitForCookie(cookieName);

        }
    }
}
