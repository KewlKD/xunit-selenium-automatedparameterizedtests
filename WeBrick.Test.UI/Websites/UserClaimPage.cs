using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeBrick.Test.UI.Websites
{
    internal class UserClaimPage : Website
    {
        public override string Url => Configuration.UserClaimPage;
        public override By AcceptAllCoockiesId => throw new NotImplementedException();
        public override IWebElement AcceptAllCoockies => throw new NotImplementedException();
        public override IWebDriver Driver { get; internal set; }



        public static By ByUserTable => By.XPath("/html/body/div/main/div[1]/table");
        public IWebElement UserTable => Driver.FindElement(ByUserTable);

        public static By ByUserEmail => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[2]/td[3]");
        public IWebElement UserEmail => Driver.FindElement(ByUserEmail);


        public static By ByUserGivenName => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[4]/td[3]");
        public IWebElement UserGivenName => Driver.FindElement(ByUserGivenName);


        public static By ByUserFamilyName => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[3]/td[3]");
        public IWebElement UserFamilyName => Driver.FindElement(ByUserFamilyName);

        public static By ByUserPhoneNum => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[5]/td[3]");
        public IWebElement UserPhoneNum => Driver.FindElement(ByUserPhoneNum);

        public static By ByUserEmailLabel => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[2]/td[2]");
        public IWebElement UserEmailLabel => Driver.FindElement(ByUserEmailLabel);

        public static By ByUserGivenNameLabel => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[4]/td[2]");
        public IWebElement UserGivenNameLabel => Driver.FindElement(ByUserGivenNameLabel);

        public static By ByUserFamilyNameLabel => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[3]/td[2]");
        public IWebElement UserFamilyNameLabel => Driver.FindElement(ByUserFamilyNameLabel);

        public static By ByUserPhoneNumLabel => By.XPath("/html/body/div/main/div[1]/table/tbody/tr[5]/td[2]");
        public IWebElement UserPhoneNumLabel => Driver.FindElement(ByUserPhoneNumLabel);




        public UserClaimPage(IWebDriver driver)


        {
            Driver = driver;

        }
    }
}
