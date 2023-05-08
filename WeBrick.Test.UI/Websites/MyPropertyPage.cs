using OpenQA.Selenium;
using System.Collections.Generic;
using System.Linq;
using WeBrick.Test.UI.Extensions;

namespace WeBrick.Test.UI.Websites
{
    public class MyPropertyPage : Website
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

        public override string Url => $"{Configuration.Website}my-properties/{Configuration.SampleProperty1}";
        public IWebElement Price => Driver.FindElements("span", "MuiTypography-root MuiTypography-h4 MuiTypography-gutterBottom", "kr.", ",-", new Dictionary<string, string>() { { "style", "color: rgb(0, 0, 0); font-weight: bold;" } }, 0, 0, "svg").First();
        public static By ByPriceModalField => By.Id("outlined-price");
        public IWebElement PriceModalField => Driver.FindElement(ByPriceModalField);
        public static By ByEditPriceIcon => By.XPath(@"//*[@id=""__next""]/div/div/section[2]/div/div/div[1]/div/div[3]/div/div/div/div/button");
        public IWebElement EditPriceIcon => Driver.FindElements("span", "MuiIconButton-label", "", "", null, 2, 1, "svg").First();
        public IWebElement ClaimHomeButton => Driver.FindElements("button", "MuiButtonBase-root MuiButton-root MuiButton-outlined MuiButton-outlinedSecondary MuiButton-outlinedSizeLarge MuiButton-sizeLarge MuiButton-fullWidth", "", "", null, 1, 0, "")[0];
        public IWebElement UnClaimHomeButton => Driver.FindElements("button", "MuiButtonBase-root MuiButton-root MuiButton-outlined MuiButton-outlinedSecondary MuiButton-outlinedSizeLarge MuiButton-sizeLarge MuiButton-fullWidth", "unclaim", "", null, 2, 0, "")[0];
        public IWebElement ConfirmUnClaimHomeButton => Driver.FindElements("button", "MuiButtonBase-root MuiButton-root MuiButton-contained MuiButton-containedSecondary", "ja", "", null, 0, 0, "")[0];
        public static By ByPriceChangePerM2Button => By.XPath(@"//*[@id=""__next""]/div/div/section[2]/div/div/div[1]/div/div[1]/div/div/div/div/button");
        public IWebElement PriceChangePerM2Button => Driver.FindElement(ByPriceChangePerM2Button);
        public static By ByClaimHomeConfirmationButton => By.XPath(@"/html/body/div[4]/div[3]/div/div[3]/button[2]/span[1]");
        public IWebElement ClaimHomeConfirmationButton => Driver.FindElement(ByClaimHomeConfirmationButton);
        public static By ByClaimHomeNextStepButton => By.XPath(@"/html/body/div[3]/div[3]/div/div[3]/button");
        public IWebElement ClaimHomeNextStepButton => Driver.FindElement(ByClaimHomeNextStepButton);
        public IWebElement ToolTipImagesTitleButton => Driver.FindElements("button", "MuiButtonBase-root MuiFab-root jss75 MuiFab-sizeSmall MuiFab-extended MuiFab-secondary", "weiter", "", null, 1, 1, "")[0];
        public IWebElement ToolTipPriceTitleButton => Driver.FindElements("button", "MuiButtonBase-root MuiFab-root jss75 MuiFab-sizeSmall MuiFab-extended MuiFab-secondary", "weiter", "", null, 1, 1, "")[0];
        public IWebElement ToolTipDescriptionTitleButton => Driver.FindElements("button", "MuiButtonBase-root MuiFab-root jss75 MuiFab-sizeSmall MuiFab-extended MuiFab-secondary", "weiter", "", null, 1, 1, "")[0];
        public IWebElement ToolTipPersonalTitleButton => Driver.FindElements("button", "MuiButtonBase-root MuiFab-root jss75 MuiFab-sizeSmall MuiFab-extended MuiFab-secondary", "weiter", "", null, 1, 1, "")[0];
        public IWebElement ToolTipDataTitleButton => Driver.FindElements("button", "MuiButtonBase-root MuiFab-root jss75 MuiFab-sizeSmall MuiFab-extended MuiFab-secondary", "weiter", "", null, 1, 1, "")[0];
        public IWebElement ToolTipDocumentsTitleCloseButton => Driver.FindElements("svg", "MuiSvgIcon-root jss80", "", "", null, 1, 1, "")[0];
        public IWebElement UnClaimHomeDialogTitleButton => Driver.FindElements("button", "MuiButtonBase-root MuiFab-root jss75 MuiFab-sizeSmall MuiFab-extended MuiFab-secondary", "weiter", "", null, 1, 1, "")[0];
        public static By ByHomeSizeM2 => By.XPath(@"//*[@id=""__next""]/div[1]/div/section[2]/div/div/div[2]/div/div[2]/div/div/div[1]/div/div/section[2]/table/tbody/tr[2]/td[2]");
        public IWebElement HomeSizeM2 => Driver.FindElement(ByHomeSizeM2);
        public static By ByPropertyAdvancedSearch => By.XPath(@"//*[@id=""__next""]/ div/div[1]/div/div[2]/button[2]/span[1]");
        public IWebElement PropertyAdvancedSearch => Driver.FindElement(ByPropertyAdvancedSearch);
        public static By ByPricePerM2 => By.XPath(@"//*[@id=""__next""]/ div[1]/div/section[2]/div/div/div[1]/div/div[1]/div/div/div/span[2]");
        public IWebElement PricePerM2 => Driver.FindElement(ByPricePerM2);

        public MyPropertyPage(IWebDriver driver)
        {
            Driver = driver;
        }

    }
}

