using OpenQA.Selenium;
using System.Collections.Generic;

namespace WeBrick.Test.UI.Websites
{
    public class ProfilePage : Website
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

        public override string Url => $"{Configuration.Website}profile";

        public ProfilePage()
        {
        }

        public ProfilePage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}





