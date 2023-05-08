using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace WeBrick.Test.UI
{
    public abstract class Website
    {
        public abstract string Url { get; }
        public abstract By AcceptAllCoockiesId { get; }
        public abstract IWebElement AcceptAllCoockies { get; }
        public abstract IWebDriver Driver { get; internal set; }
        public virtual IWebElement Body

        {
            get
            {
                return Driver.FindElement(By.TagName("body"));
            }
        }

        public IWebElement Html => Driver.FindElement(By.TagName("html"));
        public IWebElement Head => Driver.FindElement(By.TagName("head"));
        public List<IWebElement> MetaList => Driver.FindElements(By.TagName("meta")).ToList();

        public static By ByMenuButtonBy => By.XPath(@"/html/body/div[1]/div/header/div/div/div[2]/button[2]");
        public IWebElement MenuButtonBy => Driver.FindElement(ByMenuButtonBy);

        public static By ByLoginButtonBy => By.XPath(@"/html/body/div[3]/div[3]/ul/li");
        public IWebElement LoginButtonBy => Driver.FindElement(ByLoginButtonBy);

        public virtual bool IsMyUrl(string url)
        {
            return Regex.Match(url, $@"\b({Url})\b", RegexOptions.IgnoreCase).Success
                && !Regex.Match(url, $@"\b(-{Url})\b", RegexOptions.IgnoreCase).Success;
        }

        public virtual bool IsHomeForSale(string homeUrl)
        {
            Driver.Navigate().GoToUrl(homeUrl);
            if (AcceptAllCoockiesId.Exists(Driver))
            {
                if (AcceptAllCoockies.Displayed)
                {
                    AcceptAllCoockies.Click();
                    Utils.Wait();
                }
                else
                {

                }
            }
            return Body.GetInnerHtml().ToLower().Contains("kontantpris".ToLower());
        }
    }
}
