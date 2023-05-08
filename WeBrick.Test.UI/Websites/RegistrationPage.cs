using OpenQA.Selenium;
using System.Collections.Generic;
using WeBrick.Test.UI.Extensions;

namespace WeBrick.Test.UI.Websites
{
    public class RegistrationPage : Website
    {
        public override IWebDriver Driver { get; internal set; }

        public override IWebElement AcceptAllCoockies
        {
            get
            {
                return Driver.FindElement(AcceptAllCoockiesId);
            }
        }

        public override By AcceptAllCoockiesId
        {
            get
            {
                return By.XPath(@"//*[@id=""coiPage-1""]/div[2]/div[1]/button[3]");
            }
        }
        public override string Url => Configuration.RegistrationPage;

        public static By ByContinueBtn => By.XPath("/html/body/main/div/div/div[2]/form/div/button[2]");
        public IWebElement ContinueBtn => Driver.FindElement(ByContinueBtn);

        public static By ByName => By.XPath("//*[@id=\"name\"]");
        public IWebElement Name => Driver.FindElement(ByName);

        public static By ByNameError => By.XPath("//*[@id=\"name-error\"]");
        public IWebElement NameError => Driver.FindElement(ByNameError);


        public static By BySurname => By.XPath("//*[@id=\"surname\"]");
        public IWebElement Surname => Driver.FindElement(BySurname);
        public static By BySurnameError => By.XPath("//*[@id=\"name-error\"]");
        public IWebElement SurameError => Driver.FindElement(BySurnameError);

        public static By ByPhone => By.XPath("//*[@id=\"phone\"]");
        public IWebElement Phone => Driver.FindElement(ByPhone);
        public static By ByPhoneError => By.XPath("//*[@id=\"phone-error\"]");
        public IWebElement PhoneError => Driver.FindElement(ByPhoneError);

        public static By ByEmail => By.XPath("//*[@id=\"email\"]");
        public IWebElement Email => Driver.FindElement(ByEmail);
        public static By ByEmailError => By.XPath("//*[@id=\"email-error\"]");
        public IWebElement EmailError => Driver.FindElement(ByEmailError);

        public static By ByDivContainsAllFields => By.XPath("/html/body/main/div/div");
        public IWebElement DivContainsAllFields => Driver.FindElement(ByDivContainsAllFields);

        public static By ByTitle => By.XPath("/html/body/main/div/div/div[1]/p");
        public IWebElement Title => Driver.FindElement(ByTitle);

        public IWebElement SmsVerificationTextbox => Driver.FindElements("input", "mdl-textfield__input", "", "", null, 5, 1, "")[0];
        public IWebElement PasswordTextbox => Driver.FindElements("input", "mdl-textfield__input", "", "", null, 6, 1, "")[0];
        public IWebElement VerifyPasswordTextbox => Driver.FindElements("input", "mdl-textfield__input", "", "", null, 7, 1, "")[0];
        public IWebElement AcceptConditionsCheckbox => Driver.FindElements("span", "mdl-checkbox__box-outline", "", "", null, 0, 0, "")[0];
        public RegistrationPage()
        {
        }

        public RegistrationPage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}
