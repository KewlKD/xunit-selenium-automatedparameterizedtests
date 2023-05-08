using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using WeBrick.Test.UI.Extensions;
using Xunit;
using Xunit.Abstractions;

 namespace WeBrick.Test.UI.UserFlows
{ 
    public partial class SendLoanApplication
    { 

        private const string homeUrl = $"https://{Configuration.Website}property/amaliegade-11-1-1256-kobenhavn-k";
         private const string firstName = "Test";
         private const string lastName = "1337";
        private const string phoneNumber = "123456789";
         private const string emailAddress = "a@b.c";


         // Use as console output, e.g. output.WriteLine( ... )
         private readonly ITestOutputHelper output;

        public SendLoanApplication(ITestOutputHelper output)
         { 
            this.output = output;
        }

        [Fact]
        [Trait("Category", "UserFlow")]
         public void sendLoanApplication()
         { 
             using (IWebDriver driver = Utils.GetChromeDriver(false))
             { 
                // Arrange
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
              var calculateLoanButtonBy = By.XPath("//*[text()='Beregn lån']");
                var applyForLoanButtonBy = By.XPath("//*[text()='Ansøg om lånebevis']");
                 var applyButtonBy = By.XPath("//*[text()='Ansøg']");
                 var applicationRecievedBy = By.XPath("//*[text()='Bestilling modtaget']");

                 Utils.Login(driver);

              // Act
                driver.Navigate().GoToUrl(homeUrl);
                 wait.Until(ExpectedConditions.ElementIsVisible(calculateLoanButtonBy)).Click();
                 wait.Until(ExpectedConditions.ElementIsVisible(applyForLoanButtonBy)).Click();
                 wait.Until(ExpectedConditions.ElementIsVisible(applyButtonBy));

                 driver.FindElement(By.Id("firstname")).SendKeys(firstName);
                driver.FindElement(By.Id("lastname")).SendKeys(lastName);
                driver.FindElement(By.Id("phone")).SendKeys(phoneNumber);
                 driver.FindElement(By.Id("email")).SendKeys(emailAddress);

                 driver.FindElement(applyButtonBy).Click();

                 // Assert

                //xUnit recommends just calling the function that would throw an exception in case
                 //of a failure instead ofwrapping the call in an Assert
               wait.Until(ExpectedConditions.ElementIsVisible(applicationRecievedBy));

           }
         }
     }
 }
