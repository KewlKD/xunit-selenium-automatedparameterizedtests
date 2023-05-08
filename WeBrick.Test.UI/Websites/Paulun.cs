using OpenQA.Selenium;
namespace WeBrick.Test.UI.Websites
{
    public class Paulun : Website
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
        public IWebElement CachePrice
        {
            get
            {
                return Driver.FindElement(By.XPath(@"/html/body/main/section[3]/div[1]/h5[1]"));
            }
        }
        public override string Url => "paulun.dk";

        public Paulun(IWebDriver driver)
        {
            Driver = driver;
        }

        public Paulun()
        {
        }
    }
}