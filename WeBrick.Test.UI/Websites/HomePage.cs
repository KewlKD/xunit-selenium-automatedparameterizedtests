using OpenQA.Selenium;
using System.Collections.Generic;
using WeBrick.Test.UI.Extensions;

namespace WeBrick.Test.UI.Websites
{
    public class HomePage : Website
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
                return Driver.FindElement(By.XPath(@"//*[@id=""price""]"));
            }
        }

        public static By ByFooter => By.TagName("footer");
        public IWebElement Footer => Driver.FindElement(ByFooter);

        public static By ByFooterContactInfoElement => By.XPath(".//div/div/div[1]");
        public IWebElement FooterContactInfoElement => Footer.FindElement(ByFooterContactInfoElement);

        public static By ByFooterAboutWebrickElement => By.XPath(".//div/div/div[2]");
        public IWebElement FooterAboutWebrickElement => Footer.FindElement(ByFooterAboutWebrickElement);


        public static By ByFooterAboutWebrickLinkElements => By.TagName("a");
        public IList<IWebElement> FooterAboutWebrickLinkElements => Footer.FindElements(By.TagName("a"));

        public static By BySocialMediaElement => By.XPath(".//div/div/div[6]");
        public IWebElement SocialMediaElement => Footer.FindElement(BySocialMediaElement);

        public static By ByVideo => By.TagName("iframe");
        public IWebElement Video => Driver.FindElement(ByVideo);


        public static By BySocialMediaLinkElements => By.TagName("a");
        public IList<IWebElement> SocialMediaLinkElements => SocialMediaElement.FindElements(BySocialMediaLinkElements);

        public static By ByOfferButtonOne => By.XPath(@"//*[@id=""__next""]/div[1]/section[3]/div/div/div[2]/div/div[1]/div[1]/button");

        public IWebElement OfferButtonOne => Driver.FindElement(ByOfferButtonOne);


        public static By BySendOrderDialogueButton => By.XPath(@"/html/body/div[3]/div[3]/div/div[2]/div/div[2]/button");

        public IWebElement SendOrderDialogueButton => Driver.FindElement(BySendOrderDialogueButton);
        public static By ByOfferButtonTwo => By.XPath(@"//*[@id=""__next""]/div[1]/section[3]/div/div/div[2]/div/div[2]/div[1]/button");

        public IWebElement OfferButtonTwo => Driver.FindElement(ByOfferButtonTwo);


        public static By ByOfferButtonThree => By.XPath(@"//*[@id=""__next""]/div[1]/section[3]/div/div/div[2]/div/div[3]/div[1]/button");

        public IWebElement OfferButtonThree => Driver.FindElement(ByOfferButtonThree);


        public static By ByOfferButtonFour => By.XPath(@"//*[@id=""__next""]/div[1]/section[3]/div/div/div[3]/button");

        public IWebElement OfferButtonFour => Driver.FindElement(ByOfferButtonFour);

        public IWebElement OfferTextBox => Driver.FindElements("input", "MuiInputBase-input MuiInputBase-inputAdornedStart MuiInputBase-inputAdornedEnd", "", "", null, 1, 1, "")[0];

        public override string Url => Configuration.Website;

        public static By ByPropertyAdvancedSearch => By.XPath(@"//*[@id=""__next""]/ div/div[1]/div/div[2]/button[2]/span[1]");
        public IWebElement PropertyAdvancedSearch => Driver.FindElement(ByPropertyAdvancedSearch);

        public static By BySearchHomeLink => By.XPath(@"//*[@id=""__next""]/div[1]/header/div/div/div[1]/ul/li[1]/a");
        public IWebElement SearchHomeLink => Driver.FindElement(BySearchHomeLink);


        public static By BySelfSaleHomeLink => By.XPath(@"//*[@id=""__next""]/div[1]/header/div/div/div[1]/ul/li[2]/a");
        public IWebElement SelfSaleHomeLink => Driver.FindElement(BySelfSaleHomeLink);

        //
        public static By ByLoanHomeLink => By.XPath(@"//*[@id=""__next""]/div/header/div/div/div[1]/ul/li[3]/a");
        public IWebElement LoanHomeLink => Driver.FindElement(ByLoanHomeLink);
        //
        public static By BySearchHomeButton => By.XPath(@"//*[@id=""__next""]/div[1]/div/div/div[1]/div/div/button/span[1]");
        public IWebElement SearchHomeButton => Driver.FindElement(BySearchHomeButton);

        public static By ByPropertySellLink => By.XPath(@"//*[@id=""__next""]/div/footer/div/div/div[2]/ul/a[1]");
        public IWebElement PropertySellLink => Driver.FindElement(ByPropertySellLink);

        public static By ByBuyPropertyLink => By.XPath(@"//*[@id=""__next""]/div/footer/div/div/div[2]/ul/a[2]");
        public IWebElement BuyPropertyLink => Driver.FindElement(ByBuyPropertyLink);

        public static By ByQuestionsAnswersLink => By.XPath(@"//*[@id=""__next""]/div/footer/div/div/div[2]/ul/a[3]");
        public IWebElement QuestionsAnswersLink => Driver.FindElement(ByQuestionsAnswersLink);

        public HomePage()
        {
        }

        public HomePage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}





